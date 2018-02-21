using UnityEngine;
using System.Collections;

public class CasterTurretBossSP : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    //get location of character!
    public GameObject target;
    public float distanceX;
    public float distanceZ;
    public float distanceY;
    private float cooldown = 7.0f; //slightly longer cooldown than normal caster
    private float cooldownTimer;
    public int bossHealth = 1; //greatly less life than most other enemies - meant to be a sniper whose defense comes from hiding at a distance
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
        //case when your player projectile hits the caster
        if (other.gameObject.name.Contains("Shot") && !hasDied)
        {
            isAggroed = true;
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
        GAMEMANAGERSP.numScore += 2;
        FlameEffect.gameObject.SetActive(false);
        anim.SetTrigger("isDying");
        agent.enabled = false;
        SelfDestructSP sd = this.gameObject.AddComponent<SelfDestructSP>() as SelfDestructSP;
        sd.lifeSpan = 1.8f;
        hasDied = true;
        playDeathSE();

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
            StartCoroutine("checkAggro");

            if (isAggroed)
            {
                if (!HeroControllerSP.isPaused)
                {
                    attackPlayer();
                }
            }
        }

    }

    private IEnumerator checkAggro()
    {
        
        distanceX = this.transform.position.x - target.transform.position.x;
        distanceZ = this.transform.position.z - target.transform.position.z;
        distanceY = this.transform.position.y - target.transform.position.y;
        if ((distanceX > -30 && distanceX < 30) && (distanceZ > -30 && distanceZ < 30) )
        {            
            if (distanceY > -20.0f && distanceY < 20.0f)
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
        //if you have aggroed, then ran away, and you're too far she gives up
        else if ((distanceX < -45 || distanceX > 45) || (distanceZ < -45 || distanceZ > 45) || (distanceY < -10 || distanceY > 10))
        {
            FlameEffect.gameObject.SetActive(false);
            isAggroed = false;
            anim.SetBool("IsAggroed", false);
            if (agent.isActiveAndEnabled)
            {
                shouldPlayAggroEffect = true;
                agent.Stop();
            }
        }
        yield return new WaitForSeconds(3.8f);
    }

    void attackPlayer()
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

        //significantly larger range than normal caster
        if ((distanceX > -30.0 && distanceX < 30.0) && (distanceZ > -30.0 && distanceZ < 30.0) && cooldownTimer < 0.01f)
        {
            FlameEffect.gameObject.SetActive(true);
            // agent.SetDestination(target.transform.position);
            anim.SetTrigger("isAttacking");
            //we are in range. Start shooting
            Debug.Log("Caster is readying a fireball!");
            
            float offset = Random.Range(-0.03f, 0.01f);
            Quaternion spawnRot = new Quaternion
                (CasterSpawnLoc.transform.rotation.x + offset, 
                CasterSpawnLoc.transform.rotation.y + offset,
                CasterSpawnLoc.transform.rotation.z + offset, 
                CasterSpawnLoc.transform.rotation.w );
            Instantiate(objToSpawn, CasterSpawnLoc.transform.position, spawnRot);

            cooldownTimer = cooldown;
            Instantiate(CasterSpecEffect, this.transform.position, this.transform.rotation);
                  
        }
        // when you're in range but on cooldown
        else if ((distanceX > -30.0 && distanceX < 30.0) && (distanceZ > -30.0 && distanceZ < 30.0) && cooldownTimer > 0.01f)
        {
            FlameEffect.gameObject.SetActive(true);
            //anim.SetTrigger("isIdle");
            anim.SetBool("IsNotInRange", false);
        }
        else
        {
            if (agent.isActiveAndEnabled)
            {
                //anim.SetBool("IsNotInRange", true);
                FlameEffect.gameObject.SetActive(false);
                //agent.SetDestination(target.transform.position); 
            }

        }

    }

}