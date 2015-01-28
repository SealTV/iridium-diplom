using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    using UnityEngine;

    public class PanelButton : Selectable
    {
        public Sprite OpenPanelSprite;
        public Sprite ClosedPanelSprite;
        public Animator PanelAnimator;
        public bool IsClose;

        private Image targetImage;

        void Start()
        {
            base.Start();
            targetImage = this.GetComponent<Image>();
        }

        public override void OnPointerUp(PointerEventData pointerEventData)
        {
            base.OnPointerUp(pointerEventData);
            this.IsClose = !this.IsClose;
            targetImage.sprite = this.IsClose ? ClosedPanelSprite : OpenPanelSprite;
            PanelAnimator.SetBool("IsClose",IsClose);
        }
    }
}
