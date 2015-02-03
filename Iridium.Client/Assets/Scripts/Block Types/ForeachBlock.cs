﻿namespace Scripts.Blocks
{
    using UnityEngine;

    [ExecuteInEditMode]
    public class ForeachBlock : Block
    {
        public SubBlocksField HighField;
        public SubBlocksField MiddleField;
        public SubBlocksField LowField;

        public override void ReSortingLayers(int layer)
        {
            this.CurrentLayerSorting = layer;
            foreach (var connector in this.Connectors)
            {
                connector.Value.ReSortingLayers(layer+1);
            }
            foreach (var block in this.HighField.Blocks)
            {
                block.sortingOrder = layer;
            }
            foreach (var block in this.MiddleField.Blocks)
            {
                block.sortingOrder = layer;
            }
            foreach (var block in this.LowField.Blocks)
            {
                block.sortingOrder = layer;
            }
            foreach (var text in this.Texts)
            {
                text.sortingOrder = layer+1;
            }
        }
        void Start()
        {
            this.LayerSorting = Random.Range(0, 100) * 100;
            this.ReSortingLayers(this.LayerSorting);
        }

        public override float GetHeight()
        {
            float value = 0;
            value += this.HighField.GetHeight();
            value += this.LowField.GetHeight();
            Block block;
            if (this.Connectors.TryGetValue("InnerConnector", out block)) value += block.GetHeight();
            else value += this.MiddleField.GetHeight();

            if (this.Connectors.TryGetValue("OutputConnector", out block)) value += block.GetHeight();

            return value;
        }

        public override float GetWidth()
        {
            return HeadWidthStretch;
        }

        public override void Streach()
        {
            
            float BodyHeightStretch = 0;

            HeadWidthStretch = 2;
            Block block;
            if (this.Connectors.TryGetValue("InnerConnector", out block)) BodyHeightStretch += block.GetHeight();
            if (this.Connectors.TryGetValue("ConditionConnector", out block))
            {
                HeadHeightStretch = block.GetHeight();
                HeadWidthStretch +=block.GetWidth();
            }
            else
            {
                HeadHeightStretch = 0;
            }
            if (this.Parent != null)
            {
                this.transform.position = this.Parent.Connectors[this.ParentConnector].transform.position;
            }

            this.HighField.Stretch(HeadWidthStretch, HeadHeightStretch-1.5f);

            this.MiddleField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight());
            this.MiddleField.Stretch(1, BodyHeightStretch-2);

            this.LowField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight() - this.MiddleField.GetHeight());
            this.LowField.Stretch(HeadWidthStretch-2, 0);
        }

        private void Update()
        { 
            this.Streach();
        }
    }
}