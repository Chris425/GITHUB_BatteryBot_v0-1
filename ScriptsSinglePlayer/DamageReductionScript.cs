using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class called by melee enemy to determine how much damage is mitigated by player armour.

public class DamageReductionScript{

    //do separate if checks for each piece of gear. Damage reduction for each piece.
    //if damage dealt by monster is negative, simply do 1 damage to player.
    //@return int - thorns damage returned to attacker.
    public int DamageReduction(int damage)
    {

        //Unmitigated damage to be dealt.
        int damageDealt = damage;

        //are we returning damage to enemy?
        int thornsDamage = 0;

        //armour
        if (HeroControllerSP.hasArmour)
        {
            damageDealt -= 5;
        }
        //shield
        if (HeroControllerSP.hasShield && HeroControllerSP.isSlot4)
        {
            damageDealt -= 5;
        }
        //legs
        if (HeroControllerSP.hasLegs)
        {
            damageDealt -= 3;
            int variance = Random.Range(1, 4);
            thornsDamage += variance;
            
        }
        //helm
        if (HeroControllerSP.hasHelm)
        {
            damageDealt -= 1;
        }
        //boots
        if (HeroControllerSP.hasBoots)
        {
            damageDealt -= 1;
        }


        //ensure player is not healed by "negative damage"...
        if (damageDealt <= 0)
        {
            HeroControllerSP.battery -= 1;
        }
        else
        {
            HeroControllerSP.battery -= damageDealt;
        }

        return thornsDamage;
    }
}
