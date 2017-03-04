using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level3BossSP : MonoBehaviour {

    private AudioSource source;
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    public GameObject target;
    public bool isAggroed;
    private bool hasDied;
    private float distanceX;
    private float distanceZ;
    private float distanceY;

    private int bossHealth = 340;
    private int maxBossHealth = 340;

    public GameObject levelWellSelector;
    public GameObject teleSpot1;
    public GameObject teleSpot2;
    private GameObject boss;
    public GameObject HurtSpecEffect;
    public GameObject BloodSpecEffect;
    public GameObject BiteSpecEffect1;
    public GameObject BiteSpecEffect2;
    public GameObject TeleportExplosion;
    public GameObject SummonedVampire;
    public AudioClip TeleSound;
    public AudioClip OnDeathSound;

    public int bossDamage = 8;


    private bool isTransition;

    private bool hasDoneFirstTransition;
    private bool hasDoneSecondTransition;


    private float transitionCDTimer;
    private float CDTimer;
    private float cooldown = 3.0f;

    public Slider bossHealthSlider;
    public Image Fill; 

    // Use this for initialization
    void OnEnable()
    {
        source = GetComponent<AudioSource>();
        boss = GameObject.Find("Level3Boss");
        isTransition = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.Find("BatteryBot");
        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        anim.SetBool("IsAggroed", false);
        isAggroed = false;
        hasDied = false;

        bossHealthSlider.value = 10;
        bossHealthSlider.gameObject.SetActive(false);
        levelWellSelector.SetActive(false);
    }

    private void UpdateSlider()
    {
        bossHealthSlider.value = bossHealth;

        Fill.color = Color.green;

    }
    // Update is called once per frame
    void Update () {
        if (!hasDied)
        {
            transitionCDTimer -= 0.01f;
            CDTimer -= 0.01f;

            UpdateSlider();

            if (!isTransition)
            {
                
                distanceX = this.transform.position.x - target.transform.position.x;
                distanceZ = this.transform.position.z - target.transform.position.z;
                distanceY = this.transform.position.y - target.transform.position.y;
                //idle
                if (!isAggroed)
                {
                    checkAggro();
                }
                else
                {
                    //Regular gameplay
                    moveToPlayer();
                }

            }
            else
            {
                //we are in transition
                if (CDTimer < 0.01f)
                {
                    anim.SetTrigger("IsSummoning");
                    Instantiate(SummonedVampire, teleSpot1.transform.position, teleSpot1.transform.rotation);
                    Instantiate(SummonedVampire, teleSpot1.transform.position, teleSpot1.transform.rotation);
                    Instantiate(SummonedVampire, teleSpot1.transform.position, teleSpot1.transform.rotation);
                    Instantiate(TeleportExplosion, teleSpot1.transform.position, teleSpot1.transform.rotation);
                    source.PlayOneShot(TeleSound, 0.5f);
                    CDTimer = 3.0f;
                }
                if (transitionCDTimer < 0.01f)
                {
                    //go back down, it's over
                    anim.SetBool("IsAggroed", true);
                    boss.transform.position = teleSpot1.transform.position;
                    Instantiate(TeleportExplosion, teleSpot1.transform.position, teleSpot1.transform.rotation);
                    source.PlayOneShot(TeleSound, 0.5f);
                    isTransition = false;
                    SafetyDanceGridSP.isTransition = false;
                    agent.enabled = true;
                    agent.Resume();
                }

            }
        }
	}

    void moveToPlayer()
    {

        if ((distanceX > -2.5 && distanceX < 2.5) && (distanceZ > -2.5 && distanceZ < 2.5) && (distanceY > -3 && distanceY < 3) )
        {
            if (CDTimer < 0.01f)
            {
                anim.SetBool("IsAggroed", false);
                agent.Stop();
                anim.SetTrigger("ForceIdle");
                anim.SetBool("IsInMeleeRange", true);
                anim.SetTrigger("isAttacking");
                DamageReduction();



                CDTimer = cooldown;

                int randomNum = Random.Range(1, 3);
                switch (randomNum)
                {
                    case 1:
                        Instantiate(BiteSpecEffect1, this.transform.position, this.transform.rotation);
                        break;
                    case 2:
                        Instantiate(BiteSpecEffect2, this.transform.position, this.transform.rotation);
                        break;
                }
            } 
            
        }
        else
        {
            agent.Resume();
            anim.SetBool("IsAggroed", true);
            anim.SetBool("IsInMeleeRange", false);
            if (agent.isActiveAndEnabled)
            {
                agent.SetDestination(target.transform.position);
            }

        }

    }




    public void transition()
    {
        //go up
        anim.SetTrigger("ForceIdle");
        anim.SetBool("IsAggroed", false);
        agent.enabled = false;
        boss.transform.position = teleSpot2.transform.position;
        Instantiate(TeleportExplosion, teleSpot2.transform.position, teleSpot2.transform.rotation);
        transitionCDTimer = 10.0f;
        
    }
    //only happens once
    private void checkAggro()
    {
        if ((distanceX > -20 && distanceX < 20) && (distanceZ > -20 && distanceZ < 20) && (distanceY > -10 && distanceY < 10))
        {
            bossHealthSlider.gameObject.SetActive(true);

            isAggroed = true;
            SafetyDanceGridSP.isActive = true;
            anim.SetBool("IsAggroed", true);
            agent.enabled = false;
            boss.transform.position = teleSpot1.transform.position;
            Instantiate(TeleportExplosion, teleSpot1.transform.position, teleSpot1.transform.rotation);
            agent.enabled = true;
            agent.Resume();           
        }        
    }


    public void OnCollisionEnter(Collision other)
    {
        //case when your player projectile hits the boss
        if (other.gameObject.name.Contains("Shot") && !hasDied)
        {
            
            if (other.gameObject.name.Contains("PlayerShot"))
            {
                //Note that multishot has the same damage - you just shoot a bunch at the same time
                if (HeroControllerSP.isSuperCharged == true)
                {
                    bossHealth -= 2;
                    Instantiate(HurtSpecEffect, other.transform.position, this.transform.rotation);
                }
                else
                {
                    bossHealth -= 1;
                    Instantiate(HurtSpecEffect, other.transform.position, this.transform.rotation);
                }
            }
            else if (other.gameObject.name.Contains("GS_Shot"))
            {
                if (other.gameObject.name.Contains("FIRE"))
                {
                    bossHealth -= 4;
                    Instantiate(BloodSpecEffect, other.transform.position, this.transform.rotation);
                }
                else
                {
                    bossHealth -= 2;
                    Instantiate(BloodSpecEffect, other.transform.position, this.transform.rotation);
                }

            }
            else if (other.gameObject.name.Contains("Axe_Shot"))
            {
                if (other.gameObject.name.Contains("LIGHTNING"))
                {
                    bossHealth -= 3;
                    Instantiate(BloodSpecEffect, other.transform.position, this.transform.rotation);
                }
                else
                {
                    bossHealth -= 1;
                    Instantiate(BloodSpecEffect, other.transform.position, this.transform.rotation);
                }

            }
            //note that shield shot IS the ice special... shield normally shoots an axe shot (because reasons)
            else if (other.gameObject.name.Contains("Shield_Shot"))
            {
                bossHealth -= 2;
                Instantiate(HurtSpecEffect, other.transform.position, this.transform.rotation);
            }

            if (bossHealth < (0.66 * maxBossHealth) && !hasDoneFirstTransition)
            {
                anim.SetTrigger("ForceIdle");
                anim.SetBool("IsAggroed", false);
                hasDoneFirstTransition = true;
                isTransition = true;
                SafetyDanceGridSP.isTransition = true;
                transition();
            }
            if (bossHealth < (0.33 * maxBossHealth) && !hasDoneSecondTransition)
            {
                anim.SetTrigger("ForceIdle");
                anim.SetBool("IsAggroed", false);
                hasDoneSecondTransition = true;
                isTransition = true;
                SafetyDanceGridSP.isTransition = true;
                transition();
            }
            //consume playershot
            Destroy(other.gameObject);
            if (bossHealth <= 0 && !hasDied)
            {
                levelWellSelector.SetActive(true);
                source.PlayOneShot(OnDeathSound, 1.0f);
                //fix to prevent this from occuring many times; 
                //turns out the gameobject isn't destroyed immediately so it can register many collisions...
                hasDied = true;
                SafetyDanceGridSP.isActive = false;

                bossHealthSlider.gameObject.SetActive(false);

                Destroy(other.gameObject);
                Destroy(this.gameObject);

            }

        }
    }

    private void DamageReduction()
    {
        int damageDealt = 0;

        //has armour and has shield
        if (HeroControllerSP.hasArmour && (HeroControllerSP.hasShield && HeroControllerSP.isSlot4))
        {
            damageDealt = (bossDamage - 10);
        }
        //has armour no shield
        else if (HeroControllerSP.hasArmour && (!HeroControllerSP.hasShield || !HeroControllerSP.isSlot4))
        {
            damageDealt = (bossDamage - 7);
        }
        //no armour has shield
        else if (!HeroControllerSP.hasArmour && (HeroControllerSP.hasShield && HeroControllerSP.isSlot4))
        {
            damageDealt = (bossDamage - 3);
        }
        //no armour no shield
        else
        {
            damageDealt = bossDamage;
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


    }
}
