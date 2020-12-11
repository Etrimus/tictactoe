using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using TicTacToe.Core;
using TicTacToe.Core.Services;
using TicTacToe.Dal.Games;
using TicTacToe.Domain;

namespace TicTacToe.App.Game
{
    public class GameService
    {
        private readonly GameRepository _repository;
        private readonly IMapper _mapper;
        private readonly BoardManager _boardManager;

        public GameService(GameRepository repository, IMapper mapper, BoardManager boardManager)
        {
            _repository = repository;
            _mapper = mapper;
            _boardManager = boardManager;
        }

        public Task<GameModel> GetAsync(Guid id)
        {
            return _repository
                .GetAsync(id)
                .ContinueWith(x => _mapper.Map<GameEntity, GameModel>(x.Result));
        }

        public IQueryable<GameModel> GetAllAsync()
        {
            return _repository
                .GetAllAsync()
                .ProjectTo<GameModel>(_mapper.ConfigurationProvider);
        }

        public IQueryable<GameModel> GetFreeAsync()
        {
            return _repository
                .GetAllAsync()
                .Where(x => !x.ZeroId.HasValue || !x.CrossId.HasValue)
                .ProjectTo<GameModel>(_mapper.ConfigurationProvider);
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

        public Task<Guid> SetCrossPlayer(Guid gameId)
        {
            return _setPlayer(gameId, CellType.Cross);
        }

        public Task<Guid> SetZeroPlayer(Guid gameId)
        {
            return _setPlayer(gameId, CellType.Zero);
        }

        public async Task MakeTurn(Guid gameId, Guid playerId, ushort cellNumber)
        {
            var game = await GetAsync(gameId);

            if (!game.ZeroId.HasValue || !game.CrossId.HasValue)
            {
                throw new ArgumentException($"Couldn't make a turn for the game id {gameId}. The game don't have all player.");
            }

            Guid? expectedPlayerId;

            switch (game.Board.NextTurn)
            {
                case CellType.Zero:
                    expectedPlayerId = game.ZeroId;
                    break;
                case CellType.Cross:
                    expectedPlayerId = game.CrossId;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"The game has an invalid {nameof(game.Board.NextTurn)} value.");
            }

            if (expectedPlayerId != playerId)
            {
                throw new ArgumentException($"The given {nameof(playerId)} can't make a turn.");
            }

            if (_boardManager.TryTurn(game.Board, cellNumber, out var turnResult) && turnResult == TurnResult.Success)
            {
                await UpdateAsync(game);
            }
            else
            {
                throw new Exception($"Unsuccess turn. {turnResult}");
            }
        }

        private async Task<Guid> _setPlayer(Guid gameId, CellType cellType)
        {
            var game = await _repository.GetAsync(gameId);

            Func<Guid?> getPlayerFn;
            Action<Guid> setPlayerAction;

            switch (cellType)
            {
                case CellType.Zero:
                    getPlayerFn = () => game.ZeroId;
                    setPlayerAction = guid => game.ZeroId = guid;
                    break;
                case CellType.Cross:
                    getPlayerFn = () => game.CrossId;
                    setPlayerAction = guid => game.CrossId = guid;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null);
            }

            if (getPlayerFn().HasValue)
            {
                throw new ArgumentException($"A game already have a {cellType} player.");
            }

            setPlayerAction(Guid.NewGuid());

            await _repository.UpdateAsync(game);

            return getPlayerFn().Value;
        }
    }
}