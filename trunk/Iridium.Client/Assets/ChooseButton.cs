using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

namespace Assets
{
    using Scripts.Block_Types;
    using Scripts.Games;
    using UnityEngine.UI;

    public class ChooseButton : Active
    {
        public SpriteRenderer SpriteRenderer;
        private List<Sprite> Sprites;
        private int currentSprite;
        public InstanceBlock InstanceBlock;

        public void Start()
        {
            if (this.Sprites == null || this.Sprites.Count==0)
            {
                this.Sprites = BaseGameController.Instance.VariableTypes;
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
            Debug.Log(this.SpriteRenderer.sprite.name);
            this.InstanceBlock.VariableValue.text = "";
            if (++this.currentSprite == this.Sprites.Count) this.currentSprite = 0;
            this.SpriteRenderer.sprite = this.Sprites[this.currentSprite];
            switch (this.SpriteRenderer.sprite.name.ToLower())
            {
                case "int":
                    this.InstanceBlock.VariableValue.gameObject.SetActive(true);
                    this.InstanceBlock.BoolButton.gameObject.SetActive(false);
                    this.InstanceBlock.VariableValue.contentType = InputField.ContentType.IntegerNumber;
                    break;
                case "string":
                    this.InstanceBlock.VariableValue.gameObject.SetActive(true);
                    this.InstanceBlock.BoolButton.gameObject.SetActive(false);
                    this.InstanceBlock.VariableValue.contentType = InputField.ContentType.Alphanumeric;
                    break;
                case "float":
                    this.InstanceBlock.VariableValue.gameObject.SetActive(true);
                    this.InstanceBlock.BoolButton.gameObject.SetActive(false);
                    this.InstanceBlock.VariableValue.contentType = InputField.ContentType.DecimalNumber;
                    break;
                case "bool":
                    this.InstanceBlock.VariableValue.gameObject.SetActive(false);
                    this.InstanceBlock.BoolButton.gameObject.SetActive(true);
                    break;
                default:
                    this.InstanceBlock.VariableValue.gameObject.SetActive(false);
                    this.InstanceBlock.BoolButton.gameObject.SetActive(false);
                    break;
            }
            Debug.Log(this.SpriteRenderer.sprite.name);
            InstanceBlock.Stretch();
        }

        public override void UnUse()
        {
        }
    }
}
