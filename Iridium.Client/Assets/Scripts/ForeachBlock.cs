namespace Assets
{
    using System.Collections.Generic;
    using Scripts;
    using UnityEngine;
    using SimpleJSON;

    public class ForeachBlock : MonoBehaviour,
                                IBlock
    {

        public SubBlocksField UpField;
        public SubBlocksField MiddleField;
        public SubBlocksField DownField;

        public float HeightStretch = 1;
        public float WidthStretch = 1;

        public float MiddleHeightStretch = 1;

        public void OnChangeBlock(IBlock[] subBlocks)
        {
        }

        public float GetHeight()
        {
            float value = 0;
            value += this.UpField.X;
            value += this.MiddleField.X;
            value += this.DownField.X;
            return value;
        }

        public void Streach()
        {
            UpField.Stretch(1,1);
            this.GetHeight();
        }

        private void Start()
        {
            JSONClass json = JSON.Parse(Resources.Load<TextAsset>("ForeachBlock").text) as JSONClass;
            //this.UpField = new SubBlocksField(json["up_field"].AsObject, transform);
            //this.MiddleField = new SubBlocksField(json["middle_field"].AsObject, transform);
            //this.DownField = new SubBlocksField(json["down_field"].AsObject, transform);
        }

        // Update is called once per frame
        private void Update()
        {
            //MiddleField.FieldTransform.position = -new Vector3(0, 3 + HeightStretch);

            //DownField.FieldTransform.position = -new Vector3(0, 3 + HeightStretch + MiddleHeightStretch);
        }

        private void OnMouseDown()
        {
            Debug.Log("MouseDown");
        }
    }
}