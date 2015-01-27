namespace Scripts.Blocks
{
    using UnityEngine;

    public class SimpleBlock : MonoBehaviour
    {

        private float length = 1;
        public Transform LeftSide;
        public Transform RightSide;
        public Transform Center;
        public float speed = 5;



        // Update is called once per frame
        private void Update()
        {
            this.length += Time.deltaTime * Input.GetAxis("Horizontal") * this.speed;
            this.Center.localScale = new Vector3(this.length, 1, 1);
            this.RightSide.localPosition = new Vector3(0.3f * this.length + 0.3f, 0, 0);

        }
    }
}
