namespace Iridium.Utils.Data
{
    using System;

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
        public Point Position { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }

        public Enemy()
        {
            Id = 0;
            Position = new Point();
            Name = string.Empty;
            Speed = 0;
            Health = 0;
        }

        public Enemy(int id, Point position)
        {
            this.Position = position;
            this.Id = id;
        }
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
    }
}
