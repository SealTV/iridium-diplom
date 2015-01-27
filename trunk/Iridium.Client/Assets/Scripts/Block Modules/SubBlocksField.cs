namespace Scripts
{
    using System.Collections.Generic;
    using Blocks;
    using UnityEngine;

    public class SubBlocksField : MonoBehaviour
    {
        public Block Parent;
        public float HeightStretch = 1;
        public float WidthStretch = 1;

        public List<SpriteRenderer> Blocks;

        public List<Transform> HeightStretchBlocks;
        public List<Transform> WidthStretchBlocks;
        public List<Transform> HeightShiftBlocks;
        public List<Transform> WidthShiftBlocks;

        public float BaseWidth;
        public float LeftWidth;
        public float BaseHeight;
        public float HighHeight;

        public void Stretch(float width, float height)
        {
            this.HeightStretch = Mathf.Max(0, height);
            this.WidthStretch  = Mathf.Max(0, width);
            
            this.HeightStretchBlocks.ForEach(x => x.localScale = new Vector3(x.localScale.x, this.HeightStretch));
            this.WidthStretchBlocks .ForEach(x => x.localScale = new Vector3(this.WidthStretch, x.localScale.y));
            this.HeightShiftBlocks  .ForEach(x => x.localPosition = new Vector3(x.localPosition.x, -this.HighHeight - this.HeightStretch));
            this.WidthShiftBlocks   .ForEach(x => x.localPosition = new Vector3(this.LeftWidth + this.WidthStretch, x.localPosition.y));
        }

        public void Update()
        {

        }

        public float GetHeight()
        {
            return this.BaseHeight + this.HeightStretch;
        }

        public float GetWidth()
        {
            return this.BaseWidth + this.WidthStretch;
        }
    }
}
