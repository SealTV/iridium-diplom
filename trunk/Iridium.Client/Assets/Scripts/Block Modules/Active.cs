using Scripts.Blocks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Active : MonoBehaviour
    {

        public Block Parent;

        public abstract void Use();
        // Use this for initialization
        void Start () {
    	
        }
	
        // Update is called once per frame
        void Update () {
	
        }
    }
}
