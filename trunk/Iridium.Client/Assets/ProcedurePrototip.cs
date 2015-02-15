using UnityEngine;
using System.Collections;
using Assets;
using Assets.Scripts;
using Assets.Scripts.Block_Types;

public class ProcedurePrototip : BlockPrototip {


    public string Code;
    public ConnectorType ConnectorType;
    public Sprite RendererSprite;

    public override Block GetInstanse()
    {
        var instance = (GameObject)Instantiate(InstantiatedPrefab);
        var inst = instance.GetComponent<FunctionBlock>();

        inst.Code = this.Code;
        inst.ParameterConnector.ConnectorType = this.ConnectorType;
        inst.Renderer.sprite = this.RendererSprite;

        instance.transform.position = this.transform.position;
        return inst;
    }
}
