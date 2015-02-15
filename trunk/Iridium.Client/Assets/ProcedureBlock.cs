using Assets.Scripts.Block_Types;

namespace Assets
{
    using System;
    using global::Scripts;
    using Scripts;
    using UnityEngine;

    public class ProcedureBlock : Block
    {

        public string Code;
        public Block Parameter;
        public Connector ParameterConnector;
        public SpriteRenderer Renderer;
        public SubBlocksField Field;
        // Use this for initialization
        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
	
        }

        public override float GetHeight()
        {
            return 2;
        }

        public override float GetWidth()
        {
            return 0;
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

            Parent.ReSortingLayers(layer+2);
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
