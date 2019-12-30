using System.Diagnostics;

namespace Core.Models
{
    [DebuggerDisplay("{Number} - {State}")]
    public class Cell
    {
        public Cell(int number)
        {
            Number = number;
        }

        public int Number { get; }

        public CellType State { get; set; }
    }
}