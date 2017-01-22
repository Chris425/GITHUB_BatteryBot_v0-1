using UnityEngine;
using System.Collections;

public class isEquippableSP : Mixin {

	public isEquipSlot.eSlot slotType;
    public RuntimeAnimatorController HeroAnimCtrlr;
    public AnimatorOverrideController MeleeCtrlr;
    //bad solution :(
    public static bool shouldDequipGun = false;

    public void Equip()
	{
		//search through the hierarchy of the recipient, and find the first
		//slot that is empty where we can attach this object.
		// then, if found, attach it there.
		GameObject rec = GetRecipient();
		if (rec != null)
		{
			isEquipSlot[] slots = rec.GetComponentsInChildren<isEquipSlot>();

			// search for an open slot that matches our slotType
			foreach( isEquipSlot ies in slots)
			{
				if (ies.slotType == slotType)
				{
					// slot the item if the slot is empty!
					if (ies.obj == null) 
					{
						this.gameObject.transform.parent = ies.gameObject.transform;
						this.gameObject.transform.localPosition = Vector3.zero; // zeroing the position means put it at origin of parent
						ies.obj = this.gameObject; // put the actual object, into the slot.
					}
                    
				}
			}
		}
		else
			Debug.Log("isEquipSlot::Equip(): Error, recipient is null");

        //case for Gun Wep
        if (this.gameObject.name.Contains("PlayerGun"))
        {
            HeroControllerSP.hasGun = true; 
        }
        //case for melee weapons
        //else if (this.gameObject.name.Contains("PlayerGun"))
        //{
        //    //change the recipient's anim...

        //    Animator anim = rec.gameObject.GetComponentInChildren<Animator>();
        //    anim.runtimeAnimatorController = MeleeCtrlr;
        //    GameObject Hero = GameObject.Find("Hero");
        //    Hero.SendMessage("enableFireFlower");
        //}

    }

	public void Dequip()
	{
        GameObject rec = GetRecipient();
        Animator anim = rec.gameObject.GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = HeroAnimCtrlr;

        HeroControllerSP.hasGun = false;

        Destroy(this.gameObject);
    }

	// Use this for initialization
	void Start () {
	
    }
	
	// Update is called once per frame
	void Update () {

        if (shouldDequipGun)
        {
            Dequip();
        }
	}
}
