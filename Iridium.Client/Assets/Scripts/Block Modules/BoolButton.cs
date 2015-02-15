namespace Assets.Scripts.Block_Modules
{
    using UnityEngine;

    public class BoolButton : Active
    {
        public SpriteRenderer SpriteRenderer;
        public Sprite TrueSprite;
        public Sprite FalseSprite;
        public bool IsTrue;

        // Use this for initialization
        public override void Use()
        {
            Debug.Log("Use");
            this.IsTrue = !this.IsTrue;
            this.SpriteRenderer.sprite = this.IsTrue ? this.TrueSprite : this.FalseSprite;
        }

        public override void UnUse()
        {
            Debug.Log("UnUse");
        }
    }
}
