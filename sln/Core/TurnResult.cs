namespace TicTacToe.Core;

public enum TurnResult : byte
{
    Success = 0,
    CellDoesNotExist = 2,
    CellIsAlreadyTaken = 3,
    AlreadeyHaveWinner = 4
}