using System.Text;
using Assets.Scripts;
using Assets.Scripts.Block_Types;
using Scripts;
using UnityEngine;

namespace Assets
{
    [ExecuteInEditMode]
    public class IfBlock : Block
    {

        public SubBlocksField HighField;
        public SubBlocksField MiddleField;
        public SubBlocksField LowField;

        public override void ReSortingLayers(int layer)
        {
            this.CurrentLayerSorting = layer;
            foreach (var connector in this.Connectors)
            {
                connector.Value.ReSortingLayers(layer + 1);
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
                text.sortingOrder = layer + 1;
            }
        }

        public void Update()
        {
            
        }

        public override void ChooseType(ConnectorType connectorType)
        {

        }

        public override void UnChooseType()
        {
        }

        public override string GetCode()
        {
            var result = new StringBuilder();
            return result.ToString();
        }

        private void Start()
        {
            this.LayerSorting = Random.Range(0, 100)*100;
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
            return this.HeadWidthStretch;
        }

        public override void Stretch()
        {

            float BodyHeightStretch = 0;
            float headWidthStretch = this.HeadWidthStretch;
            Block block;
            if (this.Connectors.TryGetValue("InnerConnector", out block)) BodyHeightStretch += block.GetHeight();
            if (this.Connectors.TryGetValue("ConditionConnector", out block))
            {
                this.HeadHeightStretch = block.GetHeight();
                headWidthStretch += block.GetWidth() - 2;
            }
            else
            {
                this.HeadHeightStretch = 0;
            }
            if (this.Parent != null)
            {
                this.transform.position = this.Parent.Connectors[this.ParentConnector].transform.position;
            }

            this.HighField.Stretch(headWidthStretch, this.HeadHeightStretch - 1.5f);

            this.MiddleField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight());
            this.MiddleField.Stretch(1, BodyHeightStretch - 2);

            this.LowField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight() - this.MiddleField.GetHeight());
            this.LowField.Stretch(headWidthStretch - 2, 0);

            base.Stretch();
        }

    }
}
