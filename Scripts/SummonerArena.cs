/*
Summoner logic
Have multiple new Bools for behaviour states. Have methods for each state to make it easy. if state - > call state's method

when aggroed, walk towards player

when in range, roll dice (use cooldown):
- attack
- summon enemy
- special attack

IF health lower than threshold, run away. When far enough, begin rapid healing state until dead or full health. When at full, resume.



*/


//idle and OoC
//Aggroed and moving to target
//aggroed and in range to attack 
//fleeing 
//flee far enough and is healing

using UnityEngine;
using System.Collections;

public class SummonerArena : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;

    public int bossHealth = 60;
    public int maxBossHealth = 60;

    //gamestates correspond to behaviour methods. Only one can be active at a time.
    public bool gameState_OoC;
    public bool gameState_MovingToTarget;
    public bool gameState_InRangeAttacking;
    public bool gameState_Fleeing;
    public bool gameState_Healing;


    //get location of character!
    public GameObject target;
    public GameObject fleeDestination;
    public bool isSummoned;

    public float distanceX;
    public float distanceZ;
    public float distanceY;
    private float cooldown = 4.0f;
    private float summonCooldown = 6.5f; // different from attack cd
    private int maxNumEnemies = 10;
    private int currNumEnemies = 0;

    private float cooldownTimer;
    private float summonCooldownTimer;
    private int myArmour = 1;
    private EnemyDamageReductionSP edr;


    bool shouldPlayAggroEffect = false;
    Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);

    public GameObject CasterSpecEffect;
    public GameObject CasterSpawnLoc;
    public GameObject objToSpawn;
    public GameObject objToSpawnIce;
    public GameObject AggroSpecEffect;
    public ParticleSystem IceEffect; //The  shader displayed when casting!
    public GameObject vampireEnemy;
    public GameObject SE_Heal;


    //LOOT
    public GameObject RedBattery;
    public GameObject GreenBattery;
    public GameObject SuperBattery;
    public GameObject Gear;
    public GameObject GoldenGear;

    public GameObject AxeDrop;
    public GameObject ShieldDrop;
    public GameObject GunDrop;
    public GameObject GreatswordDrop;
    public GameObject BoosterDrop;

    public GameObject axeLightning;
    public GameObject gunMulti;
    public GameObject gsFire;
    public GameObject shieldIce;
    public GameObject boosterArcane;

    //skull drops for arena only
    public GameObject blueSkullDrop;
    public GameObject redSkullDrop;
    public GameObject purpleSkullDrop;
    public GameObject bronzeSkullDrop;

    public bool isAggroed;
    private bool hasDied;

    void OnEnable()
    {
        hasDied = false;
        gameState_OoC = true;
        gameState_MovingToTarget = false;
        gameState_InRangeAttacking = false;
        gameState_Fleeing = false;
        gameState_Healing = false;


        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.Find("BatteryBot");

        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        anim.SetBool("IsAggroed", false);
        isAggroed = false;
        shouldPlayAggroEffect = true;
        IceEffect.gameObject.SetActive(false);

        if (isSummoned)
        {
            //CDC Hardcoded; but this'll only occur in special circumstances during final boss battle anyways
            fleeDestination = GameObject.Find("SummonedSummonerFleeDest");
            if (fleeDestination == null) { Debug.Log("ERROR - flee dest not found."); }

            //BUG - if you summon an enemy you'll need to intialize these values at start time, or they're zero, and that means they're in range to hit you :p
            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;
            distanceY = this.transform.position.y - target.transform.position.y;

            isAggroed = true;
            anim.SetBool("IsAggroed", true);
        }
        else
        {
            isAggroed = false;
        }
    }


    void Start()
    {
        //assumption - GameManager object exists once in a scene, and contains exactly one EnemyDamageReductionSP script.
        UnityEngine.Object[] temp = FindObjectsOfType(typeof(EnemyDamageReductionSP));
        edr = temp[0] as EnemyDamageReductionSP;
    }

    void Update()
    {
        if (!hasDied)
        {
            cooldownTimer -= 0.03f;
            summonCooldownTimer -= 0.03f;

            StartCoroutine("checkAggro");

            if (isAggroed)
            {
                if (!HeroController.isPaused)
                {
                    if (bossHealth > 10 && (distanceX < -8.0 || distanceX > 8.0) && (distanceZ < -8.0 || distanceZ > 8.0) && !gameState_Fleeing && !gameState_Healing)
                    {
                        Behaviour_MovingToTarget();
                    }
                    else if (bossHealth > 10 && !gameState_Fleeing && !gameState_Healing)
                    {
                        Behaviour_InRangeAttacking();
                    }
                    else
                    {
                        gameState_Fleeing = true;
                        gameState_Healing = false; gameState_InRangeAttacking = false; gameState_MovingToTarget = false;
                    }


                    if (gameState_Fleeing)
                    {
                        Behaviour_Fleeing();
                    }

                    if (gameState_Healing)
                    {
                        Behaviour_Healing();
                    }
                }
            }

        }
    }

    /// <summary>
    /// This method ensures there will be no more than the max number of summons in existence.
    /// Ownership doesn't matter; one summoner boss can have all or less. This is for performance
    /// it also doesn't matter due to the fact that more than 1 summoner boss engaged simulatenously is not realistic in gameplay
    /// </summary>
    private int checkNumSummonedVampiresInExistence()
    {
        int numVampiresInExistence = 0;
        //get array of gameobjects(vampires that were summoned and still in existence)
        foreach (GameObject vampire in GameObject.FindObjectsOfType<GameObject>())
        {
            if (vampire.name.Contains("VampireSummoned"))
            {
                numVampiresInExistence += 1;
            }
        }
        return numVampiresInExistence;
    }

    private IEnumerator checkAggro()
    {

        if (!gameState_Fleeing && !gameState_Healing)
        {

            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;

            if ((distanceX > -15 && distanceX < 15) && (distanceZ > -15 && distanceZ < 15))
            {
                distanceY = this.transform.position.y - target.transform.position.y; //Only check y if we have to - optimization
                if (distanceY > -10.0f && distanceY < 10.0f)
                {
                    isAggroed = true;
                    anim.SetBool("IsAggroed", true);
                    agent.Resume();
                    if (shouldPlayAggroEffect)
                    {
                        Instantiate(AggroSpecEffect, this.transform.position, aggroRot);
                        shouldPlayAggroEffect = false;
                    }
                }
            }
        }
        yield return new WaitForSeconds(2);
    }



    void Behaviour_MovingToTarget()
    {
        //make the spawnloc aim at the player - so you won't be safe up high either!
        Vector3 targetPostition = new Vector3(target.transform.position.x,
                                       target.transform.position.y + 0.9f,
                                       target.transform.position.z);
        CasterSpawnLoc.transform.LookAt(targetPostition);
        this.transform.LookAt(targetPostition);

        //let the caster face the player at all times



        agent.SetDestination(target.transform.position);

        anim.SetBool("IsNotInRange", true);

        if (cooldownTimer < 0.01f)
        {
            currNumEnemies = checkNumSummonedVampiresInExistence();
            if (currNumEnemies < maxNumEnemies)
            {
                //summon as she walks
                anim.SetTrigger("isSummoning");
                Vector3 offset1 = new Vector3(CasterSpawnLoc.transform.position.x + 0.5f, CasterSpawnLoc.transform.position.y, CasterSpawnLoc.transform.position.z - 0.5f);
                Vector3 offset2 = new Vector3(CasterSpawnLoc.transform.position.x + 0.5f, CasterSpawnLoc.transform.position.y, CasterSpawnLoc.transform.position.z - 0.5f);
                Instantiate(vampireEnemy, CasterSpawnLoc.transform.position, CasterSpawnLoc.transform.rotation);
                Instantiate(vampireEnemy, offset1, CasterSpawnLoc.transform.rotation);
                Instantiate(vampireEnemy, offset2, CasterSpawnLoc.transform.rotation);
                cooldownTimer = summonCooldown;
            }

        }


    }

    void Behaviour_InRangeAttacking()
    {
        Vector3 targetPostition = new Vector3(target.transform.position.x,
                                       target.transform.position.y,
                                       target.transform.position.z);
        this.transform.LookAt(targetPostition);
        CasterSpawnLoc.transform.LookAt(targetPostition);

        if ((distanceX > -8.0 && distanceX < 8.0) && (distanceZ > -8.0 && distanceZ < 8.0) && cooldownTimer < 0.01f)
        {

            gameState_InRangeAttacking = true;

            int randomNum = Random.Range(1, 20);

            //find total number of enemies
            // ......


            if (randomNum <= 12 && cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isAttacking");
                //No cooldown... ice shots will be fast to make this boss more frightening and random
                Instantiate(objToSpawnIce, CasterSpawnLoc.transform.position, CasterSpawnLoc.transform.rotation);
                Instantiate(CasterSpecEffect, this.transform.position, this.transform.rotation);
                cooldownTimer = 0.3f;

            }
            else if (randomNum > 12 && randomNum < 15 && cooldownTimer < 0.01f)
            {
                currNumEnemies = checkNumSummonedVampiresInExistence();
                if (currNumEnemies < maxNumEnemies)
                {
                    //summon
                    anim.SetTrigger("isSummoning");
                    Vector3 offset1 = new Vector3(CasterSpawnLoc.transform.position.x + 0.5f, CasterSpawnLoc.transform.position.y, CasterSpawnLoc.transform.position.z - 0.5f);
                    Vector3 offset2 = new Vector3(CasterSpawnLoc.transform.position.x + 0.5f, CasterSpawnLoc.transform.position.y, CasterSpawnLoc.transform.position.z - 0.5f);
                    Instantiate(vampireEnemy, CasterSpawnLoc.transform.position, CasterSpawnLoc.transform.rotation);
                    Instantiate(vampireEnemy, offset1, CasterSpawnLoc.transform.rotation);
                    Instantiate(vampireEnemy, offset2, CasterSpawnLoc.transform.rotation);
                    cooldownTimer = summonCooldown;
                }

            }

            else if (cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isAttacking");
                //we are in range. Start shooting
                float offset = Random.Range(-0.03f, 0.01f);
                Quaternion spawnRot = new Quaternion
                    (CasterSpawnLoc.transform.rotation.x + offset,
                    CasterSpawnLoc.transform.rotation.y + offset,
                    CasterSpawnLoc.transform.rotation.z + offset,
                    CasterSpawnLoc.transform.rotation.w);
                Instantiate(objToSpawn, CasterSpawnLoc.transform.position, spawnRot);
                cooldownTimer = cooldown;
                Instantiate(CasterSpecEffect, this.transform.position, this.transform.rotation);
            }


        }
        // when you're in range but on cooldown
        else if ((distanceX > -8.0 && distanceX < 8.0) && (distanceZ > -8.0 && distanceZ < 8.0) && cooldownTimer > 0.01f)
        {
            IceEffect.gameObject.SetActive(true);
            //anim.SetTrigger("isIdle");
            anim.SetBool("IsNotInRange", false);
        }
        else
        {
            if (agent.isActiveAndEnabled)
            {
                anim.SetBool("IsNotInRange", true);
                IceEffect.gameObject.SetActive(false);
                agent.SetDestination(target.transform.position);
            }

        }
    }

    void Behaviour_Fleeing()
    {
        //move away from player
        agent.speed = 6.5f;
        anim.SetBool("IsNotInRange", true);
        agent.SetDestination(fleeDestination.transform.position);

        distanceX = this.transform.position.x - target.transform.position.x;
        distanceZ = this.transform.position.z - target.transform.position.z;
        distanceY = this.transform.position.y - target.transform.position.y;

        if ((distanceX < -17.0 || distanceX > 17.0) || (distanceZ < -17.0 || distanceZ > 17.0))
        {
            gameState_Healing = true;
            gameState_Fleeing = false; gameState_InRangeAttacking = false; gameState_MovingToTarget = false;

        }
    }

    void Behaviour_Healing()
    {
        agent.speed = 3.0f;
        anim.SetBool("IsNotInRange", false);

        agent.ResetPath();


        //boss health is higher than 50 at the start but she just heals to 50.
        if (bossHealth <= 50 && cooldownTimer < 0.01f)
        {
            Instantiate(SE_Heal, this.transform.position, this.transform.rotation);
            anim.SetTrigger("isHealing");
            bossHealth += 9;
            Debug.Log("Summoner boss health is " + bossHealth);
            cooldownTimer = 5.5f;

        }
        else if (bossHealth > 50)
        {
            anim.SetBool("IsNotInRange", true);

            gameState_MovingToTarget = true;
            gameState_Healing = false; gameState_InRangeAttacking = false; gameState_Fleeing = false;
        }

    }

    public void OnParticleCollision(GameObject particle)
    {
        if (particle.gameObject.name.Contains("FlameThrowerParticle"))
        {
            int damageTaken = 0;
            if (isSummoned)
            {
                damageTaken = edr.particleDmgTaken(particle, "ETHEREAL", myArmour);
            }
            else
            {
                damageTaken = edr.particleDmgTaken(particle, "NORMAL", myArmour);
            }

            bossHealth -= damageTaken;
            if (bossHealth <= 0 && !hasDied)
            {
                death();
            }
        }
    }


    public void OnCollisionEnter(Collision other)
    {
        //case when your player projectile hits the caster
        if (other.gameObject.name.Contains("Shot") && !hasDied)
        {
            isAggroed = true;
            int damageTaken = 0;
            if (isSummoned)
            {
                damageTaken = edr.collisionDmgTaken(other, "ETHEREAL", myArmour);
            }
            else
            {
                damageTaken = edr.collisionDmgTaken(other, "NORMAL", myArmour);
            }
            bossHealth -= damageTaken;
            //consume playershot
            Destroy(other.gameObject);

            if (bossHealth <= 0 && !hasDied)
            {
                death();
            }
        }
    }


    public void death()
    {
        {
            GAMEMANAGERSP.numScore += 30;
            IceEffect.gameObject.SetActive(false);
            anim.SetTrigger("isDying");
            agent.enabled = false;
            SelfDestructSP sd = this.gameObject.AddComponent<SelfDestructSP>() as SelfDestructSP;
            sd.lifeSpan = 2.0f;
            hasDied = true;

            //possibly spawn some loot!


            int randomNum = Random.Range(1, 75);

            if (randomNum <= 6)
            {
                int posOffset = Random.Range(1, 5);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(RedBattery, spawnPos, spawnRot);
                Instantiate(blueSkullDrop, spawnPos, spawnRot);
            }
            else if (randomNum > 6 && randomNum <= 12)
            {
                int posOffset = Random.Range(1, 5);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(GreenBattery, spawnPos, spawnRot);
                Instantiate(purpleSkullDrop, spawnPos, spawnRot);
            }
            else if (randomNum == 13)
            {
                int posOffset = Random.Range(1, 5);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(SuperBattery, spawnPos, spawnRot);
                Instantiate(redSkullDrop, spawnPos, spawnRot);
            }
            else if (randomNum > 13 && randomNum <= 15)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(AxeDrop, spawnPos, spawnRot);
                Instantiate(redSkullDrop, spawnPos, spawnRot);
            }
            else if (randomNum > 15 && randomNum <= 17)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(ShieldDrop, spawnPos, spawnRot);
                Instantiate(redSkullDrop, spawnPos, spawnRot);
            }
            else if (randomNum > 17 && randomNum <= 19)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(GunDrop, spawnPos, spawnRot);
                Instantiate(blueSkullDrop, spawnPos, spawnRot);
            }

            else if (randomNum == 20)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(GoldenGear, spawnPos, spawnRot);
                Instantiate(bronzeSkullDrop, spawnPos, spawnRot);
            }

            else if (randomNum > 20 && randomNum <= 30)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(Gear, spawnPos, spawnRot);
            }

            else if (randomNum > 30 && randomNum <= 34)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(BoosterDrop, spawnPos, spawnRot);
                Instantiate(blueSkullDrop, spawnPos, spawnRot);
            }
            else if (randomNum > 34 && randomNum <= 36)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(axeLightning, spawnPos, spawnRot);
            }
            else if (randomNum > 36 && randomNum <= 38)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(gunMulti, spawnPos, spawnRot);
            }
            else if (randomNum > 38 && randomNum <= 40)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(gsFire, spawnPos, spawnRot);
            }
            else if (randomNum > 40 && randomNum <= 42)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(shieldIce, spawnPos, spawnRot);
                Instantiate(purpleSkullDrop, spawnPos, spawnRot);
            }
            else if (randomNum > 42 && randomNum <= 44)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(boosterArcane, spawnPos, spawnRot);
            }
            else
            {
                Instantiate(Gear, this.transform.position, this.transform.rotation);
                Instantiate(GreenBattery, this.transform.position, this.transform.rotation);
                Instantiate(GreenBattery, this.transform.position, this.transform.rotation);
                Instantiate(GreenBattery, this.transform.position, this.transform.rotation);
                Instantiate(RedBattery, this.transform.position, this.transform.rotation);
            }


        }
    }



}