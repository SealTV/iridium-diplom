namespace Assets.Scripts
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
            IsTrue = !IsTrue;
            SpriteRenderer.sprite = IsTrue ? TrueSprite : FalseSprite;
        }

        public override void UnUse()
        {
            Debug.Log("UnUse");
        }

        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
	
        }
    }
}
