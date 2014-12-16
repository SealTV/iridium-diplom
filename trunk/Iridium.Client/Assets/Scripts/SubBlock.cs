namespace Assets.Scripts
{
    using System.Collections.Generic;
    using SimpleJSON;
    using UnityEngine;

    public class SubBlock
    {
        public SpriteRenderer Sprite;
        public GameObject gameObject;

        public bool isHeightStretch;
        public bool isWidthStretch;

        public bool isHeightShift;
        public bool isWidthShift;

        public SubBlock(JSONClass json, Transform block)
        {
            this.Height = json["height"].AsInt;
            this.Width = json["width"].AsInt;
            this.Position = json["position"].AsVector3();

            this.isHeightStretch = json["height_stretch"].AsBool;
            this.isWidthStretch  = json["width_stretch"] .AsBool;

            this.isHeightShift = json["height_shift"].AsBool;
            this.isWidthShift  = json["width_shift"].AsBool;
            
            this.gameObject = GameObject.Instantiate(Resources.Load(json["object"].Value), this.Position, Quaternion.identity) as GameObject;
            this.gameObject.transform.parent = block;
            this.gameObject.transform.localPosition = this.Position;
        }

        public int Height { get; private set; }
        public int Width { get; private set; }
        public Vector3 Position { get; private set; }
    }

    
}