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

    public class SubBlocksField
    {
        public float HeightStretch;
        public float WidthStretch;

        public GameObject Field;
        public Transform FieldTransform;
        public List<SubBlock> Blocks;

        //public List<SubBlock> HeightStretchBlocks;
        //public List<SubBlock> WidthStretchBlocks;
        //public List<SubBlock> HeightShiftBlocks;
        //public List<SubBlock> WidthShiftBlocks;

        public float X;
        public float Y;

        public float BaseWidth;
        public float BaseHeight;

        public SubBlocksField(JSONClass jsonClass, Transform transform)
        {
            this.Blocks = new List<SubBlock>();
            //this.HeightStretchBlocks = new List<SubBlock>();
            //this.WidthStretchBlocks = new List<SubBlock>();
            //this.HeightShiftBlocks = new List<SubBlock>();
            //this.WidthShiftBlocks = new List<SubBlock>();

            this.HeightStretch = 1;
            this.WidthStretch = 1;

            this.X = jsonClass["pos_x"].AsFloat;
            this.Y = jsonClass["pos_y"].AsFloat;

            this.BaseHeight = jsonClass["height"].AsFloat;
            this.BaseWidth  = jsonClass["width"].AsFloat;

            this.Field = new GameObject(jsonClass["name"].Value);
            this.FieldTransform = this.Field.transform;
            this.FieldTransform.position = new Vector3(this.X, this.Y);
            this.FieldTransform.parent = transform;

            JSONArray jsonArray = jsonClass["blocks"].AsArray;

            foreach (var json in jsonArray)
            {
                SubBlock block = new SubBlock((JSONClass)json, this.FieldTransform);
                this.Blocks.Add(block);

                //if (block.isHeightStretch) this.HeightStretchBlocks.Add(block);
                //if (block.isWidthStretch) this.WidthStretchBlocks.Add(block);
                //if (block.isHeightShift) this.HeightShiftBlocks.Add(block);
                //if (block.isWidthShift) this.WidthShiftBlocks.Add(block);
            }


        }

        public void Stretch(float widthStretch, float heightStretch)
        {
            foreach (var block in Blocks)
            {
                Vector3 position = block.gameObject.transform.localPosition;
                Vector3 scale = block.gameObject.transform.localScale;
                if (block.isHeightShift) position.y = -this.BaseHeight - heightStretch;
                if (block.isWidthShift) position.x = this.BaseWidth + widthStretch;
                if (block.isHeightStretch) scale.y = heightStretch;
                if (block.isWidthStretch) scale.x = widthStretch;

                block.gameObject.transform.localPosition = position;
                block.gameObject.transform.localScale = scale;
            }
        }
    }
}