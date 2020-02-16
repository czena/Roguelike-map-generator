namespace MazeGenerator
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static Point operator + (Point p1, Point p2)
        {
            Point p3=new Point(p1.X+p2.X, p1.Y+p2.Y);
            return p3;
        }
    }
}
