using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    using Iridium.Utils.Data;

    public class TestServerConnector : MonoBehaviour, IServerConnector
    {
        private GameData[] games;
        private LevelData[] levels;

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
            this.levels = new[]
            {
                new LevelData(1, 1, "StarWars - Успей уничтожить всех захватчиков, до того как они достигнут земли", "StarWars"), 
                new LevelData(1, 2, "Вторая Тестовая Игра", "SecondGame")
            };
            this.OnGamesLoaded(this.games);

        }

        private IEnumerator GetGameDatasFromServer()
        {
            Debug.Log("StartCoroutine");
            yield return new WaitForSeconds(3f);
            this.games = new[]
            {
                new GameData(1, "StarWars - Успей уничтожить всех захватчиков, до того как они достигнут земли", "StarWars"),
                new GameData(2, "Вторая Тестовая Игра", "SecondGame")
            };
            this.OnGamesLoaded(this.games);

        }
        public bool TryGetLevels(ref LevelData[] gameDatas)
        {
            return true;
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
