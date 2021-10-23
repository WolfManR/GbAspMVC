namespace Infrastructure.Abstractions
{
    public class GameObject
    {
        public GameObject(int x, int y, Orientation orientation)
        {
            X = x;
            Y = y;
            Orientation = orientation;
        }

        public int X { get; protected set; }
        public int Y { get; protected set; }

        public Orientation Orientation { get; protected set; }
    }

    public enum Orientation { North, South, East, West }
}