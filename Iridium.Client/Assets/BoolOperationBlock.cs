using UnityEngine;
using System.Collections;


namespace Assets
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
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
        Dictionary<string, string> dic = new Dictionary<string, string>
                                         {
                                                             {"Equals","=="},
                                                             {"Less","<"},
                                                             {"More",">"},
                                                             {"UnEquals","!="}
                                         }; 
        private float width;
        private float height;

        public override void ReSortingLayers(int layer)
        {
            this.CurrentLayerSorting = layer;
            this.Separator.sortingOrder = layer + 1;
            foreach (var block in this.Field.Blocks)
            {
                block.sortingOrder = layer;
            }
            foreach (var text in this.Texts)
            {
                text.sortingOrder = layer + 1;
            }
            if (this.LeftBlock != null) this.LeftBlock.ReSortingLayers(layer + 2);
            if (this.RightBlock != null) this.RightBlock.ReSortingLayers(layer + 2);
        }

        public override void ChooseType(ConnectorType connectorType)
        {
            this.LeftConnector.ConnectorType = connectorType;
            this.RightConnector.ConnectorType = connectorType;
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
            return String.Format("({0} {1} {2})", 
                                this.LeftBlock.GetCode(), 
                                this.dic[this.Separator.sprite.name],
                                this.RightBlock.GetCode());
        }

        private void Start()
        {
            this.LayerSorting = Random.Range(0, 100)*100;
            this.ReSortingLayers(this.LayerSorting);
            this.width = this.HeadWidthStretch;
            this.height = this.HeadHeightStretch;


        }

        public override float GetHeight()
        {
            float value = Math.Max(this.LeftBlock ? this.LeftBlock.GetHeight() : 2f, this.RightBlock ? this.RightBlock.GetHeight() : 2f);
            return this.height*1.5f;
        }

        public override float GetWidth()
        {
            //return (this.Field.BaseWidth + this.HeadWidthStretch)*this.transform.lossyScale.x;
            return this.width/1.5f;
        }

        public override void Stretch()
        {
            this.Connectors.TryGetValue("LeftConnector", out this.LeftBlock);
            this.Connectors.TryGetValue("RightConnector", out this.RightBlock);
            this.width = Math.Max(this.HeadWidthStretch,
                ((this.RightBlock == null ? 0 : this.RightBlock.GetWidth())
                +
                (this.LeftBlock == null ? 0 : this.LeftBlock.GetWidth())) * 2+5);

            this.height = Math.Max(this.HeadHeightStretch,
                Math.Max(
                    this.RightBlock == null ? 0 : this.RightBlock.GetHeight(),
                    this.LeftBlock == null ? 0 : this.LeftBlock.GetHeight())/2+1);

            this.Field.Stretch(this.width, this.height);
            this.Separator.transform.localPosition = new Vector3(this.LeftBlock != null ? this.LeftBlock.GetWidth()*2 + 3 : 8f, -0.2f);
            base.Stretch();
        }

        public void Update()
        {
        }
    }
}
