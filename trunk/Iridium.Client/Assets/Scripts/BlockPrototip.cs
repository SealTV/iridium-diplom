using Assets.Scripts;
using Scripts.Blocks;
using UnityEngine;
using System.Collections;

public class BlockPrototip : MonoBehaviour {

    public float Height;
    public string Type;
    public GameObject InstantiatedPrefab;

    public Block GetInstanse()
    {
        var instance = (GameObject)Instantiate(InstantiatedPrefab);
        instance.transform.position = this.transform.position;
        return instance.GetComponent<Block>();
    }
}
