using System.Diagnostics;

namespace Core.Models
{
    [DebuggerDisplay("{Number} - {State}")]
    public class Cell
    {
        public Cell(ushort number)
        {
            Number = number;
        }

        public ushort Number { get; }

        public CellType State { get; internal set; }
    }
}