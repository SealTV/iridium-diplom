using UnityEngine;

namespace Assets.Scripts
{
    public class DownList : Active
    {
        public GameObject DownListGameObject;
        

        // Use this for initialization
        public override void Use()
        {
            this.DownListGameObject.SetActive(true);    
        }

        public override void UnUse()
        {
            this.DownListGameObject.SetActive(false);
        }

        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
	
        }
    }
}
