﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MinotauroArena : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    //get location of character!
    public GameObject target;
    public float distanceX;
    public float distanceZ;
    public float distanceY;
    public GameObject spawnLoc;
    private float cooldown = 3.5f;
    private float cooldownPowerup = 8.0f;
    private float cooldownTimer;
    private float cooldownTimerPowerup;
    private float numTimesPoweredUp = 0;
    private float currentSpeed;
    private bool hasDoneFirstSound;
    private bool hasDoneSecondSound;
    
    Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);

    public int maxBossHealth = 150;
    public int bossHealth = 145; //145 looks better on the slider
    private int bossDamage = 5;
    private int myArmour = 1;
    private EnemyDamageReductionSP edr;

    public GameObject PowerUp;
    public GameObject HealUp;

    public GameObject leapExplosion;
    public GameObject swordFireRangeEffect;
    public GameObject shieldBash;
    public GameObject SwordSlashEffect;
    public GameObject AggroSpecEffect;
    public GameObject SE_Hit_Poison;
    public GameObject SE_Thorns;

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

    //skull drops for arena only
    public GameObject blueSkullDrop;
    public GameObject redSkullDrop;
    public GameObject purpleSkullDrop;
    public GameObject bronzeSkullDrop;

    //armour - rare chance
    public GameObject legArmourDrop;
    public GameObject bootsArmourDrop;
    public GameObject bodyArmourDrop;
    public GameObject helmArmourDrop;

    public GameObject oneUpDrop;


    public GameObject BigLootDump;

    public bool isAggroed;
    private bool hasDied;

    public Slider bossHealthSlider;
    public Image Fill;  // assign in the editor the "Fill"

    private int counter = 0;
    private GameObject Player;

    public int slotNum;
    GAMEMANAGER_ARENA gm;
    private GameObject respawnDest;



    void Start()
    {

        respawnDest = GameObject.Find("EnemyRespawnIfOoR");
        

        bossHealthSlider.value = 150;
        //assumption - GameManager object exists once in a scene, and contains exactly one EnemyDamageReductionSP script.
        UnityEngine.Object[] temp = FindObjectsOfType(typeof(EnemyDamageReductionSP));
        edr = temp[0] as EnemyDamageReductionSP;

        Player = GameObject.Find("PLAYERBASE");
        hasDied = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.Find("BatteryBot");

        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        anim.SetBool("IsAggroed", false);

        isAggroed = true;
        Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);

        currentSpeed = agent.speed;


        gm = (GAMEMANAGER_ARENA)GameObject.Find("GAMEMANAGER_ARENA").GetComponent("GAMEMANAGER_ARENA");


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

        //Fill.color = Color.green;

    }

    void Update()
    {
        if (!HeroController.isPaused && !hasDied && bossHealthSlider != null)
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
                if (isAggroed)
                {
                    moveToPlayer();
                }
            }
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

                DamageReduction();


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

                try
                {
                    if (agent.isActiveAndEnabled)
                    {
                        agent.SetDestination(target.transform.position);
                    }
                }
                catch
                {
                    //if you have an error here, minotauro probably ran through a wall or the floor.
                    //respawn him back to where he started.
                    this.gameObject.transform.position = respawnDest.gameObject.transform.position;
                    System.Console.WriteLine("Out of bounds, teleporting");

                }
               

            }
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

    public void deathAndTransition()
    {
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
        if (bossHealth <= 0 && !hasDied)
        {
            GAMEMANAGERSP.numScore += 50;
            anim.SetTrigger("isDying");
            agent.enabled = false;
            SelfDestructSP sd = this.gameObject.AddComponent<SelfDestructSP>() as SelfDestructSP;
            sd.lifeSpan = 2.5f;
            hasDied = true;
            gameManagerLvl3.isMinoDead = true;
            gameManagerLvl3.removeBarrier();

            //bossHealthSlider.gameObject.SetActive(false);
            //kill the boss
            //possibly spawn some loot!


            int randomNum = Random.Range(1, 34);
            if (randomNum <= 3)
            {
                int posOffset = Random.Range(1, 4);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(RedBattery, spawnPos, spawnRot);
                Instantiate(legArmourDrop, spawnPos, spawnRot);

            }
            else if (randomNum > 3 && randomNum <= 5)
            {
                int posOffset = Random.Range(1, 4);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(GreenBattery, spawnPos, spawnRot);
                Instantiate(bootsArmourDrop, spawnPos, spawnRot);

            }
            else if (randomNum == 6)
            {
                int posOffset = Random.Range(1, 4);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(SuperBattery, spawnPos, spawnRot);
                Instantiate(bodyArmourDrop, spawnPos, spawnRot);

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
                Instantiate(helmArmourDrop, spawnPos, spawnRot);

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
                Instantiate(oneUpDrop, spawnPos, spawnRot);

            }

            else if (randomNum >= 17 && randomNum <= 22)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(blueSkullDrop, spawnPos, spawnRot);
            }

            else if (randomNum >= 23 && randomNum <= 26)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(purpleSkullDrop, spawnPos, spawnRot);
            }

            else if (randomNum >= 27 && randomNum <= 29)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(redSkullDrop, spawnPos, spawnRot);
            }
            else if (randomNum == 30)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(bronzeSkullDrop, spawnPos, spawnRot);
            }
            else if (randomNum >= 31)
            {
                int posOffset = Random.Range(1, 3);
                int rotOffset1 = Random.Range(1, 180);
                int rotOffset2 = Random.Range(1, 180);
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                Instantiate(oneUpDrop, spawnPos, spawnRot);
            }

            Instantiate(BigLootDump, this.transform.position, this.transform.rotation);

            gm.minorBossDeath(slotNum);

            GAMEMANAGERSP.numArenaScore += 20;
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

            Destroy(other.gameObject);

            deathAndTransition();


        }
    }



}