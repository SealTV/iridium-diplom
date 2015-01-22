namespace Scripts.Blocks
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Block: MonoBehaviour
    {
        public Dictionary<string, Block> Connectors = new Dictionary<string, Block>(); 
        public abstract float GetHeight();
        public abstract float GetWidth();
        public abstract void Streach();
        public abstract void ReSortingLayers(int layer);

        public void ReSortingLayers()
        {
            this.ReSortingLayers(this.LayerSorting);
        }

        public List<SpriteRenderer> Texts;

        public Block Parent;
        public string ParentConnector;
        public Connector InputConnector;

        public float HeadHeightStretch = 1;
        public float HeadWidthStretch = 1;

        public int LayerSorting;
        public int CurrentLayerSorting;
    }
}
