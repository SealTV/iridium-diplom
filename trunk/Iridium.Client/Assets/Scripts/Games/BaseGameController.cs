using UnityEngine;

namespace Assets.Scripts.Games
{
    using System.Collections.Generic;
    using Block_Types;
    using Iridium.Utils.Data;

    public abstract class BaseGameController : MonoBehaviour
    {
        public static BaseGameController Instance;
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
        public List<Sprite> VariableTypes;
        public List<Sprite> BoolOperationTypes; 

        public GameObject InputParameters;
        protected void Awake()
        {
            Instance = this;
            VariableTypes = new List<Sprite>
                            {
                                                Resources.Load<Sprite>("Int"),
                                                Resources.Load<Sprite>("Float"),
                                                Resources.Load<Sprite>("String"),
                                                Resources.Load<Sprite>("Bool")
                            };
            Debug.Log(VariableTypes.Count);
            BoolOperationTypes = new List<Sprite>
                            {
                                                Resources.Load<Sprite>("More"),
                                                Resources.Load<Sprite>("Less"),
                                                Resources.Load<Sprite>("Equals"),
                                                Resources.Load<Sprite>("UnEquals")
                            };
            Debug.Log(BoolOperationTypes.Count);
            this.serverConnector = ServerConnector.Instance;
            this.serverConnector.OnAlgorithmResultLoaded += this.OnAlgorithmResultLoaded;
        }

        public string BaseAlgorithm;
        protected abstract void OnAlgorithmResultLoaded(PacketsFromMaster.AlgorithmResult result);

        public void SendGame()
        {
            Enemy en = new Enemy();
            string algorithm; //= MainBlock.GetCode();
            algorithm = this.BaseAlgorithm +
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
                        " \n }" +
                        " \n catch" +
                        " \n {return -1;}";
            Debug.Log(algorithm);
            //algorithm = "if (Container.Enemies.Count>0) " +
            //                   "return Container.Enemies[0].Id;" +
            //                   "else " +
            //                   "return -1;";
            //algorithm = " return Container.Enemies[0].Id;";

            this.StartCoroutine(this.serverConnector.StartSendAlgoritm(GlobalData.GameId, GlobalData.LevelId, this.MainBlock.GetCode())); 
            Debug.Log(this.MainBlock.GetCode());
        }
    }
}
