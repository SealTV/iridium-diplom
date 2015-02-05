namespace Scripts.Blocks
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts;
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
            if (Parent != null)
            {
                Parent.Stretch();
            }
        }
    }
}
