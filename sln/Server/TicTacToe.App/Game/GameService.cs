using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Core;
using TicTacToe.Dal.Game;
using TicTacToe.Domain;

namespace TicTacToe.App.Game;

public class GameService(GameRepository repository, IMapper mapper, IBoardManager boardManager)
{
    public Task<GameModel> GetAsync(Guid id, bool asNoTracking)
    {
        return repository
            .GetAsync(id, asNoTracking)
            .ContinueWith(x => mapper.Map<GameEntity, GameModel>(x.Result));
    }

    public Task<GameModel[]> GetAsync(Guid[] id)
    {
        return repository
            .Query()
            .Where(x => id.Contains(x.Id))
            .ProjectTo<GameModel>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    public Task<GameModel[]> GetAllAsync()
    {
        return repository
            .GetAll()
            .ProjectTo<GameModel>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    public Task<GameModel[]> GetFreeAsync()
    {
        return repository
            .GetAll()
            .Where(x => !x.ZeroPlayerId.HasValue || !x.CrossPlayerId.HasValue)
            .ProjectTo<GameModel>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    public Task<GameModel[]> GetByPlayerIdAsync(Guid playerId)
    {
        return repository
            .GetAll()
            .Where(x => x.ZeroPlayerId == playerId || x.CrossPlayerId == playerId)
            .ProjectTo<GameModel>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    public Task UpdateAsync(GameModel game)
    {
        return repository.UpdateAsync(mapper.Map<GameModel, GameEntity>(game));
    }

    public Task<GameModel> CreateNewAsync()
    {
        return repository
            .AddAsync(mapper.Map<GameModel, GameEntity>(new GameModel(boardManager.CreateBoard(3))))
            .ContinueWith(x => mapper.Map<GameEntity, GameModel>(x.Result));
    }

    public async Task SetPlayerAsync(Guid gameId, Guid playerId, CellType cellType)
    {
        var game = await repository.GetAsync(gameId);

        if (game == null)
        {
            throw new TicTacToeException("Указанная игра не существует.");
        }

        Func<Guid?> getPlayerFn;
        Action<Guid> setPlayerAction;

        switch (cellType)
        {
            case CellType.Zero:
                getPlayerFn = () => game.ZeroPlayerId;
                setPlayerAction = zeroPlayerId => game.ZeroPlayerId = zeroPlayerId;
                break;
            case CellType.Cross:
                getPlayerFn = () => game.CrossPlayerId;
                setPlayerAction = crossPlayerId => game.CrossPlayerId = crossPlayerId;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null);
        }

        if (getPlayerFn().HasValue)
        {
            throw new TicTacToeException($"К игре уже присоединился {cellType}-участник.");
        }

        setPlayerAction(playerId);

        await repository.UpdateAsync(game);
    }

    public async Task MakeTurnAsync(Guid gameId, Guid playerId, ushort? cellNumber)
    {
        if (!cellNumber.HasValue)
        {
            throw new TicTacToeException("Не указан номер ячейки для хода.");
        }

        var game = await GetAsync(gameId, true);

        if (game == null)
        {
            throw new TicTacToeException($"Игра с указанным {nameof(gameId)} не существует.");
        }

        if (game.Board.Winner != CellType.None)
        {
            throw new TicTacToeException("Игра уже закончилась.");
        }

        if (!game.ZeroPlayerId.HasValue || !game.CrossPlayerId.HasValue)
        {
            throw new TicTacToeException("К игре не присоединились оба участника.");
        }

        if (playerId != game.ZeroPlayerId && playerId != game.CrossPlayerId)
        {
            throw new TicTacToeException("Пользователь не участвует в данной игре и не может совершать ходы.");
        }

        var expectedPlayer = game.Board.NextTurn switch
        {
            CellType.Zero => game.ZeroPlayerId.Value,
            CellType.Cross => game.CrossPlayerId.Value,
            _ => throw new TicTacToeException($"Игра имеет некорректное значение {nameof(game.Board.NextTurn)}.")
        };

        if (expectedPlayer != playerId)
        {
            throw new TicTacToeException("Ирок не может совершить ход, сейчас очередь другого игрока.");
        }

        var turnResult = boardManager.Turn(game.Board, cellNumber.Value);

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