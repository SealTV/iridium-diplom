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

        public float HeadHeightStretch = 1;
        public float HeadWidthStretch = 1;

        public float BodyHeightStretch = 1;
        public float BodyWidthStretch = 1;

        public float MiddleHeightStretch = 1;

        public void OnChangeBlock(IBlock[] subBlocks)
        {
            this.Streach();
        }

        public float GetHeight()
        {
            float value = 0;
            value += this.UpField.X;
            value += this.MiddleField.X;
            value += this.DownField.X;
            return value;
        }

        public float GetWidth()
        {
            return 0;
        }

        public void Streach()
        {
            UpField.Stretch(HeadHeightStretch,HeadWidthStretch);
            MiddleField.transform.position = new Vector3(0, -UpField.GetHeight());
            MiddleField.Stretch(BodyHeightStretch, BodyWidthStretch);
        }

        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            this.Streach();
        }

        private void OnMouseDown()
        {
            Debug.Log("MouseDown");
        }
    }
}