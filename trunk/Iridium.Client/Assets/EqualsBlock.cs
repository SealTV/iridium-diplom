using Scripts.Blocks;

namespace Assets
{
    using global::Scripts;
    using UnityEngine;

    public class EqualsBlock : Block {

        public SubBlocksField Field;
        public Block LeftBlock;
        public Block RightBlock;
        public SpriteRenderer Separator;
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
            return (this.Field.BaseWidth + this.HeadWidthStretch) * this.transform.lossyScale.x;
        }

        public override void Stretch()
        {
            this.Field.Stretch(this.HeadWidthStretch, this.HeadHeightStretch);
            if (LeftBlock != null)
                this.Separator.transform.localPosition = new Vector3(LeftBlock.GetWidth(), 1);
            else
                this.Separator.transform.localPosition = new Vector3(1, 1);
            base.Stretch();

        }
    }
}
