using Scripts.Blocks;
using UnityEngine;

namespace Assets.Scripts
{
    using Block_Types;

    public abstract class Active : MonoBehaviour
    {

        public Block Parent;

        public abstract void Use();
        public abstract void UnUse();
        // Use this for initialization
        void Start () {
    	
        }
	
        // Update is called once per frame
        void Update () {
	
        }
    }
}
