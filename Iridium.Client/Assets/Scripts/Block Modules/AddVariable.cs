namespace Assets.Scripts
{
    using Block_Modules;
    using Block_Types;
    using global::Scripts.Blocks;
    using global::Scripts.Block_Types;
    using UnityEngine;

    public class AddVariable : VariableInstantiater
    {
        public ForeachBlock Parent;
        public GameObject VariablePrototip;

        public override Block GetInstanse()
        {
            var instance = (GameObject) Instantiate(VariablePrototip);
            var variable = instance.GetComponent<VariableBlock>();
            instance.transform.position = this.transform.position;
            variable.Name = this.Parent.Iterator.text;
            Block block;
            if (Parent.Connectors.TryGetValue("ConditionConnector", out block))
            {
                variable.InputConnector.ConnectorType = ((CollectionBlock) block).CollectionElement;
            }
            return variable;
        }
    }
}
