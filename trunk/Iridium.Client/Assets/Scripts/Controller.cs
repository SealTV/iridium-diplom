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
        private Active pressedActive;
        private BlockPrototip pressedPrototip;
        private Vector3 downPosition;
        private float screenRatio;

        public Transform Scaler;
        private const float scaleSpeed = 0.3f;
        private const float scaleMin = 0.5f;
        private const float scaleMax = 1f;
        private float scale;
        private Vector3 mousePressedPosition;
        private float minMouseShift;

        private void Start()
        {
            this.screenRatio = 0.5f*Screen.height/Camera.main.orthographicSize;
            this.minMouseShift = Screen.height/40f;
            this.scale = this.Scaler.transform.localScale.x;
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

            if (Input.GetMouseButton(0) && !this.isBlockTaken)
            {
                if (this.pressedBlock != null)
                {
                    if ((Input.mousePosition - this.mousePressedPosition).magnitude > this.minMouseShift)
                    {
                        this.GetBlock();
                    }
                }
                else
                {
                    if (this.pressedPrototip != null)
                    {
                        if ((Input.mousePosition - this.mousePressedPosition).magnitude > this.minMouseShift)
                        {
                            var instantiate = (GameObject) Instantiate(this.pressedPrototip.InstantiatedPrefab);
                            this.pressedBlock = instantiate.GetComponent<Block>();
                            this.pressedBlock.transform.localScale = this.pressedBlock.BaseScale * scale;
                            this.isBlockTaken = true;
                            this.downPosition = (Input.mousePosition - this.pressedPrototip.transform.position * this.screenRatio);
                            this.pressedPrototip = null;
                        }
                    }
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
                else if(this.pressedActive!=null)
                {
                    var worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(worldTouch.x, worldTouch.y), Vector2.zero,
                        Mathf.Infinity);
                    this.TryUsingActive(hits);
                }
            }
            this.scale += scaleSpeed * Input.GetAxis("Mouse ScrollWheel");
            this.scale = Math.Min(Math.Max(scaleMin, this.scale), scaleMax);
            this.Scaler.localScale = new Vector3(this.scale, this.scale, this.scale);
        }

        private bool TryUsingActive(RaycastHit2D[] hits)
        {
            if (!hits.Any(hit => hit.transform.gameObject == this.pressedActive.gameObject)) { return false; }
            this.pressedActive.Use();
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
            int blockHightestLayer = -5;
            int activeHightestLayer = 0;
            foreach (var hit in hits)
            {
                Transform blockTransform = hit.transform;
                if (blockTransform.tag == "Block")
                {
                    Block temp = blockTransform.GetComponent<SubBlock>().Parent.Parent;
                    if (temp.CurrentLayerSorting > blockHightestLayer)
                    {
                        blockHightestLayer = temp.CurrentLayerSorting;
                        this.pressedBlock = temp;
                        this.downPosition = (Input.mousePosition - this.pressedBlock.transform.position * this.screenRatio);
                    }
                    Debug.Log(pressedBlock.name);
                }
                else if (blockTransform.tag == "Active")
                {
                    Active temp = blockTransform.GetComponent<Active>();
                    if (temp.Parent.CurrentLayerSorting > activeHightestLayer)
                    {
                        activeHightestLayer = temp.Parent.CurrentLayerSorting;
                        this.pressedActive = temp;
                    }
                }
                else if (blockTransform.tag == "Prototip")
                {
                    this.pressedPrototip = blockTransform.GetComponent<BlockPrototip>();
                }
            }
        }
    }
}
