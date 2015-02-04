using UnityEngine;

namespace Assets.Scripts.Games
{
    using Iridium.Utils.Data;
    using SimpleJSON;

    public class Instantianer : MonoBehaviour
    {
        public GameObject StarWarsController;
        public Transform PrototipsPanel;
        public GameObject Scaler;
        public BaseGameController Instance;

        void Start () {
            this.InstantiateBlocks();
            switch (GlobalData.GameId)
            {
                case 1:
                {
                    this.Instance = ((GameObject)Instantiate(this.StarWarsController)).GetComponent<BaseGameController>();
                    this.Instance.PrototipsPanel = this.PrototipsPanel;
                    //inst.GamePanel = this.GamePanel;
                    this.Instance.Scaler = this.Scaler;
                    break;
                }
                default: return;
            }
        }

        public void SendCode()
        {
            Instance.SendGame();
        }

        void InstantiateBlocks()
        {
            PacketsFromMaster.LevelData levelData = GlobalData.LevelData;
            var json = JSON.Parse(levelData.InputParameters);
            JSONArray blocks = json["blocks"].AsArray;
            float height = 0;
            for (int i = 0; i < blocks.Count; i++)
            {
                GameObject instantiate = (GameObject)Instantiate(Resources.Load<GameObject>(blocks[i].Value));
                instantiate.transform.parent = this.PrototipsPanel;
                instantiate.transform.localPosition = new Vector3(0, -height, 0);
                instantiate.transform.localScale = new Vector3(1, 1, 1);
                height += instantiate.GetComponent<BlockPrototip>().Height;
            }
        }

    }
}
