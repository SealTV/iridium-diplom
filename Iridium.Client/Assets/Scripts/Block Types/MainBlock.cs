using System;
using Scripts;
using Scripts.Blocks;
using UnityEngine;

public class MainBlock : Block {

    public SubBlocksField HighField;
    public SubBlocksField MiddleField;
    public SubBlocksField LowField;

	// Use this for initialization
	void Start () {
        this.LayerSorting = 0;
        this.ReSortingLayers(this.LayerSorting);
	}
	
	// Update is called once per frame
	void Update () {
	    this.Streach();
	}

    public override float GetHeight()
    {
        return 0;
    }

    public override float GetWidth()
    {
        return 0;
    }

    public override void Streach()
    {
        float BodyHeightStretch = 2;
        Block block;
        if (this.Connectors.TryGetValue("InnerConnector", out block))
        {
            BodyHeightStretch = block.GetHeight();
            HeadWidthStretch = block.GetWidth();
        }


        this.HighField.Stretch(HeadWidthStretch, HeadHeightStretch - 1.5f);

        this.MiddleField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight());
        this.MiddleField.Stretch(1, BodyHeightStretch -2);

        this.LowField.transform.localPosition = new Vector3(0, -this.HighField.GetHeight() - this.MiddleField.GetHeight());
        this.LowField.Stretch(HeadWidthStretch - 2, 0);
    }

    public override void ReSortingLayers(int layer)
    {
    }
}
