using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MeleeFollowSP : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;

    public bool hasPathingWhileIdle;
    public GameObject pathDests;
    private List<Vector3> pathDestsVectors;
    private int destinationChoice;
    private EnemyDamageReductionSP edr;
    private int vampArmour = 0;
    private int vampArmourEthereal = 0;
    private int vampArmourPoison = 1;

    //get location of character!
    public GameObject target;
    public float distanceX;
    public float distanceZ;
    public float distanceY;
    public float distancePathX;
    public float distancePathZ;
    private float cooldown = 3.0f;
    private float waitTime = 2.0f;
    private float cooldownTimer;
    bool shouldPlayAggroEffect = false;
    Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);
    private GameObject Player;

    public GameObject BiteSpecEffect1;
    public GameObject BiteSpecEffect2; 
    public GameObject AggroSpecEffect;
    public GameObject SE_Thorns;
    public GameObject SE_VampDeath1;
    public GameObject SE_VampDeath2;
    public GameObject SE_VampDeath3;

    //LOOT
    public GameObject RedBattery;
    public GameObject GreenBattery;
    public GameObject Gear;

    public GameObject GunDrop;

    public bool isAggroed;

    public int bossHealth = 3;
    public int vampireDamage = 15;

    public bool isSummoned = false;
    public bool isPoisonType = false;

    private bool hasDied;

    void OnEnable()
    {
        Player = GameObject.Find("PLAYERBASE");

        hasDied = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.Find("BatteryBot");

        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        anim.SetBool("IsAggroed",false);

        pathDestsVectors = new List<Vector3>();

        if (hasPathingWhileIdle)
        {
            GameObject myDests = Instantiate(pathDests, this.transform.position, this.transform.rotation);
            foreach(Transform dest in myDests.transform)
            {
                pathDestsVectors.Add(dest.position);
            }

        }
        

        if (isSummoned)
        {
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
        
        shouldPlayAggroEffect = true;
    }



    public void OnParticleCollision(GameObject particle)
    {
        if (particle.gameObject.name.Contains("FlameThrowerParticle"))
        {
            isAggroed = true;
            int damageTaken = 0;

            if (isSummoned)
            {
                damageTaken = edr.particleDmgTaken(particle, "ETHEREAL", vampArmourEthereal);
            }
            else if (isPoisonType)
            {
                damageTaken = edr.particleDmgTaken(particle, "POISON", vampArmourPoison);
            }
            else
            {
                damageTaken = edr.particleDmgTaken(particle, "NORMAL", vampArmour);
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

        if (other.gameObject.name.Contains("Shot"))
        {
            isAggroed = true;
            int damageTaken = 0;
            if (isSummoned)
            {
                damageTaken = edr.collisionDmgTaken(other, "ETHEREAL", vampArmourEthereal);
            }
            else if (isPoisonType)
            {
                damageTaken = edr.collisionDmgTaken(other, "POISON", vampArmourPoison);
            }
            else
            {
                damageTaken = edr.collisionDmgTaken(other, "NORMAL", vampArmour);
            }

            bossHealth -= damageTaken;



            if (bossHealth <= 0 && !hasDied)
            {
                death();
            }

            Destroy(other.gameObject);
        }  

        
    }

    public void death()
    {
        //Only spawn loot if it is a normal vampire, not a summoned one.
        GAMEMANAGERSP.numScore += 2;
        anim.SetTrigger("isDying");
        agent.enabled = false;
        SelfDestructSP sd = this.gameObject.AddComponent<SelfDestructSP>() as SelfDestructSP;
        sd.lifeSpan = 1.8f;
        playDeathSE();
        

        hasDied = true;

            //vampire is dead


            if (!isSummoned)
            {


                int randomNum = UnityEngine.Random.Range(1, 14);
                if (randomNum <= 2)
                {
                    int posOffset = UnityEngine.Random.Range(-4, 4);
                    int rotOffset1 = UnityEngine.Random.Range(1, 180);
                    int rotOffset2 = UnityEngine.Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(RedBattery, spawnPos, spawnRot);

                }
                else if (randomNum == 3 || randomNum == 4)
                {
                    int posOffset = UnityEngine.Random.Range(-4, 4);
                    int rotOffset1 = UnityEngine.Random.Range(1, 180);
                    int rotOffset2 = UnityEngine.Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(GreenBattery, spawnPos, spawnRot);

                }
                else if (randomNum == 5)
                {
                    int posOffset = UnityEngine.Random.Range(1, 3);
                    int rotOffset1 = UnityEngine.Random.Range(1, 180);
                    int rotOffset2 = UnityEngine.Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(GunDrop, spawnPos, spawnRot);

                }
                else if (randomNum == 6)
                {
                    int posOffset = UnityEngine.Random.Range(1, 3);
                    int rotOffset1 = UnityEngine.Random.Range(1, 180);
                    int rotOffset2 = UnityEngine.Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(Gear, spawnPos, spawnRot);

                }
            }
            //summoned ones will (usually) drop red batteries to ensure ammo during boss fights
            else
            {
                int randomNum = UnityEngine.Random.Range(1, 20);
                if (randomNum <= 5)
                {
                    Instantiate(RedBattery, this.transform.position, this.transform.rotation);
                }

            }
          
        
    }

    private void playDeathSE()
    {
        int deathSound = UnityEngine.Random.Range(1, 25);
        if (deathSound < 4)
        {
            Instantiate(SE_VampDeath1, this.transform.position, this.transform.rotation);
        }
        else if (deathSound >= 4 && deathSound < 8)
        {
            Instantiate(SE_VampDeath2, this.transform.position, this.transform.rotation);
        }
        else if (deathSound >= 8 && deathSound < 12)
        {
            Instantiate(SE_VampDeath3, this.transform.position, this.transform.rotation);
        }
        //else - don't play sound effect
    }

    void Update()
    {
        if (!hasDied && !HeroControllerSP.isPaused) 
        {



            StartCoroutine("checkAggro");



            if (isAggroed)
            {
                    moveToPlayer();
            }
            //when not aggroed, and if pathing is enabled.
            else if (hasPathingWhileIdle)
            {
                try
                {
                    distancePathX = this.transform.position.x - pathDestsVectors[destinationChoice].x;
                    distancePathZ = this.transform.position.z - pathDestsVectors[destinationChoice].z;
                }
                catch (System.ArgumentOutOfRangeException OoR)
                {
                    GameObject myDests = Instantiate(pathDests, this.transform.position, this.transform.rotation);
                    foreach (Transform dest in myDests.transform)
                    {
                        pathDestsVectors.Add(dest.position);
                    }
                    distancePathX = this.transform.position.x - pathDestsVectors[destinationChoice].x;
                    distancePathZ = this.transform.position.z - pathDestsVectors[destinationChoice].z;
                }
                if (cooldownTimer < 0.1f)
                {
                    StartCoroutine("beginPathing");

                }
                //stop walk anim when you reach your destination        
                else if ((distancePathX > -1 && distancePathX < 1) && (distancePathZ > -1 && distancePathZ < 1))
                {
                    anim.SetBool("isPathing", false);
                }
            }

        }
    }

    private IEnumerator beginPathing()
    {
        yield return null;
        anim.SetBool("isPathing", true);
        yield return null;
        //choose random pathing loc from our list of destinations
        destinationChoice = UnityEngine.Random.Range(0, (pathDestsVectors.Count - 1));
        yield return null;
        //navigate towards it. Use a cooldown so that the npc isn't constantly walking!
        if (agent.isActiveAndEnabled)
        {
            agent.ResetPath();
            yield return null;
            agent.SetDestination(pathDestsVectors[destinationChoice]);
            yield return null;
        }
        cooldownTimer = 13.0f;
        yield return new WaitForSeconds(waitTime);
    }

    private IEnumerator checkAggro()
    {
        cooldownTimer -= 0.03f;


        yield return new WaitForSeconds(waitTime);

        distanceX = this.transform.position.x - target.transform.position.x;
        distanceZ = this.transform.position.z - target.transform.position.z;
        distanceY = this.transform.position.y - target.transform.position.y; //Only check y if we have to - optimization
        yield return new WaitForSeconds(waitTime);

        if (!isSummoned)
        {
            if ((distanceX > -10 && distanceX < 10) && (distanceZ > -10 && distanceZ < 10))
            {
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
            //if you have aggroed, then ran away, and you're too far he gives up
            else if ((distanceX < -45 || distanceX > 45) || (distanceZ < -45 || distanceZ > 45) || (distanceY < -10 || distanceY > 10))
            {
                isAggroed = false;
                anim.SetBool("IsAggroed", false);
                if (agent.isActiveAndEnabled)
                {
                    shouldPlayAggroEffect = true;
                }
            }
        }
        yield return new WaitForSeconds(waitTime);


    }


    void Start()
    {
        //assumption - GameManager object exists once in a scene, and contains exactly one EnemyDamageReductionSP script.
        UnityEngine.Object[] temp = FindObjectsOfType(typeof(EnemyDamageReductionSP));
        edr = temp[0] as EnemyDamageReductionSP;
        
    }


    void moveToPlayer()
    {
        
        //Debug.Log (distance);
        if ((distanceX > -1.2 && distanceX < 1.2) && (distanceZ > -1.2 && distanceZ < 1.2) && cooldownTimer < 0.01f)
        {
            distanceY = this.transform.position.y - target.transform.position.y; //Only check y if we have to - optimization
            if (distanceY > -1.0f && distanceY < 1.0f)
            {
                anim.SetTrigger("isAttacking");
                //we are in range. Start reducing battery. Less if Player has shield!
                DamageReduction();



                cooldownTimer = cooldown;

                int randomNum = UnityEngine.Random.Range(1, 3);
                switch (randomNum)
                {
                    case 1:
                        Instantiate(BiteSpecEffect1, this.transform.position, this.transform.rotation);
                        break;
                    case 2:
                        Instantiate(BiteSpecEffect2, this.transform.position, this.transform.rotation);
                        if (isPoisonType)
                        {
                            HeroController.isPoisoned = true;
                            HeroController.poisonTicks = 3;
                        }
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

    private void DamageReduction()
    {
        DamageReductionScript dmgReduct = new DamageReductionScript();
        //when attacking player, enemy may take thorns damage
        int thornsDmgReturned = dmgReduct.DamageReduction(vampireDamage);
        if (thornsDmgReturned > 0)
        {
           
            Quaternion PlainRot = new Quaternion(90,0,0,90);
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

}