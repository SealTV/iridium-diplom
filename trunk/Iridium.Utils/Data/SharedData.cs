﻿namespace Iridium.Utils.Data
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

        public class Enemy : MarshalByRefObject
        {
            public int Id { get; private set; }
            public Point Position { get; private set; }

            public Enemy(int id, Point position)
            {
                this.Position = position;
                this.Id = id;
            }
        }

        public class Point : MarshalByRefObject
        {
            public int X{ get; private set; }
            public int Y{ get; private set; }

            public Point(int x, int y)
            {
                this.Y = y;
                this.X = x;
            }
        }
    }
}