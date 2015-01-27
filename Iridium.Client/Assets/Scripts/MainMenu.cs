using Iridium.Utils.Data;
using UnityEngine;

namespace Assets.Scripts
{
    using UnityEngine.UI;

    public class MainMenu : MonoBehaviour
    {

        public IServerConnector ServerConnector;
        public Button[] GamesButtons;
        public GameObject GamesPanel;
        public GameObject LevelsPanel;

        private GameData[] games;
        private LevelData[] levels;


        void Start () {
            this.ServerConnector.Init();
            this.ServerConnector.Connect(12, "asdasd");
            this.ServerConnector.OnGamesLoaded += this.OnGamesLoaded;
            this.ServerConnector.OnLevelsLoaded += this.OnLevelsLoaded;
            this.ServerConnector.GetGames();
        }

        private void OnGamesLoaded(GameData[] games)
        {
            this.games = games;
            foreach (var button in GamesButtons)
            {
                button.gameObject.SetActive(false);
            }
            for (int i = 0; i < games.Length; i++)
            {
                this.GamesButtons[i].gameObject.SetActive(true);
                this.GamesButtons[i].image.sprite = Resources.Load<Sprite>(games[i].PictureName);
            }
        }

        private void OnLevelsLoaded(LevelData[] levels)
        {
            
        }

        public void SelectGame(int gameId)
        {
            GamesPanel.SetActive(false);
            LevelsPanel.SetActive(true);
            ServerConnector.Get
        }

        public void SelectLevel(int levelId)
        {
            
        }


        void OnGUI()
        {

        }

        public Rect GetGamePosition(int number)
        {
            return new Rect(300 * (number % 2), 300 * (number / 2), 300, 300);
        }


    }
}
