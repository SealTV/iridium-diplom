namespace Assets
{
    using global::Scripts.Blocks;
    using global::Scripts.Block_Types;
    using Scripts;
    using Scripts.Block_Modules;
    using Scripts.Block_Types;
    using UnityEngine;

    public class GetInputParameters : VariableInstantiater
    {
        public MainBlock Parent;
        public GameObject VariablePrototip;

        public override Block GetInstanse()
        {
            var instance = (GameObject)Instantiate(this.VariablePrototip);
            var variable = instance.GetComponent<CollectionBlock>();
            instance.transform.position = this.transform.position;
            return variable;
        }
    }
}
