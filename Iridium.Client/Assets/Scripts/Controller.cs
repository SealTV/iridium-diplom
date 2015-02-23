namespace Scripts
{
    using System;
    using System.Linq;
    using Assets.Scripts;
    using Assets.Scripts.Block_Modules;
    using Assets.Scripts.Block_Types;
    using Blocks;
    using UnityEngine;

    public class Controller : MonoBehaviour
    {
        private bool isBlockTaken;
        private Block         pressedBlock;
        private Active        pressedActive;
        private Active        lastPressedActive;
        private VariableInstantiater   pressedAddVariable;
        private BlockPrototip pressedPrototip;
        private Vector3 downPosition;
        private float screenRatio;
        public MainBlock MainBlock;

        public Transform Scaler;
        public float ScaleSpeed = 0.3f;
        public float ScaleMin = 0.5f;
        public float ScaleMax = 1f;
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

        private void EndCreateBlock(Block block)
        {
            block.transform.localScale = block.BaseScale*this.scale;
            this.GetBlock(block);
        }
        private void GetBlock(Block block)
        {
            this.pressedBlock = block;
            this.downPosition = (Input.mousePosition - block.transform.position * this.screenRatio);
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
                if ((Input.mousePosition - this.mousePressedPosition).magnitude > this.minMouseShift)
                {
                    if (this.pressedAddVariable != null)
                    {
                        this.EndCreateBlock(this.pressedAddVariable.GetInstanse());
                        this.pressedAddVariable = null;
                    }
                    else if (this.pressedBlock != null)
                    {
                        this.PickUpBlock();
                    }
                    else if (this.pressedPrototip != null)
                    {
                        this.EndCreateBlock(this.pressedPrototip.GetInstanse());
                        this.pressedPrototip = null;
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
            float deltaScale = this.ScaleSpeed*Input.GetAxis("Mouse ScrollWheel");
            if (Math.Abs(deltaScale) > 0.09)
            {
                this.scale +=deltaScale;
                this.scale = Math.Min(Math.Max(ScaleMin, this.scale), ScaleMax);
                this.Scaler.localScale = new Vector3(this.scale, this.scale, this.scale);
            }
        }

        private bool TryUsingActive(RaycastHit2D[] hits)
        {
            if (!hits.Any(hit => hit.transform.gameObject == this.pressedActive.gameObject)) { return false; }
            this.pressedActive.Use();
            this.lastPressedActive = this.pressedActive;
            return true;
        }

        private bool TryPasteBlock(RaycastHit2D[] hits)
        {
            foreach (var hit in hits)
            {
                Transform hitTransform = hit.transform;
                if (hitTransform.tag == "Bin")
                {
                    if(this.pressedBlock.name!="Main")
                    Destroy(this.pressedBlock.gameObject);
                    this.pressedBlock = null;
                    return true;
                }
                if (hitTransform.name.Contains("Connector"))
                {
                    var connector = hitTransform.GetComponent<Connector>();
                    Block parent = connector.Parent;
                    if (!parent.Connectors.ContainsKey(hitTransform.name) &&
                        (connector.ConnectorType == this.pressedBlock.InputConnector.ConnectorType || connector.ConnectorType==ConnectorType.Value))
                    {
                        parent.Connectors[hitTransform.name] = this.pressedBlock;
                        this.pressedBlock.Parent = parent;
                        this.pressedBlock.ParentConnector = hitTransform.name;
                        this.pressedBlock.transform.parent = hitTransform.transform;
                        this.pressedBlock.transform.position = hitTransform.position;
                        this.pressedBlock.ReSortingLayers(parent.CurrentLayerSorting + 2);
                        this.pressedBlock.Parent.Stretch();
                        if (connector.ConnectorType == ConnectorType.Value)
                        {
                            connector.Parent.ChooseType(this.pressedBlock.InputConnector.ConnectorType);
                        }
                        return true;
                    }
                }
            }
            this.pressedBlock.transform.parent = this.Scaler;
            return false;
        }

        private void PickUpBlock()
        {
            this.isBlockTaken = true;
            if (this.pressedBlock.Parent != null)
                this.pressedBlock.Parent.Connectors.Remove(this.pressedBlock.ParentConnector);
            this.pressedBlock.ReSortingLayers(20001);
            this.pressedBlock.transform.parent = null;
            var parent = this.pressedBlock.Parent;
            this.pressedBlock.Parent = null;
            if (parent != null)
            {
                parent.Stretch();
                parent.UnChooseType();
            }
        }

        private void TryFindBlock(RaycastHit2D[] hits)
        {
            this.pressedBlock = null;
            this.pressedAddVariable = null;
            int blockHightestLayer = -5;
            int activeHightestLayer = 0;

            foreach (var hit in hits)
            {
                Transform blockTransform = hit.transform;
                if (blockTransform.tag == "Block")
                {
                    Block temp = blockTransform.GetComponentInParent<Block>();
                    if (temp == null)
                    {
                        temp = blockTransform.GetComponent<SubBlock>().Parent.Parent;
                        Debug.Log("Parent");
                    }

                    if (temp.CurrentLayerSorting > blockHightestLayer)
                    {
                        blockHightestLayer = temp.CurrentLayerSorting;
                        this.pressedBlock = temp;
                        this.downPosition = (Input.mousePosition - this.pressedBlock.transform.position * this.screenRatio);
                    }
                }
                else if (blockTransform.tag == "Active")
                { 
                    Active temp = blockTransform.GetComponent<Active>();
                    if (temp.Parent.CurrentLayerSorting > activeHightestLayer)
                    {
                        Debug.Log("Active");
                        activeHightestLayer = temp.Parent.CurrentLayerSorting;
                        this.pressedActive = temp;
                    }
                }
                else if (blockTransform.tag == "Prototip")
                {
                    this.pressedPrototip = blockTransform.GetComponent<BlockPrototip>();
                }
                else if (blockTransform.tag == "VariableInstanse")
                {
                    this.pressedAddVariable = blockTransform.GetComponent<VariableInstantiater>();
                }

            }
            if (lastPressedActive != null)
            {
                lastPressedActive.UnUse();
                lastPressedActive = null;
            }
        }

        public void SendCode()
        {
            string code = MainBlock.GetCode();
        }
    }
}
