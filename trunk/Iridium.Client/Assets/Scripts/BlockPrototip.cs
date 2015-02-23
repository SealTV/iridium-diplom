using Assets.Scripts.Block_Types;
using UnityEngine;

namespace Assets.Scripts
{
    public class BlockPrototip : MonoBehaviour {

        public float Height;
        public string Type;
        public GameObject InstantiatedPrefab;

        public virtual Block GetInstanse()
        {
            var instance = (GameObject)Instantiate(this.InstantiatedPrefab);
            instance.transform.position = this.transform.position;
            return instance.GetComponent<Block>();
        }
    }
}
