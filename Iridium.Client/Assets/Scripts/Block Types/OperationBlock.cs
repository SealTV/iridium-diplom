namespace Scripts.Blocks
{
    using Scripts;
    using UnityEngine;

    [ExecuteInEditMode]
    public class OperationBlock : Block
    {
        public SubBlocksField Field;

        public override void ReSortingLayers(int layer)
        {
            this.CurrentLayerSorting = layer;
            foreach (var block in this.Field.Blocks)
            {
                block.sortingOrder = layer;
            }
            foreach (var text in this.Texts)
            {
                text.sortingOrder = layer + 1;
            }
        }

        public override string GetCode()
        {
            throw new System.NotImplementedException();
        }

        void Start()
        {
            this.LayerSorting = Random.Range(0, 100) * 100;
            this.ReSortingLayers(this.LayerSorting);

        }

        public override float GetHeight()
        {
            return 1.5f;
            Block block;
            float value = 0;
            value += this.Field.GetHeight();

            if (this.Connectors.TryGetValue("OutputConnector", out block)) value += block.GetHeight();

            return value;
        }

        public override float GetWidth()
        {
            return (this.Field.BaseWidth+ this.HeadWidthStretch)* this.transform.lossyScale.x;
        }

        public override void Stretch()
        {
            if (this.Parent != null)
            {
                this.transform.position = this.Parent.Connectors[this.ParentConnector].transform.position;
            }

            this.Field.Stretch(this.HeadWidthStretch, this.HeadHeightStretch);

            base.Stretch();

        }

        private void Update()
        {
        }
    }
}