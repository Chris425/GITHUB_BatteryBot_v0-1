using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*Purpose of this class is to be a generic solution that applies to all enemies. Now we only update it here, in one class.
* Further, enemies no longer need Special Effect game objects because GAMEMANAGER will have this script to do it.
*/

public class EnemyDamageReductionSP : MonoBehaviour{


    public bool willResultInHeal;

    //special effects done here.
    public GameObject gunSpecBlueEffect;
    public GameObject GunSpecEffect;
    public GameObject gunSpecSilverEffect;
    public GameObject gunSpecGoldEffect;
    public GameObject BloodSpecEffect;
    public GameObject SE_Heal;
    public GameObject SE_Hit_Poison;

    //hold damage values here for easy updating.
    private int gunShotDmg = 2;
    private int gunShotBlueDmg = 4;
    private int superChargeDmg = 2; //to be added on top of normal gunshot damage.
    private int gunShotSilverDmg = 10;
    private int gunShotGoldDmg = 15;
    private int gunShotBlueGoldDmg = 15;
    private int gsShotDmg = 5;
    private int gsShotFireDmg = 6;
    private int axeLightningMinDmg = 1;
    private int axeLightningMaxDmg = 3;
    private int axeShotDmg = 3;
    private int shieldIceShotDmg = 4;
    private int poisonShotDmg = 2;
    private int frozenOrbDmg = 3;

    //particles
    private int flameThrowerParticleDmg = 1;
    private int flameThrowerHitChanceMin = 0;
    private int flameThrowerHitChanceMax = 10;
    private int flameThrowerHitChanceThreshold = 4;


    private List<string> elementTypes = new List<string>();





    void OnEnable()
    {
        elementTypes.Add("NORMAL"); elementTypes.Add("FIRE");
        elementTypes.Add("POISON"); elementTypes.Add("ICE");
        elementTypes.Add("ETHEREAL");
    }


