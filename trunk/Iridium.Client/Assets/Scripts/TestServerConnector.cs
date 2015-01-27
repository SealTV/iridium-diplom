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
            StartCoroutine(this.GetGameDatasFromServer());
        }

        public void GetLevels(int gameId)
        {
            StartCoroutine(this.GetLevelDatasFromServer());
        }

        private IEnumerator GetLevelDatasFromServer()
        {
            Debug.Log("StartCoroutine");
            yield return new WaitForSeconds(3f);
            this.levels = new[] {"firstLevel", "secondLevel"};
            this.OnLevelsLoaded(levels);
        }

        private IEnumerator GetGameDatasFromServer()
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
    }
}
