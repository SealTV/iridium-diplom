using System.Text;
using Scripts;
using Scripts.Blocks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MainBlock : Block {

        public SubBlocksField HighField;
        public SubBlocksField MiddleField;
        public SubBlocksField LowField;

        // Use this for initialization
        void Start () {
            this.LayerSorting = 0;
            this.ReSortingLayers(this.LayerSorting);
        }
	
        // Update is called once per frame
        void Update () {
        }

        public override float GetHeight()
        {
            return 0;
        }

        public override float GetWidth()
        {
            return 0;
        }

        public override void Stretch()
        {
            float BodyHeightStretch = 2;
            Block block;
            if (this.Connectors.TryGetValue("InnerConnector", out block))
            {
                BodyHeightStretch = block.GetHeight();
                this.HeadWidthStretch = block.GetWidth();
            }


            this.HighField.Stretch(this.HeadWidthStretch, this.HeadHeightStretch - 1.5f);

            this.MiddleField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight());
            this.MiddleField.Stretch(1, BodyHeightStretch -2);

            this.LowField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight() - this.MiddleField.GetHeight());
            this.LowField.Stretch(this.HeadWidthStretch - 2, 0);

            base.Stretch();
        }

        public override void ReSortingLayers(int layer)
        {
        }

        public override string GetCode()
        {
            var result = new StringBuilder();
            Block block;
            if(this.Connectors.TryGetValue("InnerConnector", out block))
            {
                result.Append(block.GetCode());
            }
            result.Append(" return -1;");
            return result.ToString();
        }
    }
}
