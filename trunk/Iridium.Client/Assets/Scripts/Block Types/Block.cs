namespace Assets.Scripts.Block_Types
{
    using System.Collections.Generic;
    using global::Scripts;
    using UnityEngine;

    public abstract class Block: MonoBehaviour
    {
        public Vector3 BaseScale;
        public Dictionary<string, Block> Connectors = new Dictionary<string, Block>(); 
        public abstract float GetHeight();
        public abstract float GetWidth();
        public abstract void ReSortingLayers(int layer);

        public void ReSortingLayers()
        {
            this.ReSortingLayers(this.LayerSorting);
        }

        public abstract void ChooseType(ConnectorType connectorType);
        public abstract void UnChooseType();
        public abstract string GetCode();
        public List<SpriteRenderer> Texts;

        public Block Parent;
        public string ParentConnector;
        public Connector InputConnector;

        public float HeadHeightStretch = 1;
        public float HeadWidthStretch = 1;

        public int LayerSorting;
        public int CurrentLayerSorting;

        public virtual void Stretch()
        {
            if (this.Parent != null)
            {
                this.Parent.Stretch();
            }
        }

        
    }
}
