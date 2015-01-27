using Iridium.Utils.Data;
using UnityEngine;

namespace Assets.Scripts
{
    using UnityEngine.UI;

    public class MainMenu : MonoBehaviour
    {

        public TestServerConnector ServerConnector;
        public Button[] GameButtons;
        public Button[] LevelButtons;
        public GameObject GamesPanel;
        public GameObject LevelsPanel;


        void Start () {
            this.ServerConnector.Init();
            this.ServerConnector.OnGamesLoaded += this.OnGamesLoaded;
            this.ServerConnector.OnLevelsLoaded += this.OnLevelsLoaded;
            this.ServerConnector.Connect(12, "asdasd");
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
                int gameId = games[i].Id;
                this.GameButtons[i].onClick.AddListener(() => this.SelectGame(gameId));
            }
        }

        private void OnLevelsLoaded(PacketsFromMaster.GameData gameData)
        {
            foreach (var button in this.LevelButtons)
            {
                button.gameObject.SetActive(false);
            }
            for (int i = 0; i < gameData.Levels.Length; i++)
            {
                this.LevelButtons[i].gameObject.SetActive(true);
                this.LevelButtons[i].onClick.RemoveAllListeners();
                string level = gameData.Levels[i];
                this.LevelButtons[i].onClick.AddListener(() => this.SelectLevel(level));
                this.LevelButtons[i].interactable = gameData.CompletedLevels <= i;
            }
        }

        public void SelectGame(int gameId)
        {
            GamesPanel.SetActive(false);
            LevelsPanel.SetActive(true);
            ServerConnector.GetLevels(gameId);
        }

        public void SelectLevel(string level)
        {
            
        }
    }
}
