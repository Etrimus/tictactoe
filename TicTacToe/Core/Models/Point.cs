namespace Core.Models
{
    public class Point
    {
        public Point(ushort x, ushort y)
        {
            X = x;
            Y = y;
        }

        public readonly ushort X;

        public readonly ushort Y;
    }
}