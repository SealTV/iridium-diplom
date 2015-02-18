using System.Collections.Generic;
using Assets.Scripts.Games;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ChooseBoolOperation : Active
    {

        public SpriteRenderer SpriteRenderer;
        private List<Sprite> Sprites;
        private int currentSprite;

        public void Start()
        {
            if (this.Sprites == null || this.Sprites.Count == 0)
            {
                this.Sprites = BaseGameController.Instance.BoolOperationTypes;
            }
            this.Use();
        }

        public string GetSpriteName()
        {
            return this.Sprites[this.currentSprite].name;
        }

        // Use this for initialization
        public override void Use()
        {
            if (++this.currentSprite == this.Sprites.Count) this.currentSprite = 0;
            this.SpriteRenderer.sprite = this.Sprites[this.currentSprite];
        }

        public override void UnUse()
        {
            Debug.Log("UnUse");
        }
    }
}

