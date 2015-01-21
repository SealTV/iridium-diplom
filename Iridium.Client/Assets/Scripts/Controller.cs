namespace Scripts
{
    using Blocks;
    using UnityEngine;

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
            this.MouseEvent();
            if (this.isPressed && this.pressedBlock != null)
            {
                this.pressedBlock.transform.position = (Input.mousePosition - this.downPosition) / this.screenRatio;
            }
        }

        private bool TryGetBlock(RaycastHit2D[] hits)
        {
            Block block = null;
            int hightestLayer = 0;
            foreach (var hit in hits)
            {
                Transform blockTransform = hit.transform;
                if (!blockTransform.name.Contains("Connector"))
                {
                    Block temp = blockTransform.GetComponent<SubBlock>().Parent.Parent;
                    if (temp.CurrentLayerSorting > hightestLayer)
                    {
                        hightestLayer = temp.CurrentLayerSorting;
                        block = temp;
                    }
                }
            }

            if (block != null)
            {
                this.isPressed = true;
                this.pressedBlock = block;
                this.downPosition = (Input.mousePosition - this.pressedBlock.transform.position * this.screenRatio);
                if (this.pressedBlock.Parent != null)
                    this.pressedBlock.Parent.Connectors.Remove(this.pressedBlock.ParentConnector);
                this.pressedBlock.ReSortingLayers(20001);
                this.pressedBlock.transform.parent = null;
                this.pressedBlock.Parent = null;
                return true;
            }
            return false;
        }

        bool TryPasteBlock(RaycastHit2D[] hits)
        {
            foreach (var hit in hits)
            {
                Transform hitTransform = hit.transform;
                if (hitTransform.name.Contains("Connector"))
                {
                    Connector connector = hitTransform.GetComponent<Connector>();
                    Block parent = connector.Parent;
                    if (!parent.Connectors.ContainsKey(hitTransform.name) && connector.ConnectorType == this.pressedBlock.InputConnector.ConnectorType)
                    {
                        parent.Connectors[hitTransform.name] = this.pressedBlock;
                        this.pressedBlock.Parent = parent;
                        this.pressedBlock.ParentConnector = hitTransform.name;
                        this.pressedBlock.transform.parent = hitTransform.transform;
                        this.pressedBlock.transform.position = hitTransform.position;
                        this.pressedBlock.ReSortingLayers(parent.CurrentLayerSorting + 2);
                        return true;
                    }
                }
            }
            return false;
        }

        void MouseEvent()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(worldTouch.x, worldTouch.y), Vector2.zero, Mathf.Infinity);
                this.TryGetBlock(hits);

            }
            if (Input.GetMouseButtonUp(0) && this.isPressed)
            {
                this.isPressed = false;
                Vector2 raycastVector = new Vector2(this.pressedBlock.InputConnector.transform.position.x,
                                                    this.pressedBlock.InputConnector.transform.position.y);
                RaycastHit2D[] hits = Physics2D.RaycastAll(raycastVector, Vector2.zero, Mathf.Infinity);
                if (!this.TryPasteBlock(hits))
                {
                    this.pressedBlock.ReSortingLayers();
                    this.pressedBlock = null;
                }
            } 
        }

    }
}
