using System;
using System.Drawing;

namespace UlearnGame.Utilities
{
    public struct Vector
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void SetPosition(Vector vec)
        {
            X = vec.X;
            Y = vec.Y;
        }
        public void SetPosition(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public static Vector operator +(Vector vec1, Vector vec2) => new Vector(vec1.X + vec2.X, vec1.Y + vec2.Y);
        public static Vector operator -(Vector vec1, Vector vec2) => new Vector(vec1.X - vec2.X, vec1.Y - vec2.Y);
        public static bool operator ==(Vector vec1, Vector vec2)
        {
            return vec1.X == vec2.X && vec1.Y == vec2.Y;
        }
        public static bool operator !=(Vector vec1, Vector vec2)
        {
            return !(vec1 == vec2);
        }

        public int Length => (int)Math.Sqrt(X * X + Y * Y);
        public Point ToPoint() => new Point(X, Y);
        public int Distance(Vector other) => (int)Math.Sqrt((other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y));
    }
}
