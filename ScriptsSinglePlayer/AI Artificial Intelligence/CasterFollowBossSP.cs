using UnityEngine;
using System.Collections;

public class CasterFollowBossSP : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    //get location of character!
    public GameObject target;
    public float distanceX;
    public float distanceZ;
    public float distanceY;
    private float cooldown = 5.0f;
    private float cooldownTimer;
    public int bossHealth = 5;
    private float waitTime = 1.5f;
    bool shouldPlayAggroEffect = false;
    Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);

    private int myArmour = 0;
    private EnemyDamageReductionSP edr;


    public GameObject CasterSpecEffect;
    public GameObject CasterSpawnLoc;
    public GameObject objToSpawn;
    public GameObject GunSpecEffect;
    public GameObject gunSpecBlueEffect;
    public GameObject BloodSpecEffect;
    public GameObject AggroSpecEffect;
    public ParticleSystem FlameEffect; //The fire shader displayed when casting!
    public GameObject SE_Hit_Poison;
    public GameObject SE_CasterDeath1;
    public GameObject SE_CasterDeath2;
    public GameObject SE_CasterDeath3;

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

    public bool isAggroed;
    private bool hasDied;

    void OnEnable()
    {
        hasDied = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.Find("BatteryBot");

        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        anim.SetBool("IsAggroed", false);
        isAggroed = false;
        shouldPlayAggroEffect = true;
        FlameEffect.gameObject.SetActive(false);
    }

    void Start()
    {
        //assumption - GameManager object exists once in a scene, and contains exactly one EnemyDamageReductionSP script.
        UnityEngine.Object[] temp = FindObjectsOfType(typeof(EnemyDamageReductionSP));
        edr = temp[0] as EnemyDamageReductionSP;
    }

    public void OnParticleCollision(GameObject particle)
    {
        if (particle.gameObject.name.Contains("FlameThrowerParticle"))
        {
            int damageTaken = 0;
            damageTaken = edr.particleDmgTaken(particle, "NORMAL", myArmour);
            bossHealth -= damageTaken;
            if (bossHealth <= 0 && !hasDied)
            {
                death();
            }
        }
    }


    public void OnCollisionEnter(Collision other)
    {
        //case when your player projectile hits the boss
        if (other.gameObject.name.Contains("Shot") && !hasDied)
        {
            isAggroed = true;
            int damageTaken = 0;

            damageTaken = edr.collisionDmgTaken(other, "NORMAL", myArmour);
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
            Instantiate(SE_CasterDeath1, this.transform.position, this.transform.rotation);
        }
        else if (deathSound >= 4 && deathSound < 8)
        {
            Instantiate(SE_CasterDeath2, this.transform.position, this.transform.rotation);
        }
        else if (deathSound >= 8 && deathSound < 12)
        {
            Instantiate(SE_CasterDeath3, this.transform.position, this.transform.rotation);
        }
        //else - don't play sound effect
    }



    public void death()
    {
        GAMEMANAGERSP.numScore += 5;
        FlameEffect.gameObject.SetActive(false);
        anim.SetTrigger("isDying");
        agent.enabled = false;
        SelfDestructSP sd = this.gameObject.AddComponent<SelfDestructSP>() as SelfDestructSP;
        sd.lifeSpan = 1.8f;
        playDeathSE();
        hasDied = true;
        //possibly spawn some loot!


        int randomNum = Random.Range(1, 28);

        if (randomNum <= 3)
        {
            int posOffset = Random.Range(1, 5);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(RedBattery, spawnPos, spawnRot);

        }
        else if (randomNum > 3 && randomNum <= 6)
        {
            int posOffset = Random.Range(1, 5);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(GreenBattery, spawnPos, spawnRot);

        }
        else if (randomNum == 7)
        {
            int posOffset = Random.Range(1, 5);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(SuperBattery, spawnPos, spawnRot);

        }
        else if (randomNum == 8)
        {
            int posOffset = Random.Range(1, 3);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(AxeDrop, spawnPos, spawnRot);

        }
        else if (randomNum == 9)
        {
            int posOffset = Random.Range(1, 3);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(ShieldDrop, spawnPos, spawnRot);

        }
        else if (randomNum == 10)
        {
            int posOffset = Random.Range(1, 3);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(GunDrop, spawnPos, spawnRot);

        }

        else if (randomNum == 11)
        {
            int posOffset = Random.Range(1, 3);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(GoldenGear, spawnPos, spawnRot);

        }

        else if (randomNum >= 12 && randomNum <= 15)
        {
            int posOffset = Random.Range(1, 3);
            int rotOffset1 = Random.Range(1, 180);
            int rotOffset2 = Random.Range(1, 180);
            Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
            Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
            Instantiate(Gear, spawnPos, spawnRot);

        }

        
    }


    void Update()
    {
        if (!hasDied)
        {
            
            StartCoroutine("checkAggro");

            if (isAggroed)
            {
                if (!HeroControllerSP.isPaused)
                {
                    moveToPlayer();
                }
            }
        }
    }

    private IEnumerator checkAggro()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(waitTime);
            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;
            distanceY = this.transform.position.y - target.transform.position.y; 
            yield return new WaitForSeconds(waitTime);
            if ((distanceX > -15 && distanceX < 15) && (distanceZ > -15 && distanceZ < 15) )
            {
                if (!hasDied)
                {                    
                    if (distanceY > -8.0f && distanceY < 8.0f)
                    {
                        isAggroed = true;
                        anim.SetBool("IsAggroed", true);
                        agent.isStopped = false;
                        if (shouldPlayAggroEffect)
                        {
                            Instantiate(AggroSpecEffect, this.transform.position, aggroRot);
                            shouldPlayAggroEffect = false;
                        }
                    }
                }
                
            }
            //if you have aggroed, then ran away, and you're too far she gives up
            else if ((distanceX < -45 || distanceX > 45) || (distanceZ < -45 || distanceZ > 45) || (distanceY < -10 || distanceY > 10))
            {
                isAggroed = false;
                anim.SetBool("IsAggroed", false);
                if (agent.isActiveAndEnabled)
                {
                    shouldPlayAggroEffect = true;
                    agent.isStopped = true;
                }
            }

        }
    }

    void moveToPlayer()
    {
        //make the spawnloc aim at the player - so you won't be safe up high either!
        Vector3 targetPostition = new Vector3(target.transform.position.x,
                                       target.transform.position.y + 0.9f,
                                       target.transform.position.z);
        CasterSpawnLoc.transform.LookAt(targetPostition);

        //let the caster face the player at all times
        //this.transform.LookAt(targetPostition);
        this.transform.LookAt(targetPostition);
        CasterSpawnLoc.transform.LookAt(targetPostition);


        cooldownTimer -= 0.03f;

        //Debug.Log (distance);


        if ((distanceX > -8.0 && distanceX < 8.0) && (distanceZ > -8.0 && distanceZ < 8.0) && cooldownTimer < 0.01f)
        {
            agent.SetDestination(target.transform.position);
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
        // when you're in range but on cooldown
        else if ((distanceX > -8.0 && distanceX < 8.0) && (distanceZ > -8.0 && distanceZ < 8.0) && cooldownTimer > 0.01f)
        {
            FlameEffect.gameObject.SetActive(true);
            //anim.SetTrigger("isIdle");
            anim.SetBool("IsNotInRange", false);
        }
        else
        {
            if (agent.isActiveAndEnabled)
            {
                anim.SetBool("IsNotInRange", true);
                FlameEffect.gameObject.SetActive(false);
                agent.SetDestination(target.transform.position);
            }

        }

    }

}