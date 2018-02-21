
using UnityEngine;
using System.Collections;

public class WizardFireFollowSP : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    
    public int bossHealth = 12; //low health due to his massive dps
    private float shieldHeight;

    //gamestates correspond to behaviour methods. Only one can be active at a time.
    public bool gameState_OoC;
    public bool gameState_MovingToTarget;
    public bool gameState_InRangeAttacking;
    private float waitTime = 1.5f;

    public bool isDroppingSkull = false;
    public GameObject redSkull;

    //get location of character!
    public GameObject target;

    public float distanceX;
    public float distanceZ;
    public float distanceY;
    private float cooldown = 3.5f;
    private float FireBlastCooldown = 5.0f; // different from attack cd
    private float FireProjCooldown = 8.0f;
    private float shieldCooldown = 8.0f;

    private float cooldownTimer;
    private float IceBlastCooldownTimer;

    private int myArmour = 1;
    private EnemyDamageReductionSP edr;

    bool shouldPlayAggroEffect = false;
    Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);

    public GameObject SE_Heal;
    public GameObject CasterSpawnLoc;
    public GameObject objToSpawn;
    public GameObject objToSpawnFire;
    public GameObject AggroSpecEffect;
    public GameObject FireBlastMulti;
    public GameObject FireShield;
    public GameObject SE_Hit_Poison;
    public GameObject SE_WizDeath1;
    public GameObject SE_WizDeath2;
    public GameObject SE_WizDeath3;


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
    public GameObject gunFireDrop;
    public GameObject gsFire;
    public GameObject shieldIce;
    public GameObject boosterArcane;

    public bool isAggroed;
    private bool hasDied;

    void OnEnable()
    {

        gameState_OoC = true;
        gameState_MovingToTarget = false;
        gameState_InRangeAttacking = false;


        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.Find("BatteryBot");

        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        anim.SetBool("IsAggroed", false);
        isAggroed = false;
        shouldPlayAggroEffect = true;

        var myCollider = this.gameObject.GetComponent<Collider>();
        shieldHeight = (myCollider.bounds.size.y) / 2;
        hasDied = false;
    }

    void Start()
    {
        //assumption - GameManager object exists once in a scene, and contains exactly one EnemyDamageReductionSP script.
        UnityEngine.Object[] temp = FindObjectsOfType(typeof(EnemyDamageReductionSP));
        edr = temp[0] as EnemyDamageReductionSP;
    }


    void Update()
    {
        if (!hasDied && !HeroControllerSP.isPaused)
        {
            
            cooldownTimer -= 0.03f;
            IceBlastCooldownTimer -= 0.03f;

           
            StartCoroutine("checkAggro");

            if (isAggroed)
            {

                if ((distanceX < -8.0 || distanceX > 8.0) && (distanceZ < -8.0 || distanceZ > 8.0))
                {
                    Behaviour_MovingToTarget();
                }
                else
                {
                    Behaviour_InRangeAttacking();
                }


            }

        }
    }

    private IEnumerator checkAggro()
    {
        if (!hasDied)
        {
            yield return new WaitForSeconds(waitTime);
            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;
            yield return new WaitForSeconds(waitTime);
            if ((distanceX > -20 && distanceX < 20) && (distanceZ > -20 && distanceZ < 20) )
            {
                distanceY = this.transform.position.y - target.transform.position.y; //Only check y if we have to - optimization
                if (distanceY > -8.0f && distanceY < 8.0f)
                {
                    isAggroed = true;
                    anim.SetBool("IsAggroed", true);
                    //agent.Resume();
                    if (shouldPlayAggroEffect)
                    {
                        Instantiate(AggroSpecEffect, this.transform.position, aggroRot);
                        shouldPlayAggroEffect = false;
                    }
                }
            }
        }
    }


    void Behaviour_MovingToTarget()
    {
        if (!hasDied)
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

            //make an ice shield as you walk over
            if (cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isHealing");
                Vector3 ShieldPos = new Vector3(this.transform.position.x, this.transform.position.y + shieldHeight, this.transform.position.z);
                GameObject myShield = Instantiate(FireShield, ShieldPos, this.transform.rotation) as GameObject;
                myShield.transform.parent = this.gameObject.transform;
                cooldownTimer = shieldCooldown;
            }
        }

    }

    void Behaviour_InRangeAttacking()
    {
        if (!hasDied)
        {


            Vector3 targetPostition = new Vector3(target.transform.position.x,
                                           target.transform.position.y,
                                           target.transform.position.z);
            this.transform.LookAt(targetPostition);
            CasterSpawnLoc.transform.LookAt(targetPostition);

            if ((distanceX > -8.0 && distanceX < 8.0) && (distanceZ > -8.0 && distanceZ < 8.0) && cooldownTimer < 0.01f)
            {

                gameState_InRangeAttacking = true;

                int randomNum = Random.Range(1, 25);

                if (randomNum <= 8 && cooldownTimer < 0.01f)
                {
                    //set of 3 fireballs like the greatsword, except these EXPLODE
                    anim.SetTrigger("isAttacking");
                    float offset = Random.Range(-0.03f, 0.01f);
                    Quaternion spawnRot = new Quaternion
                        (CasterSpawnLoc.transform.rotation.x + offset,
                        CasterSpawnLoc.transform.rotation.y + offset,
                        CasterSpawnLoc.transform.rotation.z + offset,
                        CasterSpawnLoc.transform.rotation.w);
                    Instantiate(objToSpawnFire, CasterSpawnLoc.transform.position, spawnRot);
                    cooldownTimer = FireProjCooldown;

                }
                else if (randomNum > 8 && randomNum < 14 && cooldownTimer < 0.01f)
                {
                    //fire shield on self
                    anim.SetTrigger("isHealing");
                    Vector3 ShieldPos = new Vector3(this.transform.position.x, this.transform.position.y + shieldHeight, this.transform.position.z);
                    GameObject myShield = Instantiate(FireShield, ShieldPos, this.transform.rotation) as GameObject;
                    myShield.transform.parent = this.gameObject.transform;
                    cooldownTimer = shieldCooldown;
                }
                else if (randomNum >= 14 && randomNum < 21 && cooldownTimer < 0.01f)
                {
                    //fire blast, many projectiles
                    //slightly inaccurate
                    int randomOffset1 = Random.Range(0, 2);
                    int randomOffset2 = Random.Range(0, 2);
                    Vector3 blastPos = new Vector3(target.transform.position.x + randomOffset1, target.transform.position.y, target.transform.position.z + randomOffset2);
                    anim.SetTrigger("isSummoning");
                    Instantiate(FireBlastMulti, blastPos, this.transform.rotation);
                    cooldownTimer = FireBlastCooldown;
                }

                else if (cooldownTimer < 0.01f)
                {
                    //basic shot
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
                }


            }
            // when you're in range but on cooldown
            else if ((distanceX > -8.0 && distanceX < 8.0) && (distanceZ > -8.0 && distanceZ < 8.0) && cooldownTimer > 0.01f)
            {
                //anim.SetTrigger("isIdle");
                anim.SetBool("IsNotInRange", false);
            }
            else
            {
                if (agent.isActiveAndEnabled)
                {
                    anim.SetBool("IsNotInRange", true);
                    agent.SetDestination(target.transform.position);
                }

            }
        }
    }

    public void OnParticleCollision(GameObject particle)
    {
        if (particle.gameObject.name.Contains("FlameThrowerParticle"))
        {
            int damageTaken = 0;
            damageTaken = edr.particleDmgTaken(particle, "FIRE", myArmour);
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

            damageTaken = edr.collisionDmgTaken(other, "FIRE", myArmour);
            bossHealth -= damageTaken;
            //consume playershot
            Destroy(other.gameObject);

            if (bossHealth <= 0 && !hasDied)
            {
                death();
            }
        }
    }


    private void playDeathSE()
    {
        int deathSound = UnityEngine.Random.Range(1, 25);
        if (deathSound < 4)
        {
            Instantiate(SE_WizDeath1, this.transform.position, this.transform.rotation);
        }
        else if (deathSound >= 4 && deathSound < 8)
        {
            Instantiate(SE_WizDeath2, this.transform.position, this.transform.rotation);
        }
        else if (deathSound >= 8 && deathSound < 12)
        {
            Instantiate(SE_WizDeath3, this.transform.position, this.transform.rotation);
        }
        //else - don't play sound effect
    }
    public void death()
    {

        GAMEMANAGERSP.numScore += 15;
        agent.isStopped = true;
        agent.enabled = false;
        anim.SetTrigger("isDying");
        SelfDestructSP sd = this.gameObject.AddComponent<SelfDestructSP>() as SelfDestructSP;
        sd.lifeSpan = 1.9f;
        playDeathSE();

        hasDied = true;
        foreach (GameObject wizShield in GameObject.FindObjectsOfType<GameObject>())
        {
            if (wizShield.name.Contains("SE_WizShield"))
            {
                Destroy(wizShield);
            }
        }
        agent.enabled = false;
        //possibly spawn some loot!

        int randomNum = Random.Range(1, 60);

        if (randomNum <= 6)
        {
            int posOffset = Random.Range(1, 5);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(RedBattery, spawnPos, spawnRot);
        }
        else if (randomNum > 6 && randomNum <= 12)
        {
            int posOffset = Random.Range(1, 5);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(GreenBattery, spawnPos, spawnRot);
        }
        else if (randomNum == 13)
        {
            int posOffset = Random.Range(1, 5);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(SuperBattery, spawnPos, spawnRot);
        }
        else if (randomNum > 13 && randomNum <= 15)
        {
            int posOffset = Random.Range(1, 3);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(AxeDrop, spawnPos, spawnRot);
        }
        else if (randomNum > 15 && randomNum <= 17)
        {
            int posOffset = Random.Range(1, 3);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(ShieldDrop, spawnPos, spawnRot);
        }
        else if (randomNum > 17 && randomNum <= 19)
        {
            int posOffset = Random.Range(1, 3);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(GunDrop, spawnPos, spawnRot);
        }

        else if (randomNum == 20)
        {
            int posOffset = Random.Range(1, 3);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(GoldenGear, spawnPos, spawnRot);
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
        else if (randomNum > 44 && randomNum <= 49)
        {
            int posOffset = Random.Range(1, 3);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(gunFireDrop, spawnPos, spawnRot);

        }
        else
        {
            Instantiate(Gear, this.transform.position, this.transform.rotation);
            Instantiate(GreenBattery, this.transform.position, this.transform.rotation);
            Instantiate(GreenBattery, this.transform.position, this.transform.rotation);
            Instantiate(GreenBattery, this.transform.position, this.transform.rotation);
            Instantiate(RedBattery, this.transform.position, this.transform.rotation);
        }

        //guaranteed skull key drop if the boolean is set to true!
        if (isDroppingSkull)
        {
            Vector3 SkullPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
            GameObject skullDrop = Instantiate(redSkull, SkullPos, this.transform.rotation) as GameObject;
        }
    }
   

}