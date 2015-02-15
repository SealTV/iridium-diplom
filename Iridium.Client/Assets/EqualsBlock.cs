using Scripts.Blocks;

namespace Assets
{
    using System;
    using global::Scripts;
    using Scripts;
    using Scripts.Block_Types;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [ExecuteInEditMode]
    public class EqualsBlock : Block {

        public SubBlocksField Field;
        public Block LeftBlock;
        public Block RightBlock;
        public SpriteRenderer Separator;
        public Connector LeftConnector, RightConnector;
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

        void Start()
        {
            this.LayerSorting = Random.Range(0, 100) * 100;
            this.ReSortingLayers(this.LayerSorting);

        }

        public override float GetHeight()
        {
            Block block;
            float value = 2f;
            if (this.Connectors.TryGetValue("OutputConnector", out block))
            {
                value += block.GetHeight();
            }
            return value;
        }

        public override float GetWidth()
        {
            return (this.Field.BaseWidth + this.HeadWidthStretch) * this.transform.lossyScale.x;
        }

        public override void Stretch()
        {
            this.Connectors.TryGetValue("LeftConnector", out this.LeftBlock);
            this.Connectors.TryGetValue("RightConnector", out this.RightBlock);
            float headWidthStreatch = Math.Max(this.HeadWidthStretch,
                (this.RightBlock == null ? 0 : this.RightBlock.GetWidth())
                +
                (this.LeftBlock == null ? 0 : this.LeftBlock.GetWidth()));
            this.Field.Stretch(headWidthStreatch, this.HeadHeightStretch);
            this.Separator.transform.localPosition = new Vector3(this.LeftBlock != null ? this.LeftBlock.GetWidth()+1:3.5f, -0.2f);
            base.Stretch();

        }

        public void Update()
        {
        }
    }
}
