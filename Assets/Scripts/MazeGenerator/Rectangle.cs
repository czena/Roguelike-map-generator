namespace MazeGenerator
{
    public class Rectangle
    {
        public Point StartPosition {get;set;}
        public Point Size { get; set; }
        public Rectangle (Point start, Point size)
        {
            StartPosition = start;
            Size = size;
        }
    }
}
