namespace Iridium.Utils.Data
{
    using System;
    using System.Collections.Generic;

    public static class SharedData
    {
        [Serializable]
        public class GameData
        {
            public int Id { get; private set; }
            public string Description { get; private set; }
            public string PictureName { get; private set; }

            public GameData(int id, string description, string pictureName = null)
            {
                this.PictureName = pictureName;
                this.Description = description;
                this.Id = id;
            }
        }
    }

    public class Enemy : MarshalByRefObject
    {
        public int Id { get; set; }
        public Point StartPosition { get; set; }
        public Point Position { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }

        public Enemy()
        {
            this.Id = 0;
            this.StartPosition = new Point(); 
            this.Position = new Point();
            this.Name = string.Empty;
            this.Speed = 0;
            this.Health = 0;
        }

        public Enemy(int id, Point position)
        {
            this.StartPosition = position; 
            this.Position = position;
            this.Id = id;
        }

        public float GetDistance(Point point)
        {
            return Point.VectorLen(Point.GetVector(point, Position));
        }
    }

    public class EnemyContainer : MarshalByRefObject
    {
        public List<Enemy> Enemies;
    }

    public class Point : MarshalByRefObject
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point()
        {
            this.X = 0;
            this.Y = 0;
        }

        public Point(int x, int y)
        {
            this.Y = y;
            this.X = x;
        }

        public static void AddPoint(Point a, Point b)
        {
            a.X += b.X;
            a.Y += b.Y;
        }

        public static Point Multi(Point p, float f)
        {
            return new Point
            {
                X = p.X * f,
                Y = p.Y * f
            };
        }

        public static Point NormalizeVector(Point vector)
        {
            var len = VectorLen(vector);
            return new Point
            {
                X = vector.X / len,
                Y = vector.Y / len
            };
        }

        public static float VectorLen(Point vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Point GetVector(Point a, Point b)
        {
            return new Point
            {
                X = b.X - a.X,
                Y = b.Y - a.Y
            };
        }
    }

    public enum LoginResults
    {
        LoginOk,
        LoginFail,
        UserNotFount,
        PasswordIncorrect
    }

}
