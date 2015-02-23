namespace Scripts.Games.StarWars
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts;
    using Assets.Scripts.Games;
    using Assets.Scripts.Games.StarWars;
    using Iridium.Utils.Data;
    using SimpleJSON;
    using UnityEngine;

    public class StarWarsController : BaseGameController
    {
        public GameObject EnemyPrefab;
        public List<EnemyController> Enemies;
        public ParticleSystem Bullet;
        public float BulletSpeed;
        public List<GameObject> EnemyPrefabs; 
        private int[] destroyingIds;
        private readonly Vector3 mainShipPosition = new Vector3(0,5);
        private int currentDestroyingEnemy = -1;
        private int currentStep = -1;

        private void Awake()
        {
            base.Awake();

            this.VariableTypes.Add(Resources.Load<Sprite>("Enemy"));
            PacketsFromMaster.LevelData levelData = GlobalData.LevelData;
            var json = JSON.Parse(levelData.InputParameters);
            List<Enemy> enemiesFromJson = this.GetEnemiesList(json["input"]["enemies"].AsArray);

            this.Enemies = new List<EnemyController>();
            foreach (var enemyJson in enemiesFromJson)
            {
                var instance = (GameObject) Instantiate(this.EnemyPrefabs.Find(x => x.name == enemyJson.Name));
                var instController = instance.GetComponent<EnemyController>();

                instController.Speed = enemyJson.Speed;
                instController.StartPosition = new Vector3(enemyJson.StartPosition.X, enemyJson.StartPosition.Y);
                instController.Direction = (this.mainShipPosition - instController.StartPosition).normalized;
                instController.Id = enemyJson.Id;
                instController.HP = enemyJson.Health;

                this.Enemies.Add(instController);

                instance.transform.parent = this.GamePanel;
                instance.transform.localPosition = new Vector3(enemyJson.StartPosition.X, enemyJson.StartPosition.Y);
            }

        }

        protected override void OnAlgorithmResultLoaded(PacketsFromMaster.AlgorithmResult result)
        {
            this.Result = result;
            this.BackGround.SetActive(true);
            this.Scaler.SetActive(false);
            this.GamePanel.gameObject.SetActive(true);
            
            this.destroyingIds = new int[result.Output.Length];
            foreach (var enemy in this.Enemies)
            {
                Debug.Log(enemy.name);
                enemy.gameObject.SetActive(true);
            }
            for (int i = 0; i < result.Output.Length; i++)
            {
                this.destroyingIds[i] = Convert.ToInt32(result.Output[i]);
                Debug.Log("output: " + this.destroyingIds[i]);
            }
            this.PlayStep = 0;
            this.isPlaying = true;
            this.currentStep = 0;
            this.Bullet.gameObject.SetActive(destroyingIds[0]!=-1);
            this.Bullet.transform.localPosition = mainShipPosition;
            foreach (var enemy in this.Enemies)
            {
                enemy.transform.localPosition = enemy.StartPosition;
            }
            this.currentDestroyingEnemy = -1;
        }



        private void Update()
        {
            if (this.isPlaying)
            {
                this.PlayStep += (Time.deltaTime*this.Speed);
                if (this.PlayStep > this.destroyingIds.Length)
                {
                    this.isPlaying = false;
                    this.Bullet.gameObject.SetActive(false);
                    this.BackGround.SetActive(false);
                    this.Scaler.SetActive(true);
                    this.GamePanel.gameObject.SetActive(false);
                    return;
                }
                
                int enemyNumber = destroyingIds[(int) this.PlayStep];
                if (currentStep + 1 < this.PlayStep)
                {
                    Bullet.gameObject.SetActive(enemyNumber!=-1);
                    currentStep++;
                }
                if (enemyNumber != this.currentDestroyingEnemy) 
                {
                    if (this.currentDestroyingEnemy >= 0)
                        Enemies[this.currentDestroyingEnemy].gameObject.SetActive(false);
                    this.currentDestroyingEnemy = enemyNumber;
                }
                foreach (var enemy in this.Enemies)
                {
                    enemy.transform.localPosition = enemy.StartPosition + enemy.Direction * this.PlayStep * enemy.Speed;
                }
                if (enemyNumber >= 0)
                {
                    if (!Bullet.gameObject.activeSelf) return;

                    this.Bullet.transform.localPosition -= (Enemies[enemyNumber].Direction.normalized*BulletSpeed*Speed);
                    if (Bullet.transform.localPosition.x > this.Enemies[enemyNumber].transform.localPosition.x)
                    {
                        this.Bullet.gameObject.SetActive(false);
                        this.Enemies[enemyNumber].HP--;
                        if (Enemies[enemyNumber].HP <= 0) Enemies[enemyNumber].gameObject.SetActive(false);
                        this.Bullet.transform.localPosition = mainShipPosition;
                    }
                    else
                    {
                        this.Bullet.gameObject.SetActive(true);
                    }
                }
            }
        }

        private List<Enemy> GetEnemiesList(JSONArray enemiesData)
        {
            var result = new List<Enemy>();
            for (int i = 0; i < enemiesData.Count; i++)
            {
                var data = enemiesData[i];
                Debug.Log(data);
                result.Add(new Enemy
                {
                    Id = data["id"].AsInt,
                    StartPosition = new Point(
                        data["position"]["x"].AsInt,
                        data["position"]["y"].AsInt
                        ),
                    Name = data["name"].Value,
                    Speed = data["speed"].AsInt,
                    Health = data["health"].AsInt
                });
            }
            return result;
        }
    }
}
