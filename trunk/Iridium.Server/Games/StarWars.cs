namespace Iridium.Server.Games
{
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

            Logger.Debug(intput);
            Logger.Debug(codeSource);

            List<Enemy> enemies = GetEnemiesList(enemiesData);
            bool isAlive = true;
            enemyList = new List<string>();
            var code = CodeGenerator.CreateCode<int>(sandbox, CS.Compiler, codeSource,
                                                         new[]
                                                         {
                                                             "Iridium.Utils", 
                                                             "Iridium.Utils.Data",
                                                             "System.Collections.Generic"
                                                         },
                                                         new[]
                                                         {
                                                             string.Format("{0}\\Iridium.Utils.dll", IridiumMasterServer.Configuration.ServerProperties.FilePath),
                                                             string.Format("C:\\Windows\\microsoft.net\\framework\\v4.0.30319\\mscorlib.dll")
                                                         },
                                                         new CodeParameter("Container", typeof(EnemyContainer)));
            while (isAlive && enemies.Count() != 0)
            {
                Logger.Debug("enemies.Count() = {0}", enemies.Count());
                EnemyContainer container = new EnemyContainer()
                {
                    Enemies = enemies.Where(_enemy => _enemy.Position.X <= 15).ToList()
                };
                var enemyId = code.Execute(container);
                Logger.Info(enemyId);
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
                var vector = Point.GetVector(enemy.StartPosition, playerPosition);
                vector = Point.NormalizeVector(vector);
                vector = Point.Multi(vector, enemy.Speed);
                Point.AddPoint(enemy.Position, vector);
            }
        }
    }
}