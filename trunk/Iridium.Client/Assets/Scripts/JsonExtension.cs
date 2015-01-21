namespace Scripts
{
    using SimpleJSON;
    using UnityEngine;

    public static class JsonExtension
    {
        public static Vector3 AsVector3(this JSONNode json)
        {
            string[] vector = json.Value.Replace(" ", "").Split(';');
            return new Vector3(float.Parse(vector[0]), -float.Parse(vector[1]));
        }
    }
}