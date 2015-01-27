using Iridium.Utils.Data;
using UnityEngine;

namespace Assets.Scripts
{
    using UnityEngine.UI;

    public class MainMenu : MonoBehaviour
    {

        public IServerConnector ServerConnector;
        public Button[] GameButtons;
        public Button[] LevelButtons;
        public GameObject GamesPanel;
        public GameObject LevelsPanel;

        private string[] levels;


        void Start () {
            this.ServerConnector.Init();
            this.ServerConnector.Connect(12, "asdasd");
            this.ServerConnector.OnGamesLoaded += this.OnGamesLoaded;
            this.ServerConnector.OnLevelsLoaded += this.OnLevelsLoaded;
            this.ServerConnector.GetGames();
        }

        private void OnGamesLoaded(SharedData.GameData[] games)
        {
            foreach (var button in this.GameButtons)
            {
                button.gameObject.SetActive(false);
            }
            for (int i = 0; i < games.Length; i++)
            {
                this.GameButtons[i].gameObject.SetActive(true);
                this.GameButtons[i].image.sprite = Resources.Load<Sprite>(games[i].PictureName);
                this.GameButtons[i].onClick.RemoveAllListeners();
                this.GameButtons[i].onClick.AddListener(()=>this.SelectGame(games[i].Id));
            }
        }

        private void OnLevelsLoaded(string[] levels)
        {
            
        }

        public void SelectGame(int gameId)
        {
            GamesPanel.SetActive(false);
            LevelsPanel.SetActive(true);
            ServerConnector.GetLevels(gameId);
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
