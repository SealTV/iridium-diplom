using Iridium.Utils.Data;
using UnityEngine;

namespace Assets.Scripts
{
    using System.Threading;
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

        private Thread workTread;

        void Start () {
            this.ServerConnector.Init();
            this.ServerConnector.OnGamesLoaded += this.OnGamesLoaded;
            this.ServerConnector.OnLevelsLoaded += this.OnLevelsLoaded;
            this.ServerConnector.OnLevelDataLoaded += this.OnLevelDataLoaded;
            this.ServerConnector.OnConnectedToServer += this.OnConnectedToServer;
            this.ServerConnector.OnLoggedOnServer += this.OnLoggedOnServer;
            this.StartCoroutine(this.ServerConnector.StartConnectServer(27001, "104.40.216.136"));
            //this.ServerConnector.Connect(27001, "104.40.216.136");
            //this.ServerConnector.Connect(27001, "127.0.0.1");
        }

        public void SelectGame(int gameId)
        {
            Debug.Log("Select Game: " + gameId);
            this.StartCoroutine(this.ServerConnector.StartGetLevels(gameId));
        }

        public void SelectLevel(int game, int level)
        {
            Debug.Log("Select level: "+ level);
            this.StartCoroutine(this.ServerConnector.StartGetLevelData(game, level));
        }

        public void TryLogin()
        {
            Debug.Log("adasd");
            this.StartCoroutine(this.ServerConnector.StartLogin(this.LoginField.text, this.PasswordField.text));
        }

        private void OnConnectedToServer()
        {
            this.LoginPanel.SetActive(true);
        }
        private void OnLoggedOnServer()
        {
            this.LoginPanel.SetActive(false);
            this.GamesPanel.SetActive(true);
            this.StartCoroutine(this.ServerConnector.StartGetGames());
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
                int levelId = i+1;
                this.LevelButtons[i].onClick.AddListener(() => this.SelectLevel(gameData.GameId, levelId));
                this.LevelButtons[i].interactable = (gameData.CompletedLevels > i);
            }
            this.GamesPanel.SetActive(false);
            this.LevelsPanel.SetActive(true);
        }
        private void OnLevelDataLoaded()
        {
            Application.LoadLevel(1);
        }



    }
}
