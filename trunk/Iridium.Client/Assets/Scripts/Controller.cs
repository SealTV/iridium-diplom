using UnityEngine;

namespace Assets.Scripts
{
    using System.Linq;

    public class Controller : MonoBehaviour
    {

        private bool isPressed;
        private Block pressedBlock;

        private Vector3 downPosition;
        private float screenRatio;
        // Use this for initialization
        void Start()
        {
            this.screenRatio = 0.5f * Screen.height / Camera.main.orthographicSize;
            
        }
	
        // Update is called once per frame
        void Update () {
            this.TestMouseEvent();
            if (this.isPressed && this.pressedBlock != null)
            {
                this.pressedBlock.transform.position = (Input.mousePosition - this.downPosition) / this.screenRatio;
            }
        }


        void TestMouseEvent()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(worldTouch.x, worldTouch.y), Vector2.zero, Mathf.Infinity);
                
                foreach (var hit in hits)
                {
                    Transform blockTransform = hit.transform;
                    if (!blockTransform.name.Contains("Connector"))
                    {
                        this.isPressed = true;
                        this.pressedBlock = blockTransform.GetComponent<SubBlock>().Parent.Parent;
                        this.downPosition = (Input.mousePosition - this.pressedBlock.transform.position * this.screenRatio);
                        if (this.pressedBlock.Parent != null)
                            this.pressedBlock.Parent.Connectors.Remove(this.pressedBlock.ParentConnector);
                        this.pressedBlock.transform.parent = null;
                        this.pressedBlock.Parent = null;
                        break;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0) && this.isPressed)
            {
                this.isPressed = false;
                Vector2 raycastVector = new Vector2(this.pressedBlock.InputConnector.transform.position.x,
                                                    this.pressedBlock.InputConnector.transform.position.y);
                RaycastHit2D[] hits = Physics2D.RaycastAll(raycastVector, Vector2.zero, Mathf.Infinity);
                foreach (var hit in hits)
                {
                    Transform connector = hit.transform;
                    if (connector.name.Contains("Connector"))
                    {
                        Block block = connector.GetComponent<Connector>().Parent;
                        Debug.Log(block);
                        if (!block.Connectors.ContainsKey(connector.name))
                        {
                            block.Connectors[connector.name] = this.pressedBlock;
                            this.pressedBlock.Parent = block;
                            this.pressedBlock.ParentConnector = connector.name;
                            this.pressedBlock.transform.parent = connector.transform;
                            this.pressedBlock.transform.position = connector.position;
                            break;
                        }
                    }
                }
                this.pressedBlock = null;
            } 
        }
    }
}
