using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level2BossSP : MonoBehaviour
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
    
    bool shouldPlayAggroEffect = false;
    Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);

    public int bossHealth = 200;
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

    public bool isLeaping = false;

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

    public static bool isAggroed;


    public Slider bossHealthSlider;
    public Image Fill;  // assign in the editor the "Fill"

    private int counter = 0;

    void OnEnable()
    {
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

    private void UpdateSlider()
    {
        bossHealthSlider.value = bossHealth;
        
        Fill.color = Color.green;
       
    }

    void Update()
    {
        if (isAggroed)
        {
            UpdateSlider();
        }
        
        cooldownTimer -= 0.02f;
        cooldownTimerPowerup -= 0.02f;

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
        else
        {
            checkAggro();
            if (isAggroed)
            {
                moveToPlayer();
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
        //Paladin only gives up if your vertical distance is more than 15 units
        else if ((distanceY < -15 || distanceY > 15))
        {
            isAggroed = false;
            anim.SetBool("IsAggroed", false);
            anim.SetBool("isInRange", true);
            if (agent.isActiveAndEnabled)
            {
                shouldPlayAggroEffect = true;
                agent.Stop();
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


        int attackDecision = Random.Range(1, 30);

        //IN MELEE RANGE
        if ((distanceX > -7.5 && distanceX < 7.5) && (distanceZ > -7.5 && distanceZ < 7.5) && (distanceY > -5 && distanceY < 5))
        {
            //make him idle if in range regardless of cooldowns
            anim.SetBool("isInRange", true);

            if (attackDecision < 8 && cooldownTimer < 0.01f)
            {
                GameObject myPowerUp = Instantiate(PowerUp, this.transform.position, this.transform.rotation);
                myPowerUp.transform.parent = this.gameObject.transform;

                anim.SetTrigger("isPoweringUp");
                bossDamage += 5;
                agent.speed += 0.5f;
                currentSpeed = agent.speed;
                cooldownTimer = cooldownPowerup;
            }
            else if (attackDecision >= 8 && attackDecision < 15 && cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isAttacking");
                Instantiate(swordFireRangeEffect, spawnLoc.transform.position, spawnLoc.transform.rotation);
                cooldownTimer = cooldown;
            }
            else if (attackDecision >= 15 && attackDecision < 20 && cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isShieldBashing");
                Instantiate(shieldBash, this.transform.position, this.transform.rotation);
               
                cooldownTimer = cooldown;
            }
            else if (cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isAttacking");
                //we are in range. Start reducing battery. Less if Player has shield!
                if (HeroControllerSP.hasShield && HeroControllerSP.isSlot4)
                {
                    HeroControllerSP.battery -= (bossDamage - 5);
                }
                else
                {
                    HeroControllerSP.battery -= bossDamage;
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
                if (attackDecision < 12 && cooldownTimerPowerup < 0.01f && numTimesPoweredUp <= 5)
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
                else if (attackDecision >= 12 && attackDecision < 25 && cooldownTimer < 0.01f)
                {
                    anim.SetTrigger("isAttacking");
                    Instantiate(swordFireRangeEffect, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    cooldownTimer = cooldown;
                }
                else if (attackDecision >= 25 && cooldownTimer < 0.01f)
                {
                    isLeaping = true;
                    anim.SetTrigger("isLeaping");
                    cooldownTimer = 1.8f;
                    agent.enabled = false;
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
        if (other.gameObject.name.Contains("Shot"))
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
            if (bossHealth <= 0)
            {
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