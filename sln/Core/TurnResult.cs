namespace TicTacToe.Core;

public enum TurnResult: byte
{
    Success = 0,
    CellDoesNotExists = 2,
    CellIsAlreadyTaken = 3,
    AlreadyHaveWinner = 4
}