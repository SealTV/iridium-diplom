namespace Assets.Scripts.Block_Types
{
    using UnityEngine;
    using UnityEngine.UI;

    public class VariableBlock : Block
    {
        public float Height;
        public float Width;
        public SpriteRenderer Sprite;
        public Text Text;
        public Canvas Canvas;

        private string _name;
        public string Name
        {
            get { return this._name; }
            set
            {
                this._name = value;
                this.Text.text = this._name;
            }
        }

        public override float GetHeight()
        {
            return this.Height;
        }

        public override float GetWidth()
        {
            return this.Width;
        }

        public override void ReSortingLayers(int layer)
        {
            this.CurrentLayerSorting = layer;
            this.Sprite.sortingOrder = layer;
            this.Canvas.sortingOrder = layer + 1;
        }

        public override void ChooseType(ConnectorType connectorType)
        {
            
        }

        public override void UnChooseType()
        {
            
        }

        public override string GetCode()
        {
            return " "+this.Text.text;
        }
    }
}
