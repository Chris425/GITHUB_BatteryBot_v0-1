using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//
//	this mixin responds to a gameobject event, and dispatches a message
//
public class isTouchableSP : Mixin {

	public List<string> onTouchCBs;
	public List<string> touchTypes;	// objects of this type, can responds to touch events

	public void OnCollisionEnter(Collision col)
	{
		// so if the thing that touched me, should respond to this message
		GameObject ColObj = col.gameObject;

		// does ColObj has a component of type 'touchType'?
		// fixme: there is a bug in here... okay?!
		foreach(string s in touchTypes)
		{
			// we only care if we find the first touchtype that matches...
			if (ColObj.GetComponent(s) != null)
			{
				// here, we know who touched us... so tell components attached to us
				// as needed...
				isConsumable isCons = this.gameObject.GetComponent<isConsumable>();
				isTransformModifer isTM = this.gameObject.GetComponent<isTransformModifer>();
				isEquippable isEq = this.gameObject.GetComponent<isEquippable>();
				if (isTM != null)
					isTM.SetRecipient(ColObj);

				if (isCons != null)
					isCons.SetRecipient(ColObj);

				if (isEq != null)
					isEq.SetRecipient(ColObj);

				// dispatch the message(s) 
				foreach( string touchcb in onTouchCBs)
				{
					if (touchcb != "")
						SendMessage(touchcb);
                    
				}
			}
		}
	}
		
	public void DebugMsg()
	{
		Debug.Log("I was touched!");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
