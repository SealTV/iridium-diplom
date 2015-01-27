namespace Scripts.Blocks
{
    using Scripts;
    using UnityEngine;

    [ExecuteInEditMode]
    public class EqualsBlock : Block
    {
        public SubBlocksField Field;

        public override void ReSortingLayers(int layer)
        {
            this.CurrentLayerSorting = layer; 
            foreach (var connector in this.Connectors)
            {
                connector.Value.ReSortingLayers(layer + 1);
            }
            foreach (var block in this.Field.Blocks)
            {
                block.sortingOrder = layer;
            }
            foreach (var text in this.Texts)
            {
                text.sortingOrder = layer + 1;
            }
        }

        void Start()
        {
            this.LayerSorting = Random.Range(0, 100) * 100;
            this.ReSortingLayers(this.LayerSorting);
            
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