using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
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
                .Where(x => !x.ZeroId.HasValue || !x.CrossId.HasValue)
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

            if (game.Board.Winner != CellType.None)
            {
                throw new TicTacToeException("The game already has a winner.");
            }

            if (!game.ZeroId.HasValue || !game.CrossId.HasValue)
            {
                throw new TicTacToeException($"Couldn't make a turn for the game id {gameId}. The game doesn't have all players.");
            }

            var givenPlayerCellType = playerId == game.ZeroId.Value
                ? CellType.Zero
                : playerId == game.CrossId.Value
                    ? CellType.Cross
                    : CellType.None;

            if (givenPlayerCellType == CellType.None)
            {
                throw new TicTacToeException($"The given {playerId} doesn't belongs to the game.");
            }

            var expectedPlayerId = game.Board.NextTurn switch
            {
                CellType.Zero => game.ZeroId.Value,
                CellType.Cross => game.CrossId.Value,
                _ => throw new TicTacToeException($"The game has an invalid {nameof(game.Board.NextTurn)} value.")
            };

            if (expectedPlayerId != playerId)
            {
                throw new TicTacToeException($"The given player can't make a turn. The player belongs to {givenPlayerCellType}. The game awaits a {game.Board.NextTurn} turn.");
            }

            var turnResult = _boardManager.Turn(game.Board, cellNumber);
            if (turnResult == TurnResult.Success)
            {
                await UpdateAsync(game);
            }
            else
            {
                throw new TicTacToeException($"Unsuccessful turn. {turnResult}.");
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
                throw new TicTacToeException($"A game already have a {cellType} player.");
            }

            setPlayerAction(Guid.NewGuid());

            await _repository.UpdateAsync(game);

            return getPlayerFn().Value;
        }
    }
}