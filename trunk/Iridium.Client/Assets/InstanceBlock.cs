using Scripts.Blocks;

namespace Assets
{
    using System;
    using System.Text;
    using global::Scripts;
    using Scripts;
    using Scripts.Block_Modules;
    using Scripts.Block_Types;
    using UnityEngine;
    using UnityEngine.UI;

    public class InstanceBlock : Block {

        public SubBlocksField Field;
        public InputField VariableName;
        public InputField VariableValue;
        public BoolButton BoolButton;
        public ChooseButton ChooseButton;
        public GetVariable GetVariable;

        // Use this for initialization
        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
	
        }

        public ConnectorType GetConnectorType()
        {
            string a = this.ChooseButton.GetSpriteName();
            string d = a[0].ToString().ToUpper();
            Debug.Log(d + a.Substring(1));
            return (ConnectorType)Enum.Parse(typeof(ConnectorType), d + a.Substring(1));
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
            GetVariable.gameObject.SetActive(
                !string.IsNullOrEmpty(VariableName.text) &&
                (
                    (VariableValue.enabled && !string.IsNullOrEmpty(VariableValue.text))||
                    (BoolButton.gameObject.activeSelf)||
                    (!BoolButton.gameObject.activeSelf && !VariableValue.gameObject.activeSelf)
                )
            );

            if (this.Parent != null)
            {
                this.transform.position = this.Parent.Connectors[this.ParentConnector].transform.position;
            }

            this.Field.Stretch(this.HeadWidthStretch, this.HeadHeightStretch);
            base.Stretch();
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
            Block block;
            string value;
            if (this.VariableValue.IsActive())
            {
                if (this.ChooseButton.SpriteRenderer.sprite.name == "String") value = String.Format("\"{0}\"", this.VariableValue.text);
                else value = this.VariableValue.text;
            }
            else if (this.BoolButton.gameObject.activeSelf)
                value = BoolButton.SpriteRenderer.sprite.name.ToLower();
            else value = "null";
            return String.Format("{0} {1} = {2}; \n", this.ChooseButton.SpriteRenderer.sprite.name, this.VariableName.text, value)+
                    (this.Connectors.TryGetValue("OutputConnector", out block)? block.GetCode(): string.Empty);
        }
    }
}
