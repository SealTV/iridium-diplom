using UnityEngine;

namespace Assets
{
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class Test : Selectable
    {

        private bool isPressed;
        public Selectable select;
        public Image image;
        private Vector3 downPosition;
        private float screenRatio;
        // Use this for initialization
        void Start()
        {
            this.screenRatio = 0.5f * Screen.height / Camera.main.orthographicSize;
        }

        public override void OnPointerDown(PointerEventData pointerEventData)
        {
            base.OnPointerDown(pointerEventData);
            Debug.Log("asdasda");
        }
        // Update is called once per frame
        void Update()
        {
            //this.MouseEvent();
        }

        void MouseEvent()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(worldTouch.x, worldTouch.y), Vector2.zero, Mathf.Infinity);
                foreach (var raycastHit2D in hits)
                {
                    Debug.Log(raycastHit2D);
                }

            }
        }

        public void Print(string te)
        {
            Debug.Log("press");
        }

    }
}
