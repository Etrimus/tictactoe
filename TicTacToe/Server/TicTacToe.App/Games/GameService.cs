using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using TicTacToe.Core;
using TicTacToe.Dal.Games;
using TicTacToe.Domain;

namespace TicTacToe.App.Games
{
    public class GameService
    {
        private readonly GameRepository _repository;
        private readonly IMapper _mapper;

        public GameService(GameRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<Game> GetAsync(Guid id)
        {
            return _repository
                .GetAsync(id)
                .ContinueWith(x => _mapper.Map<GameEntity, Game>(x.Result));
        }

        public IQueryable<Game> GetAllAsync()
        {
            return _repository
                .GetAllAsync()
                .ProjectTo<Game>(_mapper.ConfigurationProvider);
        }

        public Task UpdateAsync(Game game)
        {
            return _repository.UpdateAsync(_mapper.Map<Game, GameEntity>(game));
        }

        public Task<Game> CreateNewAsync()
        {
            return _repository
                .AddAsync(_mapper.Map<Game, GameEntity>(new Game()))
                .ContinueWith(x => _mapper.Map<GameEntity, Game>(x.Result));
        }

        public Task<Guid> SetCrossPlayer(Guid gameId)
        {
            return _setPlayer(gameId, CellType.Cross);
        }

        public Task<Guid> SetZeroPlayer(Guid gameId)
        {
            return _setPlayer(gameId, CellType.Zero);
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