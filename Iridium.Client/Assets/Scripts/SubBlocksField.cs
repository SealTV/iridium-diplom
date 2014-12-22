using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts
{
    public class SubBlocksField : MonoBehaviour
    {
        public float HeightStretch = 1;
        public float WidthStretch = 1;

        public GameObject Field;
        public Transform FieldTransform;
        public List<Transform> Blocks;

        public List<Transform> HeightStretchBlocks;
        public List<Transform> WidthStretchBlocks;
        public List<Transform> HeightShiftBlocks;
        public List<Transform> WidthShiftBlocks;

        public float X;
        public float Y;

        public float BaseWidth;
        public float BaseHeight;

        public SubBlocksField(JSONClass jsonClass, Transform transform)
        {
            this.HeightStretch = 1;
            this.WidthStretch = 1;

            this.X = jsonClass["pos_x"].AsFloat;
            this.Y = jsonClass["pos_y"].AsFloat;

            this.BaseHeight = jsonClass["height"].AsFloat;
            this.BaseWidth = jsonClass["width"].AsFloat;

            this.Field = new GameObject(jsonClass["name"].Value);
            this.FieldTransform = this.Field.transform;
            this.FieldTransform.position = new Vector3(this.X, this.Y);
            this.FieldTransform.parent = transform;

            JSONArray jsonArray = jsonClass["blocks"].AsArray;

            foreach (var json in jsonArray)
            {
                SubBlock block = new SubBlock((JSONClass)json, this.FieldTransform);
            }

        }

        public void Stretch(float height, float width)
        {
            this.HeightStretch = Mathf.Max(0, height);
            this.WidthStretch = Mathf.Max(0, width);

            foreach (var block in HeightStretchBlocks) { block.localScale = new Vector3(block.localScale.x, HeightStretch); }
            foreach (var block in WidthStretchBlocks) { block.localScale = new Vector3(WidthStretch, block.localScale.y); }
            foreach (var block in HeightShiftBlocks) { block.localPosition = new Vector3(block.localPosition.x, -BaseHeight - HeightStretch); }
            foreach (var block in WidthShiftBlocks) { block.localPosition = new Vector3(BaseWidth + WidthStretch, block.localPosition.y); }
        }

        public void Update()
        {

        }

        public float GetHeight()
        {
            return BaseHeight + this.HeightStretch;
        }
    }
}
