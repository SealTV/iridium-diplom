namespace Assets.Scripts
{
    using System;
    using global::Scripts.Blocks;
    using UnityEngine;
    using UnityEngine.UI;

    public class VariableBlock : Block
    {
        public float Height;
        public float Width;
        public SpriteRenderer Sprite;
        public Block Value;
        public Text Text;

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
        



        // Update is called once per frame
        private void Update(){

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
            this.Sprite.sortingOrder = layer;
            if(Value!=null)
                this.Value.ReSortingLayers(layer+2);
        }

        public override string GetCode()
        {
            return String.Format(" {0} = {1};", this.Name, this.Value.GetCode());
        }
    }
}
