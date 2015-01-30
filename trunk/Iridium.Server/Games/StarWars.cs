namespace Iridium.Server.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NLog;

    using Tech.CodeGeneration;
    using Tech.CodeGeneration.Compilers;

    using Iridium.Utils.Data;

    public class StarWars : Game
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override bool ProcessCode(string codeSource, string intput, Sandbox sandbox, out List<string> enemyList)
        {
            var json = JsonConvert.DeserializeObject<JToken>(intput);
            var enemiesData = json["enemies"].ToArray();

            List<Enemy> enemies = GetEnemiesList(enemiesData);
            bool isAlive = true;
            enemyList = new List<string>();
            while (isAlive && enemies.Count() != 0)
            {
                EnemyContainer container = new EnemyContainer()
                {
                    Enemies = enemies.ToArray()
                };
                var code = CodeGenerator.CreateCode<int>(sandbox, CS.Compiler, codeSource,
                                                         new[] { "Iridium.Utils" },
                                                         new[] { "Iridium.Utils.dll" },
                                                         CodeParameter.Create("container", container));
                var enemyId = code.Execute(container);
                enemyList.Add(enemyId.ToString());
                Enemy enemy = enemies.FirstOrDefault(e => e.Id == enemyId);
                if (enemy != null)
                {
                    enemy.Health--;
                    if (enemy.Health <= 0)
                        enemies.Remove(enemy);
                }

                ChangePositions(enemies);
                if (enemies.Any(e => e.Position.X <= 0f))
                    isAlive = false;
            }

            return isAlive;
        }

        private static List<Enemy> GetEnemiesList(IEnumerable<JToken> enemiesData)
        {
            return (from t in enemiesData
                    select (t)
                    into enemy
                    let point = new Point
                    {
                        X = enemy["position"]["x"].ToObject<float>(),
                        Y = enemy["position"]["y"].ToObject<float>()
                    }
                    select new Enemy
                    {
                        Id = enemy["id"].ToObject<int>(),
                        StartPosition = point,
                        Position = point,
                        Name = enemy["name"].ToString(),
                        Speed = enemy["speed"].ToObject<int>(),
                        Health = enemy["health"].ToObject<int>()
                    }).ToList();
        }

        private static void ChangePositions(IEnumerable<Enemy> enemies)
        {
            Point playerPosition = new Point(0, 5);
            foreach (var enemy in enemies)
            {
                var vector = GetVector(enemy.StartPosition, playerPosition);
                vector = NormalizeVector(vector);
                vector = Multi(vector, enemy.Speed);
                AddPoint(enemy.Position, vector);
            }
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

        private static float VectorLen(Point vector)
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
}