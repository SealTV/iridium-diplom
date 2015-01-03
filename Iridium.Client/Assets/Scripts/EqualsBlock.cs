namespace Assets.Scripts
{
    using UnityEngine;

    [ExecuteInEditMode]
    public class EqualsBlock : Block
    {
        public SubBlocksField Field;


        void Start()
        {
            int layer = Random.Range(0, 10000);
            foreach (var block in this.Field.Blocks)
            {
                block.GetComponent<SpriteRenderer>().sortingOrder = layer;
            }
        }

        public override float GetHeight()
        {
            Block block;
            float value = 0;
            value += this.Field.GetHeight();

            if (this.Connectors.TryGetValue("OutputConnector", out block)) value += block.GetHeight();

            return value;
        }

        public override float GetWidth()
        {
            return 0;
        }

        public override void Streach()
        {
            if (this.Parent != null)
            {
                this.transform.position = this.Parent.Connectors[this.ParentConnector].transform.position;
            }

            this.Field.Stretch(this.HeadWidthStretch, this.HeadHeightStretch);

        }

        private void Update()
        {
            this.Streach();
        }
    }
}