namespace Assets.Scripts
{
    using UnityEngine;

    [ExecuteInEditMode]
    public class ForeachBlock : Block
    {
        public SubBlocksField HighField;
        public SubBlocksField MiddleField;
        public SubBlocksField LowField;

        void Start()
        {
            int layer = Random.Range(0, 10000);
            foreach (var block in HighField.Blocks)
            {
                block.GetComponent<SpriteRenderer>().sortingOrder = layer;
            }
            foreach (var block in MiddleField.Blocks)
            {
                block.GetComponent<SpriteRenderer>().sortingOrder = layer;
            }
            foreach (var block in LowField.Blocks)
            {
                block.GetComponent<SpriteRenderer>().sortingOrder = layer;
            }
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
            return 0;
        }

        public override void Streach()
        {
            float BodyHeightStretch = 0;
            Block block;
            if (this.Connectors.TryGetValue("InnerConnector", out block)) BodyHeightStretch = block.GetHeight();
            if (this.Parent != null)
            {
                this.transform.position = this.Parent.Connectors[this.ParentConnector].transform.position;
            }

            this.HighField.Stretch(this.HeadWidthStretch, this.HeadHeightStretch);

            this.MiddleField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight());
            this.MiddleField.Stretch(1, BodyHeightStretch-2);

            this.LowField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight() - this.MiddleField.GetHeight());
            this.LowField.Stretch(this.HeadWidthStretch-2, 0);
        }

        private void Update()
        { 
            this.Streach();
        }
    }
}