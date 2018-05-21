using UnityEngine;
using System.Collections;

public class MeleeFollowBoss : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    //get location of character!
    public GameObject target;
    public float distanceX;
    public float distanceZ;
    public float distanceY;
    private float waitTime = 1.0f;
    private float cooldown = 6.0f;
    private float cooldownPowerup = 8.0f;
    private float cooldownTimer;
    private float cooldownTimerPowerup;
    private float numTimesPoweredUp = 0;
    public int bossHealth = 10;
    bool shouldPlayAggroEffect = false;
    Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);

    private int bossDamage = 20;
    private int myArmour = 2;
    private EnemyDamageReductionSP edr;
    private GameObject Player;

    public bool isSummoned;

    public GameObject SkelePowerUp;
    public GameObject BiteSpecEffect1;
    public GameObject BiteSpecEffect2;
    public GameObject AggroSpecEffect;
    public GameObject SE_Thorns;
    public GameObject SE_SkeleZombieDeath;
    public GameObject SE_SkeleZombieDeath2;
    public GameObject SE_SkeleZombieDeath3;

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

    void OnEnable()
    {
        Player = GameObject.Find("PLAYERBASE");
        hasDied = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.Find("BatteryBot");

        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        anim.SetBool("IsAggroed", false);
        isAggroed = false;
        shouldPlayAggroEffect = true;
        Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);

        if (isSummoned)
        {
            //BUG - if you summon an enemy you'll need to intialize these values at start time, or they're zero, and that means they're in range to hit you :p
            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;
            distanceY = this.transform.position.y - target.transform.position.y;

            isAggroed = true;
            anim.SetBool("IsAggroed", true);
        }
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
        //case when your player projectile hits the boss
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

    private void playDeathSE()
    {
        int deathSound = UnityEngine.Random.Range(1, 15);
        if (deathSound < 4)
        {
            Instantiate(SE_SkeleZombieDeath, this.transform.position, this.transform.rotation);
        }
        else if (deathSound >= 4 && deathSound < 8)
        {
            Instantiate(SE_SkeleZombieDeath2, this.transform.position, this.transform.rotation);
        }
        else if (deathSound >= 8 && deathSound < 12)
        {
            Instantiate(SE_SkeleZombieDeath3, this.transform.position, this.transform.rotation);
        }
        //else - don't play sound effect
    }


    public void death()
    {
        GAMEMANAGERSP.numArenaScore += 6;
        anim.SetTrigger("isDying");
        agent.enabled = false;
        SelfDestructSP sd = this.gameObject.AddComponent<SelfDestructSP>() as SelfDestructSP;
        sd.lifeSpan = 2.3f;
        hasDied = true;
        playDeathSE();

        //kill the skeleton
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

    }

    void Update()
    {
        if (!hasDied)
        {
            StartCoroutine("checkAggro");

            if (isAggroed)
            {
                if (!HeroController.isPaused)
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
            yield return new WaitForSeconds(waitTime);
            if ((distanceX > -20 && distanceX < 20) && (distanceZ > -20 && distanceZ < 20))
            {
                distanceY = this.transform.position.y - target.transform.position.y; //Only check y if we have to - optimization
                if (distanceY > -3.0f && distanceY < 3.0f)
                {
                    isAggroed = true;
                    anim.SetBool("IsAggroed", true);
                    if (shouldPlayAggroEffect)
                    {
                        Instantiate(AggroSpecEffect, this.transform.position, aggroRot);
                        shouldPlayAggroEffect = false;
                    }
                }

            }
        }
        //This monster doesn't give up - will chase until defeated
    }

    private void DamageReduction()
    {
        DamageReductionArena dmgReduct = new DamageReductionArena();
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
            death();
        }
    }

    void moveToPlayer()
    {
        cooldownTimer -= 0.02f;
        cooldownTimerPowerup -= 0.02f;

        //Debug.Log (distance);
        int attackDecision = Random.Range(1, 10);

        //50% chance of doing the power up. When in range, it may do a power up or attack - but only one because it consumes the cooldown.
        if (attackDecision < 4 && cooldownTimerPowerup < 0.01f && numTimesPoweredUp <= 5)
        {
            GameObject myPowerUp = Instantiate(SkelePowerUp, this.transform.position, this.transform.rotation);
            myPowerUp.transform.parent = this.gameObject.transform;

            anim.SetTrigger("isPoweringUp");
            bossDamage += 3;
            agent.speed += 1.0f;
            cooldownTimerPowerup = cooldownPowerup;

            numTimesPoweredUp += 1;
        }

        if ((distanceX > -2.7 && distanceX < 2.7) && (distanceZ > -2.7 && distanceZ < 2.7) && cooldownTimer < 0.01f)
        {
            distanceY = this.transform.position.y - target.transform.position.y; //Only check y if we have to - optimization
            if (distanceY > -2.0f && distanceY < 2.0f)
            {
                anim.SetTrigger("isAttacking");
                //we are in range. Start reducing battery.

                DamageReduction();

                cooldownTimer = cooldown;

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
            if (agent.isActiveAndEnabled)
            {
                agent.SetDestination(target.transform.position);
            }

        }

    }




}