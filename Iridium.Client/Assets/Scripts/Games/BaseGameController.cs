using UnityEngine;

namespace Assets.Scripts.Games
{
    using Iridium.Utils.Data;

    public abstract class BaseGameController : MonoBehaviour
    {

        public Transform PrototipsPanel;
        public GameObject BackGround;
        public Transform GamePanel;
        public MainBlock MainBlock;

        public GameObject Scaler;
        protected PacketsFromMaster.AlgorithmResult Result;

        protected ServerConnector serverConnector;
        protected bool isPlaying;
        public float PlayStep;
        public float Speed;
        // Use this for initialization
        protected void Awake()
        {
            this.serverConnector = ServerConnector.Instance;
            this.serverConnector.OnAlgorithmResultLoaded += this.OnAlgorithmResultLoaded;
        }

        public string BaseAlgorithm;
        protected abstract void OnAlgorithmResultLoaded(PacketsFromMaster.AlgorithmResult result);

        public void SendGame()
        {
            Enemy en = new Enemy();
            string algorithm; //= MainBlock.GetCode();
            algorithm = BaseAlgorithm +
                        " \n var minDistance = 100f;" +
                        " \n Enemy enemy = null;" +
                        " \n foreach(var enemy1 in Enemies){" +
                        " \n if(enemy1.GetDistance(new Point(0,5))<minDistance)" +
                        " \n {" +
                        " \n    minDistance = enemy1.GetDistance(new Point(0,5));" +
                        " \n    enemy = enemy1;" +
                        " \n }" +
                        " \n }" +
                        " \n try{" +
                        " \n return enemy.Id;" +
                        " \n}" +
                        " \n catch" +
                        " \n {return -1;}";
            Debug.Log(algorithm);
            //algorithm = "if (Container.Enemies.Count>0) " +
            //                   "return Container.Enemies[0].Id;" +
            //                   "else " +
            //                   "return -1;";
            //algorithm = " return Container.Enemies[0].Id;";
            Debug.Log(MainBlock.GetCode());
            this.StartCoroutine(this.serverConnector.StartSendAlgoritm(GlobalData.GameId, GlobalData.LevelId, algorithm));
        }
    }

}
