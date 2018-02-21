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

    public GameObject spawnLoc;
    public GameObject levelWellSelector;
    public GameObject teleSpot1;
    public GameObject teleSpot2;
    private GameObject boss;
    public GameObject BiteSpecEffect1;
    public GameObject BiteSpecEffect2;
    public GameObject TeleportExplosion;
    public GameObject SummonedVampire;
    public GameObject coldObj;
    public AudioClip TeleSound;
    public AudioClip OnDeathSound;
    public GameObject SE_Hit_Poison;
    public GameObject SE_Thorns;
    public GameObject SE_WizDeath3;
    public GameObject SE_VampDeath2;


    public GameObject BigLootDump;
    
    private int myArmour = 1;
    private EnemyDamageReductionSP edr;
    private int bossDamage = 16;


    private bool isTransition;

    private bool hasDoneFirstTransition;
    private bool hasDoneSecondTransition;
    private GameObject Player;

    private float transitionCDTimer;
    private float CDTimer;
    private float cooldown = 3.0f;

    public Slider bossHealthSlider;
    public Image Fill; 

    // Use this for initialization
    void OnEnable()
    {
        Player = GameObject.Find("PLAYERBASE");
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

    void Start()
    {
        //assumption - GameManager object exists once in a scene, and contains exactly one EnemyDamageReductionSP script.
        UnityEngine.Object[] temp = FindObjectsOfType(typeof(EnemyDamageReductionSP));
        edr = temp[0] as EnemyDamageReductionSP;
    }

    void Update ()
    {
        if (!HeroControllerSP.isPaused && !hasDied) 
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
                    Vector3 offset1 = new Vector3(teleSpot1.transform.position.x + 0.5f, teleSpot1.transform.position.y, teleSpot1.transform.position.z + 0.5f);
                    Vector3 offset2 = new Vector3(teleSpot1.transform.position.x - 0.5f, teleSpot1.transform.position.y, teleSpot1.transform.position.z - 0.5f);
                    Vector3 offset3 = new Vector3(teleSpot1.transform.position.x + 0.2f, teleSpot1.transform.position.y, teleSpot1.transform.position.z - 0.2f);
                    Instantiate(SummonedVampire, teleSpot1.transform.position, teleSpot1.transform.rotation);
                    Instantiate(SummonedVampire, offset1, teleSpot1.transform.rotation);
                    Instantiate(SummonedVampire, offset2, teleSpot1.transform.rotation);                    
                    Instantiate(SummonedVampire, offset3, teleSpot1.transform.rotation);
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
                    agent.isStopped = false;
                }

            }
        }
	}

    void moveToPlayer()
    {
        Vector3 targetPostition = new Vector3(target.transform.position.x,
                                       target.transform.position.y,
                                       target.transform.position.z);
        spawnLoc.transform.LookAt(targetPostition);        
        this.transform.LookAt(targetPostition);
        spawnLoc.transform.LookAt(targetPostition);

        int attackChance = Random.Range(1, 170);
        if (attackChance >= 169 && CDTimer < 0.01f)
        {
           // anim.SetTrigger("isAttacking");
            Instantiate(coldObj, spawnLoc.transform.position, spawnLoc.transform.rotation);
            CDTimer = 0.3f;

        }
        if ((distanceX > -2.5 && distanceX < 2.5) && (distanceZ > -2.5 && distanceZ < 2.5) && (distanceY > -3 && distanceY < 3) )
        {
            if (CDTimer < 0.01f)
            {
                anim.SetBool("IsAggroed", false);
                agent.isStopped = true;
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
            agent.isStopped = false;
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
            agent.isStopped = false;            
        }        
    }


    public void OnParticleCollision(GameObject particle)
    {
        if (particle.gameObject.name.Contains("FlameThrowerParticle"))
        {
            int damageTaken = 0;
            damageTaken = edr.particleDmgTaken(particle, "NORMAL", myArmour);
            bossHealth -= damageTaken;

            deathAndTransition();
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        //case when your player projectile hits the boss
        if (other.gameObject.name.Contains("Shot") && !hasDied)
        {
            //During transition, if the boss is hit by any weapon he takes an additional one damage.
            if (isTransition)
            {
                bossHealth -= 1;
            }

            int damageTaken = 0;

            damageTaken = edr.collisionDmgTaken(other, "NORMAL", myArmour);
            bossHealth -= damageTaken;
            
            
            //consume playershot
            Destroy(other.gameObject);

            //code to see if health delta brings current boss health to a transition or death...
            deathAndTransition();

        }
    }


    public void deathAndTransition()
    {
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

        if (bossHealth <= 0 && !hasDied)
        {
            GAMEMANAGERSP.numScore += 50;
            levelWellSelector.SetActive(true);
            source.PlayOneShot(OnDeathSound, 1.0f);
            anim.SetTrigger("isDying");
            agent.enabled = false;
            SelfDestructSP sd = this.gameObject.AddComponent<SelfDestructSP>() as SelfDestructSP;
            sd.lifeSpan = 2.9f;

            Instantiate(SE_WizDeath3, teleSpot1.transform.position, teleSpot1.transform.rotation);
            Instantiate(SE_VampDeath2, teleSpot1.transform.position, teleSpot1.transform.rotation);

            hasDied = true;
            SafetyDanceGridSP.isActive = false;

            bossHealthSlider.gameObject.SetActive(false);

            Instantiate(BigLootDump, this.transform.position, this.transform.rotation);

        }
    }
    private void DamageReduction()
    {
        DamageReductionScript dmgReduct = new DamageReductionScript();
        //when attacking player, enemy may take thorns damage
        int thornsDmgReturned = dmgReduct.DamageReduction(bossDamage);
        if (thornsDmgReturned > 0)
        {
            Quaternion PlainRot = new Quaternion(90, 0, 0, 90);
            Vector3 plainHeight = new Vector3(Player.transform.position.x, Player.transform.position.y + 0.5f, Player.transform.position.z);
            GameObject thorns = Instantiate(SE_Thorns, plainHeight, PlainRot) as GameObject;
            thorns.transform.SetParent(Player.transform);
            bossHealth -= thornsDmgReturned;
        }
        if (bossHealth <= 0 && !hasDied)
        {
            deathAndTransition();
        }
    }
}
