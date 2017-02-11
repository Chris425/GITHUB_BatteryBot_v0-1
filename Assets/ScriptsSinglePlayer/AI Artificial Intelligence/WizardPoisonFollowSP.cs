
using UnityEngine;
using System.Collections;

public class WizardPoisonFollowSP : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    
    public int bossHealth = 20;
    private float shieldHeight;

    //gamestates correspond to behaviour methods. Only one can be active at a time.
    public bool gameState_OoC;
    public bool gameState_MovingToTarget;
    public bool gameState_InRangeAttacking;
    public bool gameState_FindCover;

    public bool isDroppingSkull = false;
    public GameObject purpleSkull;

    //get location of character!
    public GameObject target;
    public GameObject fleeDestination;

    public float distanceX;
    public float distanceZ;
    public float distanceY;
    private float cooldown = 5.5f;
    private float PoisonWellCooldown = 7.0f; // poison wizard is balanced offense and defense
    private float shieldCooldown = 9.0f;

    private float cooldownTimer;
    private float IceBlastCooldownTimer;
    
    bool shouldPlayAggroEffect = false;
    Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);
    
    public GameObject CasterSpawnLoc;
    public GameObject WizHeal;
    public GameObject objToSpawnPoison;
    public GameObject DeathSpecEffect;
    public GameObject BloodSpecEffect;
    public GameObject AggroSpecEffect;
    public GameObject PoisonWell;
    public GameObject PoisonShield;

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

    public bool isAggroed;
    private bool hasDied;

    void OnEnable()
    {
        hasDied = false;
        gameState_OoC = true;
        gameState_MovingToTarget = false;
        gameState_InRangeAttacking = false;
        gameState_FindCover = false;


        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.Find("BatteryBot");

        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        anim.SetBool("IsAggroed", false);
        isAggroed = false;
        shouldPlayAggroEffect = true;

        var myCollider = this.gameObject.GetComponent<Collider>();
        shieldHeight = (myCollider.bounds.size.y) / 2;
    }

    void Update()
    {
        if (!hasDied)
        {


            cooldownTimer -= 0.03f;
            IceBlastCooldownTimer -= 0.03f;

            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;
            distanceY = this.transform.position.y - target.transform.position.y;
            checkAggro();

            if (isAggroed)
            {

                if (bossHealth > 6 && (distanceX < -8.0 || distanceX > 8.0) && (distanceZ < -8.0 || distanceZ > 8.0) && !gameState_FindCover)
                {
                    Behaviour_MovingToTarget();
                }
                else if (bossHealth > 6 && !gameState_FindCover)
                {
                    Behaviour_InRangeAttacking();
                }
                else
                {
                    gameState_FindCover = true;
                    gameState_InRangeAttacking = false; gameState_MovingToTarget = false;
                }


                if (gameState_FindCover)
                {
                    Behaviour_FindCover();
                }

            }

        }
    }

    private void checkAggro()
    {
        if (!gameState_FindCover)
        {


            if ((distanceX > -15 && distanceX < 15) && (distanceZ > -15 && distanceZ < 15) && (distanceY > -10 && distanceY < 10))
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



    void Behaviour_MovingToTarget()
    {
        //make the spawnloc aim at the player - so you won't be safe up high either!
        Vector3 targetPostition = new Vector3(target.transform.position.x,
                                       target.transform.position.y,
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
            GameObject myShield = Instantiate(PoisonShield, ShieldPos, this.transform.rotation) as GameObject;
            myShield.transform.parent = this.gameObject.transform;
            cooldownTimer = shieldCooldown;
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

            int randomNum = Random.Range(1, 26);
           
            if (randomNum <= 6 && cooldownTimer < 0.01f)
            {
                //bolt of poison. Applies DoT on player 
                anim.SetTrigger("isAttacking");                
                Instantiate(objToSpawnPoison, CasterSpawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = cooldown;

            }
            else if(randomNum > 6 && randomNum < 11 && cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isHealing");
                Vector3 ShieldPos = new Vector3(this.transform.position.x, this.transform.position.y + shieldHeight, this.transform.position.z);
                GameObject myShield = Instantiate(PoisonShield, ShieldPos, this.transform.rotation) as GameObject;
                myShield.transform.parent = this.gameObject.transform;
                cooldownTimer = shieldCooldown;
            }
            else if (randomNum >= 11 && randomNum < 23 && cooldownTimer < 0.01f)
            {
                //poison well that is like AoE damage. Ticks for damage. This functionality addressed in the prefab being instantiated.
                //this spell will be inaccurate to prevent it from being too strong
                int randomOffset1 = Random.Range(0, 3);
                int randomOffset2 = Random.Range(0, 3);
                Vector3 wellPos = new Vector3(target.transform.position.x + randomOffset1, target.transform.position.y, target.transform.position.z + randomOffset2);
                anim.SetTrigger("isSummoning");
                Instantiate(PoisonWell, wellPos, this.transform.rotation);
                cooldownTimer = PoisonWellCooldown;
            }

            else if(cooldownTimer < 0.01f && bossHealth < 20)
            {
                anim.SetTrigger("isHealing");
                Instantiate(WizHeal, this.transform.position, CasterSpawnLoc.transform.rotation);
                bossHealth += 5;
                Debug.Log("Poison Wizard health is " + bossHealth);
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

    void Behaviour_FindCover()
    {
        //move away from player
        agent.speed = 6.5f;
        anim.SetBool("IsNotInRange", true);
        agent.SetDestination(fleeDestination.transform.position);

        //make ice shields as you run away too
        if (cooldownTimer < 0.01f)
        {
            anim.SetTrigger("isHealing");
            Vector3 ShieldPos = new Vector3(this.transform.position.x, this.transform.position.y + shieldHeight, this.transform.position.z);
            GameObject myShield = Instantiate(PoisonShield, ShieldPos, this.transform.rotation) as GameObject;
            myShield.transform.parent = this.gameObject.transform;
            cooldownTimer = shieldCooldown;
        }

        if ((distanceX < -30.0 || distanceX > 30.0) || (distanceZ < -30.0 || distanceZ > 30.0))
        {
            agent.speed = 4.0f;
            anim.SetBool("IsNotInRange", false);
            anim.SetBool("IsAggroed", false);

            agent.ResetPath();
            //when far away enough, give up and lose aggro until player comes back again 
            gameState_FindCover = false; gameState_InRangeAttacking = false; gameState_MovingToTarget = false;
            isAggroed = false;
            gameState_OoC = true;
        }
    }

    


    public void OnCollisionEnter(Collision other)
    {
       
        //case when your player projectile hits the caster
        if (other.gameObject.name.Contains("Shot") && !hasDied)
        {
            
            isAggroed = true;
            if (other.gameObject.name.Contains("PlayerShot"))
            {
                //Note that multishot has the same damage - you just shoot a bunch at the same time
                if (HeroControllerSP.isSuperCharged == true)
                {
                    bossHealth -= 3;
                    Instantiate(DeathSpecEffect, other.transform.position, this.transform.rotation);
                }
                else
                {
                    bossHealth -= 1;
                    Instantiate(DeathSpecEffect, other.transform.position, this.transform.rotation);
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
                    bossHealth -= 2;
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
                Instantiate(DeathSpecEffect, other.transform.position, this.transform.rotation);
            }


            //consume playershot
            Destroy(other.gameObject);
            if (bossHealth <= 0 && !hasDied)
            {
                //fix to prevent this from occuring many times; 
                //turns out the gameobject isn't destroyed immediately so it can register many collisions...
                hasDied = true;
                agent.Stop();
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
                    GameObject skullDrop = Instantiate(purpleSkull, SkullPos, this.transform.rotation) as GameObject;
                }

                Destroy(other.gameObject);

                Destroy(this.gameObject);
            }

        }
    }

   

}