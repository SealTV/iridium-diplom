namespace Assets.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Block: MonoBehaviour
    {
        public Dictionary<string, Block> Connectors = new Dictionary<string, Block>(); 
        public abstract float GetHeight();
        public abstract float GetWidth();
        public abstract void Streach();

        public Block Parent;
        public string ParentConnector;
        public Transform InputConnector;

        public float HeadHeightStretch = 1;
        public float HeadWidthStretch = 1;
    }
}
