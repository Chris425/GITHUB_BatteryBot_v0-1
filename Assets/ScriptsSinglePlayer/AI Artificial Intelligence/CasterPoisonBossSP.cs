using UnityEngine;
using System.Collections;

public class CasterPoisonBossSP : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    //get location of character!
    public GameObject target;
    public float distanceX;
    public float distanceZ;
    public float distanceY;
    private float cooldown = 4.0f;
    private float cooldownTimer;
    public int bossHealth = 8;
    bool shouldPlayAggroEffect = false;
    Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);

    public GameObject CasterSpecEffect;
    public GameObject CasterSpawnLoc;
    public GameObject objToSpawn;
    public GameObject poisonWell;
    public GameObject DeathSpecEffect;
    public GameObject BloodSpecEffect;
    public GameObject AggroSpecEffect;
    public ParticleSystem FlameEffect; //The fire shader displayed when casting!

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

    public void OnCollisionEnter(Collision other)
    {
        //case when your player projectile hits the caster
        if (other.gameObject.name.Contains("Shot") && !hasDied)
        {
            isAggroed = true;
            if (other.gameObject.name.Contains("PlayerShot"))
            {
                if (other.gameObject.name.Contains("PlayerShotBlue"))
                {
                    if (HeroControllerSP.isSuperCharged == true)
                    { bossHealth -= 5; Instantiate(DeathSpecEffect, other.transform.position, this.transform.rotation); }
                    else { bossHealth -= 3; Instantiate(DeathSpecEffect, other.transform.position, this.transform.rotation); }
                }
                else
                {
                    if (HeroControllerSP.isSuperCharged == true)
                    { bossHealth -= 3; Instantiate(DeathSpecEffect, other.transform.position, this.transform.rotation); }
                    else { bossHealth -= 1; Instantiate(DeathSpecEffect, other.transform.position, this.transform.rotation); }
                }
            }
            else if (other.gameObject.name.Contains("GS_Shot"))
            {
                if (other.gameObject.name.Contains("FIRE"))
                {
                    bossHealth -= 5;
                    Instantiate(BloodSpecEffect, other.transform.position, this.transform.rotation);
                }
                else
                {
                    bossHealth -= 4;
                    Instantiate(BloodSpecEffect, other.transform.position, this.transform.rotation);
                }

            }
            else if (other.gameObject.name.Contains("Axe_Shot"))
            {
                if (other.gameObject.name.Contains("LIGHTNING"))
                {
                    int lightningVariance = Random.Range(1, 2);
                    bossHealth -= lightningVariance;
                    Instantiate(BloodSpecEffect, other.transform.position, this.transform.rotation);
                }
                else
                {
                    bossHealth -= 3;
                    Instantiate(BloodSpecEffect, other.transform.position, this.transform.rotation);
                }

            }
            //note that shield shot IS the ice special... shield normally shoots an axe shot (because reasons)
            else if (other.gameObject.name.Contains("Shield_Shot"))
            {
                bossHealth -= 4;
                Instantiate(DeathSpecEffect, other.transform.position, this.transform.rotation);
            }


            //consume playershot
            Destroy(other.gameObject);
            if (bossHealth <= 0 && !hasDied)
            {
                //fix to prevent this from occuring many times; 
                //turns out the gameobject isn't destroyed immediately so it can register many collisions...
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
                else if (randomNum == 7 )
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

                else if (randomNum >= 12 && randomNum <=15)
                {
                    int posOffset = Random.Range(1, 3);
                    int rotOffset1 = Random.Range(1, 180);
                    int rotOffset2 = Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(Gear, spawnPos, spawnRot);

                }

                Destroy(other.gameObject);
                Destroy(this.gameObject);

            }

        }
    }

    void Update()
    {
        if (!hasDied && !HeroControllerSP.isPaused)
        {
            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;
            distanceY = this.transform.position.y - target.transform.position.y;
            checkAggro();

            if (isAggroed)
            {
                moveToPlayer();
            }
        }
    }

    private void checkAggro()
    {
        if ((distanceX > -15 && distanceX < 15) && (distanceZ > -15 && distanceZ < 15) && (distanceY > -10 && distanceY < 10))
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
        //if you have aggroed, then ran away, and you're too far she gives up
        else if ((distanceX < -45 || distanceX > 45) || (distanceZ < -45 || distanceZ > 45) || (distanceY < -10 || distanceY > 10))
        {
            isAggroed = false;
            anim.SetBool("IsAggroed", false);
            if (agent.isActiveAndEnabled)
            {
                shouldPlayAggroEffect = true;
                agent.Stop();
            }
        }
    }

    void moveToPlayer()
    {
        //make the spawnloc aim at the player - so you won't be safe up high either!
        Vector3 targetPostition = new Vector3(target.transform.position.x,
                                       target.transform.position.y,
                                       target.transform.position.z);
        CasterSpawnLoc.transform.LookAt(targetPostition);

        //let the caster face the player at all times
        //this.transform.LookAt(targetPostition);
        this.transform.LookAt(targetPostition);
        CasterSpawnLoc.transform.LookAt(targetPostition);


        cooldownTimer -= 0.03f;

        //Debug.Log (distance);

        //when in range poison caster can do a bolt or a well, much like the wizard. Lacks the explosion and ice shield though
        if ((distanceX > -8.0 && distanceX < 8.0) && (distanceZ > -8.0 && distanceZ < 8.0) && cooldownTimer < 0.01f)
        {
            int attackDecision = Random.Range(1,10);

            if (attackDecision <= 7)
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
            else
            {
                //poison well that is like AoE damage. Ticks for damage. This functionality addressed in the prefab being instantiated.
                //this spell will be inaccurate to prevent it from being too strong
                int randomOffset1 = Random.Range(0, 3);
                int randomOffset2 = Random.Range(0, 3);
                Vector3 wellPos = new Vector3(target.transform.position.x + randomOffset1, target.transform.position.y, target.transform.position.z + randomOffset2);
                anim.SetTrigger("isSummoning");
                Instantiate(poisonWell, wellPos, this.transform.rotation);
                cooldownTimer = cooldown;
            }          
            
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