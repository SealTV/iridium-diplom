namespace Scripts
{
    using Assets.Scripts;
    using Assets.Scripts.Block_Types;
    using Blocks;
    using UnityEngine;

    public class BoolBlock : Block {

        public SubBlocksField Field;
        public SpriteRenderer image;
        public SpriteRenderer Renderer;
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
            //foreach (var block in this.Field.Blocks)
            //{
            //    block.sortingOrder = layer;
            //}
            foreach (var text in this.Texts)
            {
                text.sortingOrder = layer + 1;
            }
            this.image.sortingOrder = layer + 1;
            this.Renderer.sortingOrder = layer;
        }

        public override void ChooseType(ConnectorType connectorType)
        {
            
        }

        public override void UnChooseType()
        {
        }

        public override string GetCode()
        {
            return value.ToString();
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
            return 5;
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