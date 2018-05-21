using UnityEngine;
using System.Collections;

public class isEquipSlot : Mixin {

	public GameObject obj;// null means open, !null means slotted..?
	public enum eSlot
	{
		eInvalid = -1,
		eNone = 0,
		eHand,
		eHead,
		eFoot,
		eBody
	};
	public eSlot slotType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
