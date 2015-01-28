namespace Scripts
{
    using Blocks;
    using UnityEngine;

    public class BoolBlock : Block {

        public SubBlocksField Field;
        public SpriteRenderer image;
        private bool value;

        public Sprite TrueSprite;
        public Sprite FalseSprite;

        public void SwitchImage()
        {
            this.value = !this.value;
            this.image.sprite = this.value ? this.TrueSprite : this.FalseSprite;
        }

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
            this.image.sortingOrder = layer + 1;
        }

        void Start()
        {
            this.LayerSorting = Random.Range(0, 100) * 100+10000;
            this.ReSortingLayers(this.LayerSorting);
            
        }

        public override float GetHeight()
        {
            return 1.5f;
        }

        public override float GetWidth()
        {
            return (this.Field.BaseWidth + this.HeadWidthStretch) * this.transform.lossyScale.x;
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