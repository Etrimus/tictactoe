using System.Diagnostics;

namespace TicTacToe.Core.Models
{
    [DebuggerDisplay("{Number} - {State}")]
    public class Cell
    {
        internal Cell(ushort number)
        {
            Number = number;
        }

        public ushort Number { get; }

        public CellType State { get; internal set; }
    }
}