using System;
using Assets.Scripts.Block_Types;
using Scripts;
using UnityEngine;

namespace Assets
{
    using Scripts;

    public class FunctionBlock : Block {

        public string Code;
        public Block Parameter;
        public Connector ParameterConnector;
        public SpriteRenderer Renderer;
        public SubBlocksField Field;


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
            foreach (var connector in this.Connectors)
            {
                connector.Value.ReSortingLayers(layer + 1);
            }
            foreach (var block in this.Field.Blocks)
            {
                block.sortingOrder = layer;
            }

            if(Parameter!=null)
                Parameter.ReSortingLayers(layer + 2);
            Renderer.sortingOrder = layer + 1;
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
                return string.Format(this.Code, this.Parameter);
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
