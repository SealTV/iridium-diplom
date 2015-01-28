using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    using Iridium.Utils.Data;

    public class TestServerConnector : MonoBehaviour, IServerConnector
    {
        private SharedData.GameData[] games;
        private string[] levels;

        public void GetGames()
        {
            StartCoroutine(this.GetGamesDataFromServer());
        }

        public void GetLevels(int gameId)
        {
            StartCoroutine(this.GetLevelsFromServer());
        }

        public void GetLevelData(int gameId, int levelId)
        {
            StartCoroutine(this.GetLevelDataFromServer());
        }

        private IEnumerator GetLevelDataFromServer()
        {
            yield return new WaitForSeconds(1f);
            this.levels = new[] { "firstLevel", "secondLevel" };
            this.OnLevelDataLoaded(new PacketsFromMaster.LevelData(1,1,new byte[0]));
        }

        private IEnumerator GetLevelsFromServer()
        {
            Debug.Log("StartCoroutine");
            yield return new WaitForSeconds(1f);
            this.levels = new[] {"firstLevel", "secondLevel"};
            this.OnLevelsLoaded(new PacketsFromMaster.GameData(1, "test", 2, 5, new[] { "asdas", "asdas", "asdas", "asdas", "asdas"}));
        }

        private IEnumerator GetGamesDataFromServer()
        {
            Debug.Log("StartCoroutine");
            yield return new WaitForSeconds(1f);
            this.games = new[]
            {
                new SharedData.GameData(1, "StarWars - Успей уничтожить всех захватчиков, до того как они достигнут земли", "StarWars"),
                new SharedData.GameData(2, "Вторая Тестовая Игра", "SecondGame")
            };
            this.OnGamesLoaded(this.games);
        }


        public void Init()
        {
        }

        public void Connect(int port, string ip)
        {
        }

        public event GamesLoaded OnGamesLoaded;
        public event LevelsLoaded OnLevelsLoaded;
        public event LevelDataLoaded OnLevelDataLoaded;
    }
}
