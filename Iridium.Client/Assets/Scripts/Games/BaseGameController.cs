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
            BoolOperationTypes = new List<Sprite>
                            {
                                                Resources.Load<Sprite>("More"),
                                                Resources.Load<Sprite>("Less"),
                                                Resources.Load<Sprite>("Equals"),
                                                Resources.Load<Sprite>("UnEquals")
                            };
            this.serverConnector = ServerConnector.Instance;
            this.serverConnector.OnAlgorithmResultLoaded += this.OnAlgorithmResultLoaded;
        }

        public string BaseAlgorithm;
        protected abstract void OnAlgorithmResultLoaded(PacketsFromMaster.AlgorithmResult result);

        public void SendGame()
        {
            string algorithm = this.BaseAlgorithm + MainBlock.GetCode();
            this.StartCoroutine(this.serverConnector.StartSendAlgoritm(GlobalData.GameId, GlobalData.LevelId, algorithm)); 
            Debug.Log(algorithm);
        }
    }
}
