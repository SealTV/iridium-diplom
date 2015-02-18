using UnityEngine;
using System.Collections;


namespace Assets
{
    using System;
    using global::Scripts;
    using Scripts;
    using Scripts.Block_Types;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [ExecuteInEditMode]
    public class BoolOperationBlock : Block
    {

        public SubBlocksField Field;
        public Block LeftBlock;
        public Block RightBlock;
        public SpriteRenderer Separator;
        public Connector LeftConnector, RightConnector;

        public override void ReSortingLayers(int layer)
        {
            this.CurrentLayerSorting = layer;
            Separator.sortingOrder = layer + 1;
            foreach (var block in this.Field.Blocks)
            {
                block.sortingOrder = layer;
            }
            foreach (var text in this.Texts)
            {
                text.sortingOrder = layer + 1;
            }
            if (LeftBlock != null) LeftBlock.ReSortingLayers(layer + 2);
            if (RightBlock != null) RightBlock.ReSortingLayers(layer + 2);
        }

        public override void ChooseType(ConnectorType connectorType)
        {
            LeftConnector.ConnectorType = connectorType;
            RightConnector.ConnectorType = connectorType;
        }

        public override void UnChooseType()
        {
            if (!this.Connectors.ContainsKey("LeftConnector") &&
                !this.Connectors.ContainsKey("RightConnector"))
            {
                this.LeftConnector.ConnectorType = ConnectorType.Value;
                this.RightConnector.ConnectorType = ConnectorType.Value;
            }
        }

        public override string GetCode()
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            this.LayerSorting = Random.Range(0, 100)*100;
            this.ReSortingLayers(this.LayerSorting);

        }

        public override float GetHeight()
        {
            float value = Math.Max(LeftBlock ? LeftBlock.GetHeight() : 2f, RightBlock ? RightBlock.GetHeight() : 2f);
            return value;
        }

        public override float GetWidth()
        {
            return (this.Field.BaseWidth + this.HeadWidthStretch)*this.transform.lossyScale.x;
        }

        public override void Stretch()
        {
            this.Connectors.TryGetValue("LeftConnector", out this.LeftBlock);
            this.Connectors.TryGetValue("RightConnector", out this.RightBlock);
            this.HeadHeightStretch = Math.Max(LeftBlock ? LeftBlock.GetHeight() : 0, RightBlock ? RightBlock.GetHeight() : 0);
            float headWidthStreatch = Math.Max(this.HeadWidthStretch,
                (this.RightBlock == null ? 2 : this.RightBlock.GetWidth())
                +
                (this.LeftBlock == null ? 2 : this.LeftBlock.GetWidth()));
            this.Field.Stretch(headWidthStreatch, this.HeadHeightStretch);
            this.Separator.transform.localPosition = new Vector3(this.LeftBlock != null ? this.LeftBlock.GetWidth() + 1 : 7f, -0.2f);
            base.Stretch();

        }

        public void Update()
        {
        }
    }
}
