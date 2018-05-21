using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class called by melee enemy to determine how much damage is mitigated by player armour.

public class DamageReductionArena
{

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
        if (HeroController.hasArmour)
        {
            damageDealt -= 5;
        }
        //shield
        if (HeroController.hasShield && HeroController.isSlot4)
        {
            damageDealt -= 5;
        }
        //legs
        if (HeroController.hasLegs)
        {
            damageDealt -= 3;
            int variance = Random.Range(1, 4);
            thornsDamage += variance;

        }
        //helm
        if (HeroController.hasHelm)
        {
            damageDealt -= 1;
        }
        //boots
        if (HeroController.hasBoots)
        {
            damageDealt -= 1;
        }


        //ensure player is not healed by "negative damage"...
        if (damageDealt <= 0)
        {
            HeroController.battery -= 1;
        }
        else
        {
            HeroController.battery -= damageDealt;
        }

        return thornsDamage;
    }
}
