using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// buffs the attribute that this obj is designed to buff
// then it self destructs
public class isConsumableSP : Mixin {

    public List<Data> buffs; //The attributes we try to buff if the recipient has them.
    public GameObject BatterySpecEffect1;
    public GameObject BatterySpecEffect2;
    public GameObject BatterySpecEffect3;

    public void Consume()
	{
		// apply buffs to recipient - deprecated
		//ApplyBuffs();

		// then self destruct
		Destroy(this.gameObject);
	}


    public void ConsumeBattery()
    {
        //generate different special effect when you pick up a battery for some variety.
        //different sound and spark effects! This will determine how much charge you get back.
        int randomNum = Random.Range(1, 4);
        switch (randomNum)
        {
            case 1:
                Instantiate(BatterySpecEffect1, this.transform.position, this.transform.rotation);
                HeroControllerSP.battery += 9;
                break;
            case 2:
                Instantiate(BatterySpecEffect2, this.transform.position, this.transform.rotation);
                HeroControllerSP.battery += 12;
                break;
            case 3:
                Instantiate(BatterySpecEffect3, this.transform.position, this.transform.rotation);
                HeroControllerSP.battery += 5;
                break;
        }
        
        
        //then self destruct
        Destroy(this.gameObject);
    }


    public void ConsumeRedBattery()
    {
        //Red batteries will give you more ammo and a small amount of battery charge
        int randomNum = Random.Range(1, 4);
        switch (randomNum)
        {
            case 1:
                Instantiate(BatterySpecEffect1, this.transform.position, this.transform.rotation);
                HeroControllerSP.Ammo += 20;
                HeroControllerSP.battery += 1;
                break;
            case 2:
                Instantiate(BatterySpecEffect2, this.transform.position, this.transform.rotation);
                HeroControllerSP.Ammo += 20;
                HeroControllerSP.battery += 5;
                break;
            case 3:
                Instantiate(BatterySpecEffect3, this.transform.position, this.transform.rotation);
                HeroControllerSP.Ammo += 20;
                HeroControllerSP.battery += 2;
                break;
        }


        //then self destruct
        Destroy(this.gameObject);
    }



    public void ConsumeSuperBattery()
    {
        //super battery gives ammo and charge
        Instantiate(BatterySpecEffect2, this.transform.position, this.transform.rotation);
        Instantiate(BatterySpecEffect1, this.transform.position, this.transform.rotation);
        Instantiate(BatterySpecEffect1, this.transform.position, this.transform.rotation);
        Instantiate(BatterySpecEffect1, this.transform.position, this.transform.rotation);
        Instantiate(BatterySpecEffect1, this.transform.position, this.transform.rotation);
        Instantiate(BatterySpecEffect3, this.transform.position, this.transform.rotation);
        HeroControllerSP.Ammo += 50;
        HeroControllerSP.battery += 50;
        

        //then self destruct
        Destroy(this.gameObject);
    }

    public void ConsumeGearUpBattery()
    {
        //Gear up battery makes you combat ready
        Instantiate(BatterySpecEffect2, this.transform.position, this.transform.rotation);
        Instantiate(BatterySpecEffect1, this.transform.position, this.transform.rotation);
        Instantiate(BatterySpecEffect1, this.transform.position, this.transform.rotation);
        if (HeroControllerSP.Ammo < 300)
        { HeroControllerSP.Ammo = 300;}

        HeroControllerSP.hasAxe = true;
        HeroControllerSP.hasGun = true;
        HeroControllerSP.hasShield = true;
        HeroControllerSP.hasGS = true;
        HeroControllerSP.hasJetBooster = true;
        HeroControllerSP.isSlot2 = true;
        HeroControllerSP.isSlot4 = true;

        //if player health is greater than 100 don't reduce it; only bring them up to 100 if they're lower.
        if (HeroControllerSP.battery < 100)
        {
            HeroControllerSP.battery = 100;
        }


        //then self destruct
        Destroy(this.gameObject);
    }

    //   public void ApplyBuffs()
    //{
    //	// all buffs attached to the object
    //	Data[] attachedBuffs = GetComponents<Data>();
    //	Data[] recipientAttributes = GetRecipient().GetComponents<Data>(); //who is the recipient of this consume?

    //	// now, double nested loop, check for components that match by name (and type?)
    //	foreach (Data d in attachedBuffs)
    //	{
    //		// for each buff attached to us..
    //		foreach (Data attrib in recipientAttributes)
    //		{
    //			// check for an attribute with same name on recipient
    //			if (d.name == attrib.name)
    //			{
    //				// the names match, so apply the value from buff to attrib
    //				IntData intd = (attrib as IntData);
    //				if (intd != null)
    //					intd.IncData( (d as IntData));
    //			}
    //		}
    //	}
    //}

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
