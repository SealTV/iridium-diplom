using UnityEngine;

namespace Assets.Scripts.Games.StarWars
{
    using System.Collections.Generic;
    using System.Linq;
    using Iridium.Utils.Data;
    using SimpleJSON;

    public class StarWarsController : MonoBehaviour
    {
        public GameObject EnemyPrefab;
        private List<GameObject> Enemies;
        // Use this for initialization
        private void Start()
        {
            Enemies = new List<GameObject>();
            PacketsFromMaster.LevelData levelData = GlobalData.LevelData;
            var json = JSON.Parse(levelData.InputParameters);
            var enemiesData = json["input"]["enemies"].AsArray;
            List<Enemy> enemies = this.GetEnemiesList(enemiesData);
            foreach (var enemy in enemies)
            {
                Debug.Log(enemy.Name);
                Enemies.Add(
                    (GameObject)Instantiate(EnemyPrefab, 
                    new Vector3(enemy.StartPosition.X, enemy.StartPosition.Y),
                    Quaternion.identity)
                    );
            }

        }

        public void StartGame()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        private List<Enemy> GetEnemiesList(JSONArray enemiesData)
        {
            var result = new List<Enemy>();
            for (int i = 0; i < enemiesData.Count; i++)
            {
                var data = enemiesData[i];
                result.Add(new Enemy
                {
                    Id = data["id"].AsInt,
                    StartPosition = new Point(
                        data["start_position"]["X"].AsInt,
                        data["start_position"]["Y"].AsInt
                        ),
                    Name = data["name"].ToString(),
                    Speed = data["speed"].AsInt,
                    Health = data["health"].AsInt
                });
            }
            return result;
        }

        internal class Enemy
        {
            public int Id { get; set; }
            public Point StartPosition { get; set; }
            public string Name { get; set; }
            public int Speed { get; set; }
            public int Health { get; set; }

            public Enemy()
            {
                this.Id = 0;
                this.StartPosition = new Point();
                this.Name = string.Empty;
                this.Speed = 0;
                this.Health = 0;
            }

            public Enemy(int id, Point position)
            {
                this.StartPosition = position;
                this.Id = id;
            }
        }
    }
}
