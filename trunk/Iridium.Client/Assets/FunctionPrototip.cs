namespace Assets
{
    using Scripts;
    using Scripts.Block_Types;
    using UnityEngine;

    public class FunctionPrototip : BlockPrototip {

        public string Code;
        public ConnectorType ConnectorType;
        public ConnectorType OutputType;
        public Sprite RendererSprite;

        public override Block GetInstanse()
        {
            var instance = (GameObject)Instantiate(InstantiatedPrefab);
            var inst = instance.GetComponent<FunctionBlock>();
            
            inst.Code = this.Code;
            inst.ParameterConnector.ConnectorType = this.ConnectorType;
            inst.Renderer.sprite = this.RendererSprite;
            inst.InputConnector.ConnectorType = this.OutputType;

            instance.transform.position = this.transform.position;
            return inst;
        }
    }
}
