using Iridium.Utils.Data;
using UnityEngine;

namespace Assets.Scripts
{
    using UnityEngine.UI;

    public class MainMenu : MonoBehaviour
    {

        public ServerConnector ServerConnector;
        public Button[] GameButtons;
        public Button[] LevelButtons;
        public GameObject GamesPanel;
        public GameObject LevelsPanel;
        public GameObject LoginPanel;
        public InputField LoginField;
        public InputField PasswordField;

        void Start () {
            this.ServerConnector.Init();
            this.ServerConnector.OnGamesLoaded += this.OnGamesLoaded;
            this.ServerConnector.OnLevelsLoaded += this.OnLevelsLoaded;
            this.ServerConnector.OnLevelDataLoaded += this.OnLevelDataLoaded;
            this.ServerConnector.OnConnectedToServer += this.OnConnectedToServer;
            this.ServerConnector.OnLoggedOnServer += this.OnLoggedOnServer;
            this.ServerConnector.Connect(27001, "104.40.216.136");
        }

        public void SelectGame(int gameId)
        {
            Debug.Log("Select Game "+gameId);
            this.ServerConnector.GetLevels(gameId);
        }

        public void SelectLevel(int level)
        {
            this.ServerConnector.GetLevelData(1, 1);
        }

        public void TryLogin()
        {
            this.ServerConnector.Login(this.LoginField.text, this.PasswordField.text);
        }

        private void OnGamesLoaded(SharedData.GameData[] games)
        {
            Debug.Log("GamesLoaded");
            foreach (var button in this.GameButtons)
            {
                button.gameObject.SetActive(false);
            }
            for (int i = 0; i < games.Length; i++)
            {
                this.GameButtons[i].gameObject.SetActive(true);
                Debug.Log(games[i].Description);
                this.GameButtons[i].image.sprite = Resources.Load<Sprite>(games[i].Description);
                Debug.Log("remove");
                Debug.Log(this.GameButtons[i].name);
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
                int levelId = i;
                this.LevelButtons[i].onClick.AddListener(() => this.SelectLevel(levelId));
                this.LevelButtons[i].interactable = (gameData.CompletedLevels > i);
            }
            this.GamesPanel.SetActive(false);
            this.LevelsPanel.SetActive(true);
        }

        private void OnLevelDataLoaded(PacketsFromMaster.LevelData levelData)
        {
            Application.LoadLevel(1);
        }

        private void OnConnectedToServer()
        {
            //this.ServerConnector.Login("BaltX","215435");
        }

        private void OnLoggedOnServer()
        {
            this.LoginPanel.SetActive(false);
            this.GamesPanel.SetActive(true);
            this.ServerConnector.GetGames();
        }

    }
}
