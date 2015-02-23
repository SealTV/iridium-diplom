namespace Scripts.Blocks
{
    using Assets.Scripts;
    using Assets.Scripts.Block_Types;
    using UnityEngine;

    [ExecuteInEditMode]
    public class ValueBlock : Block {

        public SubBlocksField Field;
        public SpriteRenderer Render;
        public Canvas canvas;
        public override void ReSortingLayers(int layer)
        {
            this.CurrentLayerSorting = layer;

            foreach (var text in this.Texts)
            {
                text.sortingOrder = layer + 1;
            }
            this.canvas.sortingOrder = layer + 1;
            this.Render.sortingOrder = layer;
        }

        public override void ChooseType(ConnectorType connectorType)
        {
            
        }

        public override void UnChooseType()
        {
        }

        public override string GetCode()
        {
            throw new System.NotImplementedException();
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