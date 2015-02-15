﻿namespace Scripts.Blocks
{
    using Assets.Scripts;
    using Assets.Scripts.Block_Types;
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

        public override void ChooseType(ConnectorType connectorType)
        {
            
        }

        public override void UnChooseType()
        {
            
        }

        public override string GetCode()
        {
            return "";
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