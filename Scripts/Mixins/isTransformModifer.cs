using UnityEngine;
using System.Collections;

public class isTransformModifer : Mixin {

	public string OnModifyTransformFinishedCB;

	public bool ModifyPos;
	public Vector3 posModifier;

	public bool ModifyRot;
	public Vector3 rotModifier;

	public bool ModifyScale;
	public Vector3 scaleModifier;

	public void ModifyTransform()
	{
		// modifies transform of the recipient, if flag is set
		if (ModifyScale)
			GetRecipient().transform.localScale = scaleModifier;

		if (OnModifyTransformFinishedCB != "")
			SendMessage(OnModifyTransformFinishedCB);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
