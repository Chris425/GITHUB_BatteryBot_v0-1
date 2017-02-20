using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level2BossLightningSP : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    //get location of character!
    public GameObject target;
    public float distanceX;
    public float distanceZ;
    public float distanceY;
    public GameObject spawnLoc;
    private float cooldown = 3.0f;
    private float cooldownPowerup = 7.0f;
    private float cooldownTimer;
    private float cooldownTimerPowerup;
    private float numTimesPoweredUp = 0;
    private float currentSpeed;
    private bool hasDoneFirstSound;
    private bool hasDoneSecondSound;

    bool shouldPlayAggroEffect = false;
    Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);

    public int maxBossHealth = 200;
    public int bossHealth = 195; //195 looks better on the slider
    private int bossDamage = 5;

    public GameObject PowerUp;
    public GameObject HealUp;

    public GameObject leapExplosion;
    public GameObject swordFireRangeEffect;
    public GameObject shieldBash;
    public GameObject SwordSlashEffect;
    public GameObject DeathSpecEffect;
    public GameObject BloodSpecEffect;
    public GameObject AggroSpecEffect;

    //Sound effects
    public GameObject MinotauroHurtSound1;
    public GameObject MinotauroHurtSound2;
    public GameObject MinotauroBigSlamAttack;
    public GameObject SlamAttackWell;

    private bool isLeaping = false;
    private bool isGroundSlamming = false;
    private bool isHurt = false;

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

    public bool isAggroed;
    private bool hasDied;

    public Slider bossHealthSlider;
    public Image Fill;  // assign in the editor the "Fill"

    private int counter = 0;

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
        Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);

        currentSpeed = agent.speed;

        bossHealthSlider.value = 200;
        bossHealthSlider.gameObject.SetActive(false);

        hasDoneFirstSound = false;
        hasDoneSecondSound = false;

    }


    private void leap()
    {
        transform.position += transform.forward * Time.deltaTime * 12;
        int shouldSpawn = Random.Range(1, 60);
        if (shouldSpawn <= 5)
        {
            Instantiate(leapExplosion, this.transform.position, this.transform.rotation);
        }
    }

    private void slam()
    {
        int shouldSpawn = Random.Range(1, 60);
        if (shouldSpawn <= 5)
        {            
            int randomOffset1 = Random.Range(-25, 25);
            int randomOffset2 = Random.Range(-25, 25);
            Vector3 wellPos = new Vector3(this.transform.position.x + randomOffset1, this.transform.position.y - 0.4f, this.transform.position.z + randomOffset2);
            Instantiate(SlamAttackWell, wellPos, this.transform.rotation);            
        }
    }

    private void UpdateSlider()
    {
        bossHealthSlider.value = bossHealth;
        
        Fill.color = Color.green;
       
    }

    void Update()
    {
        if (!hasDied)
        {
            if (isAggroed)
            {
                UpdateSlider();
            }

            cooldownTimer -= 0.015f;
            cooldownTimerPowerup -= 0.015f;

            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;
            distanceY = this.transform.position.y - target.transform.position.y;

            if (isLeaping)
            {
                if (cooldownTimer > 0.01f)
                {
                    leap();
                }
                else
                {
                    //instantiate leap explosion upon landing
                    Instantiate(leapExplosion, this.transform.position, this.transform.rotation);
                    Instantiate(shieldBash, this.transform.position, target.transform.rotation);
                    cooldownTimer = 2.9f;
                    isLeaping = false;
                    agent.enabled = true;
                    if (agent.isActiveAndEnabled)
                    {
                        agent.Resume();
                    }

                }
            }
            else if (isGroundSlamming)
            {
                if (cooldownTimer > 0.01f)
                {
                    slam();
                }
                else
                {
                    cooldownTimer = 2.9f;
                    agent.enabled = true;
                    isGroundSlamming = false;
                    if (agent.isActiveAndEnabled)
                    {
                        agent.Resume();
                    }

                }
            }
            else if (isHurt)
            {
                if (cooldownTimer > 0.01f)
                {
                  //do nothing; play animation that you're hurt until you recover
                }
                else
                {
                    cooldownTimer = 2.9f;
                    agent.enabled = true;
                    isHurt = false;
                    if (agent.isActiveAndEnabled)
                    {
                        agent.Resume();
                    }

                }
            }
            else
            {
                checkAggro();
                if (isAggroed)
                {
                    moveToPlayer();
                }
            }
        }
    }


    private void checkAggro()
    {
        if ((distanceX > -30 && distanceX < 30) && (distanceZ > -30 && distanceZ < 30) && (distanceY > -5 && distanceY < 5))
        {
            isAggroed = true;
            bossHealthSlider.gameObject.SetActive(true);

            anim.SetBool("IsAggroed", true);
            if (shouldPlayAggroEffect)
            {
                Instantiate(AggroSpecEffect, this.transform.position, aggroRot);
                shouldPlayAggroEffect = false;
            }

        }
        

    }

    void moveToPlayer()
    {


        Vector3 targetPostition = new Vector3(target.transform.position.x,
                                      target.transform.position.y,
                                      target.transform.position.z);
        spawnLoc.transform.LookAt(targetPostition);

        //let the boss face the player at all times
        //this.transform.LookAt(targetPostition);
        this.transform.LookAt(targetPostition);
        spawnLoc.transform.LookAt(targetPostition);


        int attackDecision = Random.Range(1, 35);

        //IN MELEE RANGE
        if ((distanceX > -7.5 && distanceX < 7.5) && (distanceZ > -7.5 && distanceZ < 7.5) && (distanceY > -5 && distanceY < 5))
        {
            //make him idle if in range regardless of cooldowns
            anim.SetBool("isInRange", true);

            if (attackDecision < 6 && cooldownTimerPowerup < 0.01f && numTimesPoweredUp <= 7 && bossHealth > (maxBossHealth * 0.5))
            {
                GameObject myPowerUp = Instantiate(PowerUp, this.transform.position, this.transform.rotation);
                myPowerUp.transform.parent = this.gameObject.transform;

                anim.SetTrigger("isPoweringUp");
                bossDamage += 5;
                agent.speed += 0.5f;
                currentSpeed = agent.speed;
                cooldownTimer = cooldownPowerup;
                numTimesPoweredUp += 1;
            }
            else if (attackDecision >= 6 && attackDecision < 15 && cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isAttacking");
                float offset = Random.Range(-0.03f, 0.01f);
                Quaternion spawnRot = new Quaternion
                    (spawnLoc.transform.rotation.x + offset,
                    spawnLoc.transform.rotation.y + offset,
                    spawnLoc.transform.rotation.z + offset,
                    spawnLoc.transform.rotation.w);
                Instantiate(swordFireRangeEffect, spawnLoc.transform.position, spawnRot);
                cooldownTimer = cooldown;
            }
            else if (attackDecision >= 15 && attackDecision < 20 && cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isShieldBashing");
                Instantiate(shieldBash, this.transform.position, this.transform.rotation);
               
                cooldownTimer = cooldown;
            }
            else if (attackDecision >= 20 && attackDecision < 25 && cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isAttacking");
                //we are in range. Start reducing battery. Less if Player has shield!
                int damageDealt = 0;
                if (HeroControllerSP.hasArmour)
                {
                    //armour and shield
                    if (HeroControllerSP.hasShield && HeroControllerSP.isSlot4)
                    {
                        damageDealt = (bossDamage - 8);
                    }
                    else
                    {
                        //only armour
                        damageDealt = (bossDamage - 5);
                    }
                }
                else
                {
                    //no armour no shield
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

                cooldownTimer = cooldown;

                Instantiate(SwordSlashEffect, this.transform.position, this.transform.rotation);
            }


        }
        //NOT IN MELEE RANGE
        else
        {
            anim.SetBool("isInRange", false);
            if (isAggroed)
            {
                if (attackDecision < 12 && cooldownTimerPowerup < 0.01f && numTimesPoweredUp <= 7 && bossHealth > (maxBossHealth * 0.5))
                {
                    GameObject myPowerUp = Instantiate(PowerUp, this.transform.position, this.transform.rotation);
                    myPowerUp.transform.parent = this.gameObject.transform;

                    anim.SetTrigger("isPoweringUp");
                    bossDamage += 5;
                    agent.speed += 0.5f;
                    currentSpeed = agent.speed;
                    cooldownTimerPowerup = cooldownPowerup;
                    numTimesPoweredUp += 1;
                }
                else if (attackDecision >= 12 && attackDecision < 22 && cooldownTimer < 0.01f)
                {
                    anim.SetTrigger("isAttacking");
                    Instantiate(swordFireRangeEffect, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    cooldownTimer = cooldown;
                }
                else if (attackDecision >= 22 && attackDecision < 28 && cooldownTimer < 0.01f)
                {
                    isLeaping = true;
                    anim.SetTrigger("isLeaping");
                    cooldownTimer = 1.6f;
                    agent.enabled = false;
                }

                if (bossHealth < (maxBossHealth * 0.5) && cooldownTimer < 0.01f)
                {
                    anim.SetTrigger("isRoaring");
                    anim.SetBool("isInRange", true);
                    isGroundSlamming = true;
                    agent.enabled = false;
                    cooldownTimer = 5.0f;
                    Instantiate(MinotauroBigSlamAttack, this.transform.position, this.transform.rotation);                    
                }
                                

                if (agent.isActiveAndEnabled)
                {
                    agent.SetDestination(target.transform.position);
                }

            }
        }
            




    }

    


    public void OnCollisionEnter(Collision other)
    {
        //case when your player projectile hits the boss
        if (other.gameObject.name.Contains("Shot") && !hasDied)
        { 
            
            isAggroed = true;
            if (other.gameObject.name.Contains("PlayerShot"))
            {
                //Note that multishot has the same damage - you just shoot a bunch at the same time
                if (HeroControllerSP.isSuperCharged == true)
                {
                    bossHealth -= 2;
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
                Instantiate(DeathSpecEffect, other.transform.position, this.transform.rotation);
            }

            if (bossHealth < (0.66 * maxBossHealth) && !hasDoneFirstSound) 
            {
                anim.SetTrigger("isHurt");
                isHurt = true;
                agent.enabled = false;
                cooldownTimer = 2.5f;

                Instantiate(MinotauroHurtSound1, spawnLoc.transform.position, this.transform.rotation);
                hasDoneFirstSound = true;
            }
            if (bossHealth < (0.33 * maxBossHealth) && !hasDoneSecondSound)
            {
                anim.SetTrigger("isHurt");
                isHurt = true;
                agent.enabled = false;
                Instantiate(MinotauroHurtSound2, spawnLoc.transform.position, this.transform.rotation);
                hasDoneSecondSound = true;
            }
            //consume playershot
            Destroy(other.gameObject);
            if (bossHealth <= 0 && !hasDied)
            {
                //fix to prevent this from occuring many times; 
                //turns out the gameobject isn't destroyed immediately so it can register many collisions...
                hasDied = true;


                bossHealthSlider.gameObject.SetActive(false);
                //kill the boss
                //possibly spawn some loot!


                int randomNum = Random.Range(1, 21);
                if (randomNum <= 3)
                {
                    int posOffset = Random.Range(1, 4);
                    int rotOffset1 = Random.Range(1, 180);
                    int rotOffset2 = Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(RedBattery, spawnPos, spawnRot);

                }
                else if (randomNum > 3 && randomNum <= 5)
                {
                    int posOffset = Random.Range(1, 4);
                    int rotOffset1 = Random.Range(1, 180);
                    int rotOffset2 = Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(GreenBattery, spawnPos, spawnRot);

                }
                else if (randomNum == 6)
                {
                    int posOffset = Random.Range(1, 4);
                    int rotOffset1 = Random.Range(1, 180);
                    int rotOffset2 = Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(SuperBattery, spawnPos, spawnRot);

                }
                else if (randomNum == 7)
                {
                    int posOffset = Random.Range(1, 3);
                    int rotOffset1 = Random.Range(1, 180);
                    int rotOffset2 = Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(GreatswordDrop, spawnPos, spawnRot);

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
                else if (randomNum == 12)
                {
                    int posOffset = Random.Range(1, 3);
                    int rotOffset1 = Random.Range(1, 180);
                    int rotOffset2 = Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(BoosterDrop, spawnPos, spawnRot);

                }

                else if (randomNum >= 13 && randomNum <= 16)
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

   
   
}