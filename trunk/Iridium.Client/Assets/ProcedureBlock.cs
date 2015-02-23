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
            Block block;
            return 2+(Connectors.TryGetValue("OutputConnector", out block)?block.GetHeight():0);
        }

        public override float GetWidth()
        {
            return 0;
        }

        public override void ReSortingLayers(int layer)
        {
            Debug.Log(layer);
            CurrentLayerSorting = layer;
            foreach (var connector in this.Connectors)
            {
                connector.Value.ReSortingLayers(layer + 1);
            }
            foreach (var block in this.Field.Blocks)
            {
                block.sortingOrder = layer;
            }
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
                Block block;
                return string.Format(this.Code, this.Connectors.TryGetValue("ParameterConnector", out block) ? block.GetCode() : string.Empty);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return "";
            }
        }
    }
}
