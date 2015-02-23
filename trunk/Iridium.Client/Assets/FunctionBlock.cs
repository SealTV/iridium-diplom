using System;
using Assets.Scripts.Block_Types;
using Scripts;
using UnityEngine;

namespace Assets
{
    using Scripts;

    public class FunctionBlock : Block {

        public string Code;
        public Connector ParameterConnector;
        public SpriteRenderer Renderer;
        public SubBlocksField Field;
        private float height;

        public override float GetHeight()
        {
            return 2;
        }

        public override float GetWidth()
        {
            return 10;
        }

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
            Block block2;
            if(this.Connectors.TryGetValue("ParameterConnector", out block2)) block2.ReSortingLayers(layer + 2);
            this.Renderer.sortingOrder = layer + 1;
        }

        public override void ChooseType(ConnectorType connectorType)
        {
            
        }

        public override void UnChooseType()
        {
            
        }

        public override string GetCode()
        {
            try
            {
                Block block;
                return string.Format(this.Code, this.Connectors.TryGetValue("ParameterConnector", out block) ? block.GetCode() : string.Empty);
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
