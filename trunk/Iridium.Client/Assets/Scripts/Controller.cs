namespace Scripts
{
    using System;
    using System.Linq;
    using Assets.Scripts;
    using Blocks;
    using UnityEngine;

    public class Controller : MonoBehaviour
    {
        private bool isBlockTaken;
        private Block pressedBlock;

        private Vector3 downPosition;
        private float screenRatio;

        public Transform Scaler;
        private float scaleSpeed = 0.3f;
        private float scaleMin = 0.5f;
        private float scaleMax = 1f;
        private Vector3 mousePressedPosition;
        private float minMouseShift;

        private void Start()
        {
            this.screenRatio = 0.5f*Screen.height/Camera.main.orthographicSize;
            this.minMouseShift = Screen.height/40f;
        }

        private void Update()
        {
            this.MouseEvent();
            if (this.isBlockTaken && this.pressedBlock != null)
            {
                this.pressedBlock.transform.position = (Input.mousePosition - this.downPosition)/this.screenRatio;
            }
        }

        private void MouseEvent()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                this.mousePressedPosition = Input.mousePosition;
                RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(worldTouch.x, worldTouch.y), Vector2.zero,
                    Mathf.Infinity);
                this.TryFindBlock(hits);
            }

            if (Input.GetMouseButton(0) && !this.isBlockTaken && pressedBlock != null)
            {
                if ((Input.mousePosition - this.mousePressedPosition).magnitude > this.minMouseShift)
                {
                    this.GetBlock();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                
                if (this.isBlockTaken)
                {
                    var raycastVector = new Vector2(this.pressedBlock.InputConnector.transform.position.x,
                        this.pressedBlock.InputConnector.transform.position.y);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(raycastVector, Vector2.zero, Mathf.Infinity);

                    if (!this.TryPasteBlock(hits))
                    {
                        this.pressedBlock.ReSortingLayers();
                    }
                    this.isBlockTaken = false;
                }
                else
                {
                    var worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(worldTouch.x, worldTouch.y), Vector2.zero,
                        Mathf.Infinity);
                    Debug.Log(hits.Count());
                    this.TryUsingGUI(hits);
                }
            }
            float i = Scaler.localScale.x;
            i += scaleSpeed * Input.GetAxis("Mouse ScrollWheel");
            i = Math.Min(Math.Max(scaleMin, i), scaleMax);
            Scaler.localScale=new Vector3(i,i,i);
        }

        private bool TryUsingGUI(RaycastHit2D[] hits)
        {
            int hightestLayer = 0;
            Active tempActive= null;
            foreach (var hit in hits)
            {
                Transform blockTransform = hit.transform;
                if (blockTransform.tag == "GUI")
                {
                    Debug.Log("GUI");
                    Active temp = blockTransform.GetComponent<Active>();
                    if (temp.Parent.CurrentLayerSorting > hightestLayer)
                    {
                        hightestLayer = temp.Parent.CurrentLayerSorting;
                        tempActive = temp;
                    }
                }
            }
            if(tempActive!=null) tempActive.Use();
            return true;
        }

        private bool TryPasteBlock(RaycastHit2D[] hits)
        {
            foreach (var hit in hits)
            {
                Transform hitTransform = hit.transform;
                if (hitTransform.name.Contains("Connector"))
                {
                    var connector = hitTransform.GetComponent<Connector>();
                    Block parent = connector.Parent;
                    if (!parent.Connectors.ContainsKey(hitTransform.name) &&
                        connector.ConnectorType == this.pressedBlock.InputConnector.ConnectorType)
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
            this.pressedBlock.transform.parent = this.Scaler;
            return false;
        }

        private void GetBlock()
        {
            if (this.pressedBlock != null)
            {
                this.isBlockTaken = true;
                if (this.pressedBlock.Parent != null)
                    this.pressedBlock.Parent.Connectors.Remove(this.pressedBlock.ParentConnector);
                this.pressedBlock.ReSortingLayers(20001);
                this.pressedBlock.transform.parent = null;
                this.pressedBlock.Parent = null;
            }
        }

        private void TryFindBlock(RaycastHit2D[] hits)
        {
            this.pressedBlock = null;
            int hightestLayer = 0;
            foreach (var hit in hits)
            {
                Transform blockTransform = hit.transform;
                if (blockTransform.tag == "Block")
                {
                    Block temp = blockTransform.GetComponent<SubBlock>().Parent.Parent;
                    if (temp.CurrentLayerSorting > hightestLayer)
                    {
                        hightestLayer = temp.CurrentLayerSorting;
                        this.pressedBlock = temp;
                        this.downPosition = (Input.mousePosition - this.pressedBlock.transform.position * this.screenRatio);
                    }
                }
            }
        }
    }
}
