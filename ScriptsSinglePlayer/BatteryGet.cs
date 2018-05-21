using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryGet : MonoBehaviour {

    public GameObject BatterySpecEffect1;
    public GameObject BatterySpecEffect2;
    public GameObject BatterySpecEffect3;

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Contains("InteractShot") || col.gameObject.name.Contains("PLAYERBASE"))
        {
            if (this.name.Contains("Red"))
            {
                ConsumeRedBattery();
            }
            else if (this.name.Contains("GearUp"))
            {
                ConsumeGearUpBattery();
            }
            else if (this.name.Contains("Super"))
            {
                ConsumeSuperBattery();
            }
            else
            {
                ConsumeBattery();
            }

            
        }
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
                HeroController.battery += 12;
                break;
            case 2:
                Instantiate(BatterySpecEffect2, this.transform.position, this.transform.rotation);
                HeroControllerSP.battery += 12;
                HeroController.battery += 16;
                break;
            case 3:
                Instantiate(BatterySpecEffect3, this.transform.position, this.transform.rotation);
                HeroControllerSP.battery += 5;
                HeroController.battery += 9;
                break;
        }

        GAMEMANAGERSP.numScore += 1;
        GAMEMANAGERSP.numArenaScore += 1;
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
                HeroController.Ammo += 20;
                HeroController.battery += 3;
                break;
            case 2:
                Instantiate(BatterySpecEffect2, this.transform.position, this.transform.rotation);
                HeroControllerSP.Ammo += 20;
                HeroControllerSP.battery += 5;
                HeroController.Ammo += 20;
                HeroController.battery += 8;
                break;
            case 3:
                Instantiate(BatterySpecEffect3, this.transform.position, this.transform.rotation);
                HeroControllerSP.Ammo += 20;
                HeroControllerSP.battery += 2;
                HeroController.Ammo += 20;
                HeroController.battery += 5;
                break;
        }

        GAMEMANAGERSP.numScore += 1;
        GAMEMANAGERSP.numArenaScore += 1;
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
        HeroController.Ammo += 50;
        HeroController.battery += 75;

        GAMEMANAGERSP.numScore += 5;
        GAMEMANAGERSP.numArenaScore += 5;
        //then self destruct
        Destroy(this.gameObject);
    }

    //Doesn't exist in arena mode!
    public void ConsumeGearUpBattery()
    {
        //Gear up battery makes you combat ready
        Instantiate(BatterySpecEffect2, this.transform.position, this.transform.rotation);
        Instantiate(BatterySpecEffect1, this.transform.position, this.transform.rotation);
        Instantiate(BatterySpecEffect1, this.transform.position, this.transform.rotation);
        if (HeroControllerSP.Ammo < 300)
        { HeroControllerSP.Ammo = 300; }

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

        GAMEMANAGERSP.numScore += 3;
        //then self destruct
        Destroy(this.gameObject);
    }


}
