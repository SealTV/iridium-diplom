namespace Assets.Scripts
{
    using global::Scripts.Blocks;
    using UnityEngine;

    public class AddVariable : MonoBehaviour
    {
        public ForeachBlock Parent;
        public GameObject VariablePrototip;

        public Block GetInstanse()
        {
            var instance = (GameObject) Instantiate(VariablePrototip);
            var variable = instance.GetComponent<VariableBlock>();
            instance.transform.position = this.transform.position;
            variable.Name = Parent.Iterator.text;
            return variable;
        }
    }
}
