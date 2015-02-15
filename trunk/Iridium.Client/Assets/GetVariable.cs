using Assets.Scripts.Block_Modules;
using Assets.Scripts.Block_Types;

namespace Assets
{
    using Scripts;
    using UnityEngine;

    public class GetVariable : VariableInstantiater
    {
        public GameObject VariableBlock;
        public InstanceBlock InstanceBlock;
        // Use this for initialization
        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
	
        }

        public override Block GetInstanse()
        {
            var instance = (GameObject) Instantiate(this.VariableBlock);
            var inst = instance.GetComponent<VariableBlock>();
            inst.InputConnector.ConnectorType = this.InstanceBlock.GetConnectorType();
            inst.Name = this.InstanceBlock.VariableName.text;
            instance.transform.position = this.transform.position;
            return inst;
        }
    }
}