    /*
    @param string elementType can be NORMAL, FIRE, POISON, ICE, ETHEREAL (summoned). This is used to determine who is weak to what
    @param int armour is >= 0. Damage - armour = damageTaken, although this can never be negative or 0. Always 1.
    returns an int showing how much damage will be dealt to an enemy. If negative - enemy is healed.
*/
    public int collisionDmgTaken(Collision other, string elementType, int armour)
    {
        checkTypeExists(elementType);

        int tempDmg = 0;
        int damageTaken = 0;


        //case when your player projectile hits the vampire


        //make a special effect on death
        if (other.gameObject.name.Contains("PlayerShot"))
        {
            if (other.gameObject.name.Contains("PlayerShotBlue"))
            {
                if (HeroControllerSP.isSuperCharged == true)
                {
                    tempDmg = gunShotBlueDmg + superChargeDmg; GameObject.Instantiate(gunSpecBlueEffect, other.transform.position, other.transform.rotation);
                }
                else { tempDmg = gunShotBlueDmg; GameObject.Instantiate(gunSpecBlueEffect, other.transform.position, other.transform.rotation); }
            }
            else
            {
                if (HeroControllerSP.isSuperCharged == true)
                {
                    tempDmg = gunShotDmg + superChargeDmg; GameObject.Instantiate(GunSpecEffect, other.transform.position, other.transform.rotation);
                }
                else { tempDmg = gunShotDmg; GameObject.Instantiate(GunSpecEffect, other.transform.position, other.transform.rotation); }
            }
        }
        else if (other.gameObject.name.Contains("PlyrShot"))
        {
            if (other.gameObject.name.Contains("PlyrShotSilver"))
            {
                tempDmg = gunShotSilverDmg; GameObject.Instantiate(gunSpecSilverEffect, other.transform.position, other.transform.rotation);
            }
            else if (other.gameObject.name.Contains("PlyrShotGold"))
            {
                tempDmg = gunShotGoldDmg; GameObject.Instantiate(gunSpecGoldEffect, other.transform.position, other.transform.rotation);
            }
            else if (other.gameObject.name.Contains("PlyrShotBlueGold"))
            {
                tempDmg = gunShotBlueGoldDmg; GameObject.Instantiate(gunSpecBlueEffect, other.transform.position, other.transform.rotation);
            }

        }
        else if (other.gameObject.name.Contains("GS_Shot"))
        {
            if (other.gameObject.name.Contains("FIRE"))
            {
                if (elementType.Equals("FIRE"))
                {
                    tempDmg = -1;
                    GameObject.Instantiate(SE_Heal, other.transform.position, other.transform.rotation);
                }
                else
                {
                    tempDmg = gsShotFireDmg;
                    GameObject.Instantiate(BloodSpecEffect, other.transform.position, other.transform.rotation);
                }
                
            }
            else
            {
                if (elementType.Equals("ETHEREAL"))
                {
                    tempDmg = gsShotDmg - 1;
                    GameObject.Instantiate(BloodSpecEffect, other.transform.position, other.transform.rotation);
                }
                else
                {
                    tempDmg = gsShotDmg;
                    GameObject.Instantiate(BloodSpecEffect, other.transform.position, other.transform.rotation);
                }
                
            }

        }
        else if (other.gameObject.name.Contains("Axe_Shot"))
        {
            if (other.gameObject.name.Contains("LIGHTNING"))
            {
                int lightningVariance = UnityEngine.Random.Range(axeLightningMinDmg, axeLightningMaxDmg);
                tempDmg = lightningVariance;
                GameObject.Instantiate(BloodSpecEffect, other.transform.position, other.transform.rotation);
            }
            else
            {
                if (elementType.Equals("ETHEREAL"))
                {
                    tempDmg = axeShotDmg - 1;
                    GameObject.Instantiate(BloodSpecEffect, other.transform.position, other.transform.rotation);
                }
                else
                {
                    tempDmg = axeShotDmg;
                    GameObject.Instantiate(BloodSpecEffect, other.transform.position, other.transform.rotation);
                }
                
            }

        }
        //note that shield shot IS the ice special... shield normally shoots an axe shot (because reasons)
        else if (other.gameObject.name.Contains("Shield_Shot"))
        {
            if (elementType.Equals("ICE"))
            {
                tempDmg = -1;
                GameObject.Instantiate(SE_Heal, other.transform.position, other.transform.rotation);
            }
            else
            {
                tempDmg = shieldIceShotDmg;
                GameObject.Instantiate(GunSpecEffect, other.transform.position, other.transform.rotation);
            }
            
        }
        else if (other.gameObject.name.Contains("PlayerPsnWellShot"))
        {

            //it's a little bit OP... make it have a chance to fail
            int psnHitChance = UnityEngine.Random.Range(0, 10);
            if (psnHitChance > 4)
            {
                if (elementType.Equals("POISON"))
                {
                    tempDmg = -1;
                    GameObject.Instantiate(SE_Heal, other.transform.position, other.transform.rotation);
                }
                else
                {
                    tempDmg = poisonShotDmg;
                    GameObject.Instantiate(SE_Hit_Poison, other.transform.position, other.transform.rotation);
                }
            }
        }
        else if (other.gameObject.name.Contains("PlayerFRZOrbShot"))
        {
            if (elementType.Equals("ICE"))
            {
                tempDmg = -1;
                GameObject.Instantiate(SE_Heal, other.transform.position, other.transform.rotation);
            }
            else
            {
                tempDmg = frozenOrbDmg;
                GameObject.Instantiate(gunSpecBlueEffect, other.transform.position, other.transform.rotation);
            }

        }

       
        //heal case
        if (tempDmg < 0)
        {
            damageTaken = tempDmg;
            return damageTaken;
        }
        //non-heal, so do armour reduction
        else if(tempDmg > 0) //0 means it isn't something that should be hurting the enemy and we shouldn't even be here in this code :O
        {
            tempDmg -= armour;
            if (tempDmg <= 0)
            {
                tempDmg = 1;
            }
            damageTaken = tempDmg;
            return damageTaken;
        }
        return damageTaken;

    }

    public int particleDmgTaken(GameObject particle, string elementType, int armour)
    {
        checkTypeExists(elementType);

        int tempDmg = 0;
        int damageTaken;

        if (elementType.Equals("FIRE"))
        {
            tempDmg = -2;
        }
        else
        {
            int FT_hitChance = UnityEngine.Random.Range(flameThrowerHitChanceMin, flameThrowerHitChanceMax);
            if (FT_hitChance > flameThrowerHitChanceThreshold)
            {
                tempDmg -= flameThrowerParticleDmg;
                Instantiate(GunSpecEffect, this.transform.position, this.transform.rotation);
            }
        }
        

        //heal case
        if (tempDmg < 0)
        {
            damageTaken = tempDmg;
            return damageTaken;
        }
        //non-heal, so do armour reduction
        else
        {
            tempDmg -= armour;
            if (tempDmg <= 0)
            {
                tempDmg = 1;
            }
            damageTaken = tempDmg;
            return damageTaken;
        }


    }

    private void checkTypeExists(String elementType)
    {
        if (!elementTypes.Contains(elementType))
        {
            Debug.Log("[WARN] Element type of " + elementType + " does not exist in list of known element types.");
        }
    }
}
