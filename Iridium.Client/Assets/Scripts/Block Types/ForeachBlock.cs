namespace Scripts.Blocks
{
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class ForeachBlock : Block
    {
        public SubBlocksField HighField;
        public SubBlocksField MiddleField;
        public SubBlocksField LowField;

        public InputField Iterator;
        public Canvas Canvas;

        public override void ReSortingLayers(int layer)
        {
            this.CurrentLayerSorting = layer;
            foreach (var connector in this.Connectors)
            {
                connector.Value.ReSortingLayers(layer+1);
            }
            foreach (var block in this.HighField.Blocks)
            {
                block.sortingOrder = layer;
            }
            foreach (var block in this.MiddleField.Blocks)
            {
                block.sortingOrder = layer;
            }
            foreach (var block in this.LowField.Blocks)
            {
                block.sortingOrder = layer;
            }
            foreach (var text in this.Texts)
            {
                text.sortingOrder = layer + 1;
            }
            Canvas.sortingOrder = layer + 1;
        }

        public override string GetCode()
        {
            var result = new StringBuilder();
            Block block;
            if (this.Connectors.TryGetValue("ConditionConnector", out block))
            {
                string.Format("foreach(var {0} in {1}){/n {2}/n}", 
                    Iterator.text, 
                    block.GetCode(),
                    this.Connectors.TryGetValue("InnerConnector", out block) 
                        ? block.GetCode() 
                        : string.Empty);
            }
            return result.ToString();
        }

        void Start()
        {
            this.LayerSorting = Random.Range(0, 100) * 100;
            this.ReSortingLayers(this.LayerSorting);
        }

        public override float GetHeight()
        {
            float value = 0;
            value += this.HighField.GetHeight();
            value += this.LowField.GetHeight();
            Block block;
            if (this.Connectors.TryGetValue("InnerConnector", out block)) value += block.GetHeight();
            else value += this.MiddleField.GetHeight();

            if (this.Connectors.TryGetValue("OutputConnector", out block)) value += block.GetHeight();

            return value;
        }

        public override float GetWidth()
        {
            return HeadWidthStretch;
        }

        public override void Stretch()
        {
            Debug.Log("stretch");
            float BodyHeightStretch = 0;

            //HeadWidthStretch = 2;
            Block block;
            if (this.Connectors.TryGetValue("InnerConnector", out block)) BodyHeightStretch += block.GetHeight();
            if (this.Connectors.TryGetValue("ConditionConnector", out block))
            {
                HeadHeightStretch = block.GetHeight();
                HeadWidthStretch +=block.GetWidth();
            }
            else
            {
                HeadHeightStretch = 0;
            }
            if (this.Parent != null)
            {
                this.transform.position = this.Parent.Connectors[this.ParentConnector].transform.position;
            }

            this.HighField.Stretch(HeadWidthStretch, HeadHeightStretch-1.5f);

            this.MiddleField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight());
            this.MiddleField.Stretch(1, BodyHeightStretch-2);

            this.LowField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight() - this.MiddleField.GetHeight());
            this.LowField.Stretch(HeadWidthStretch-2, 0);

            base.Stretch();
        }

        private void Update()
        { 
        }
    }
}