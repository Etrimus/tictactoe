using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TicTacToe.App.User;
using TicTacToe.Core;
using TicTacToe.Core.Services;
using TicTacToe.Dal.Game;
using TicTacToe.Dal.User;
using TicTacToe.Domain;

namespace TicTacToe.App.Game
{
    public class GameService
    {
        private readonly GameRepository _repository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly BoardManager _boardManager;

        public GameService(GameRepository repository, UserRepository userRepository, IMapper mapper, BoardManager boardManager)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
            _boardManager = boardManager;
        }

        public Task<GameModel> GetAsync(Guid id, bool asNoTracking)
        {
            return _repository
                .GetAsync(id, asNoTracking)
                .ContinueWith(x => _mapper.Map<GameEntity, GameModel>(x.Result));
        }

        public Task<GameModel[]> GetAllAsync()
        {
            return _repository
                .GetAllAsync()
                .ProjectTo<GameModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public Task<GameModel[]> GetFreeAsync()
        {
            return _repository
                .GetAllAsync()
                .Where(x => x.ZeroPlayer == null || x.CrossPlayer == null)
                .ProjectTo<GameModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public Task<GameModel[]> GetByUserAsync(Guid playerId)
        {
            return _repository
                .GetAllAsync()
                .Where(x => x.ZeroPlayer.Id == playerId || x.CrossPlayer.Id == playerId)
                .ProjectTo<GameModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public Task UpdateAsync(GameModel game)
        {
            return _repository.UpdateAsync(_mapper.Map<GameModel, GameEntity>(game));
        }

        public Task<GameModel> CreateNewAsync()
        {
            return _repository
                .AddAsync(_mapper.Map<GameModel, GameEntity>(new GameModel(_boardManager.CreateBoard(3))))
                .ContinueWith(x => _mapper.Map<GameEntity, GameModel>(x.Result));
        }

        public async Task SetPlayerAsync(Guid gameId, CellType cellType, UserModel player)
        {
            var game = await _repository.GetAsync(gameId);
            if (game == null)
            {
                throw new TicTacToeException("Указанная игра не существует.");
            }

            var playerEntity = await _userRepository.GetAsync(player.Name, true);

            Func<UserEntity> getPlayerFn;
            Action<UserEntity> setPlayerAction;

            switch (cellType)
            {
                case CellType.Zero:
                    getPlayerFn = () => game.ZeroPlayer;
                    setPlayerAction = userEntity => game.ZeroPlayerId = userEntity.Id;
                    break;
                case CellType.Cross:
                    getPlayerFn = () => game.CrossPlayer;
                    setPlayerAction = userEntity => game.CrossPlayerId = userEntity.Id;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null);
            }

            if (getPlayerFn() != null)
            {
                throw new TicTacToeException($"К игре уже присоединился {cellType}-участник.");
            }

            setPlayerAction(playerEntity);

            await _repository.UpdateAsync(game);
        }

        public async Task MakeTurn(Guid gameId, UserModel player, ushort cellNumber)
        {
            var game = await GetAsync(gameId, true);

            if (game.Board.Winner != CellType.None)
            {
                throw new TicTacToeException("Игра уже закончилась.");
            }

            if (game.ZeroPlayer == null || game.CrossPlayer == null)
            {
                throw new TicTacToeException($"К игре не присоединились оба участника.");
            }

            if (player.Id != game.ZeroPlayer.Id && player.Id != game.CrossPlayer.Id)
            {
                throw new TicTacToeException($"Пользователь не участвует в данной игре и не может совершать ходы.");
            }

            var expectedPlayer = game.Board.NextTurn switch
            {
                CellType.Zero => game.ZeroPlayer,
                CellType.Cross => game.CrossPlayer,
                _ => throw new TicTacToeException($"Игра имеет некорректное значение {nameof(game.Board.NextTurn)}.")
            };

            if (expectedPlayer.Id != player.Id)
            {
                throw new TicTacToeException($"Ирок не может совершить ход, сейчас очередь другого игрока.");
            }

            var turnResult = _boardManager.Turn(game.Board, cellNumber);
            if (turnResult == TurnResult.Success)
            {
                await UpdateAsync(game);
            }
            else
            {
                throw new TicTacToeException($"Неудавшийся ход - {turnResult}.");
            }
        }
    }
}