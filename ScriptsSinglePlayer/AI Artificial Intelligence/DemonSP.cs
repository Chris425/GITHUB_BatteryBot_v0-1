using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSP : MonoBehaviour {

    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    
    public GameObject pathDests;
    private List<Vector3> pathDestsVectors;
    private int destinationChoice;
    public float pathingTime = 18.0f;

    //get location of character!
    public GameObject target;
    public float distanceX;
    public float distanceZ;
    public float distanceY;
    public float distancePathX;
    public float distancePathZ;
    private float cooldown = 3.0f;
    private float cooldownFire  = 4.0f;
    private float waitTime = 2.0f;
    private float runSpeed = 17.0f;
    private float cooldownTimer;
    private float cooldownTimerFire;
    bool shouldPlayAggroEffect = false;
    Quaternion aggroRot = new Quaternion(0.0f, 180.0f, 180.0f, 0.0f);
    private GameObject Player;
    private bool hasBeenFrozen;
    private GameObject demon;
    public GameObject spawnLoc;

    private int myArmour = 5;
    private EnemyDamageReductionSP edr;

    //effects
    private Renderer rend;
    public Material glacier;
    public GameObject SE_iceShatter;
    public GameObject SE_DemonFrozen;
    public GameObject fireRangeEffect;
    public GameObject SE_DemonMelee1;
    public GameObject SE_DemonMelee2;
    public GameObject AggroSpecEffect;
    public GameObject SE_Thorns;
    public GameObject SE_DemonDeath;

    //LOOT
    public GameObject RedBattery;
    public GameObject GreenBattery;
    public GameObject Gear;

    public bool isAggroed;
    
    public int bossHealth = 250;
    public int damage = 50;
    

    private bool hasDied;

    void OnEnable()
    {
        hasBeenFrozen = false;
        Player = GameObject.Find("PLAYERBASE");

        hasDied = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.Find("BatteryBot");

        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        anim.SetBool("isAggroed", false);

        pathDestsVectors = new List<Vector3>();


        GameObject myDests = Instantiate(pathDests, this.transform.position, this.transform.rotation);
        foreach (Transform dest in myDests.transform)
        {
            pathDestsVectors.Add(dest.position);
        }


        demon = this.gameObject;

        foreach (Transform child in demon.transform)
        {
            if (child.gameObject.name.Contains("Group3366")) //that's just what the demon skin is called
            {
                rend = child.gameObject.GetComponent<Renderer>();
                //bbMetalSurface = child.gameObject;
            }
        }

        shouldPlayAggroEffect = true;

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
        //case when your player projectile hits the vampire
        if (other.gameObject.name.Contains("Shot") && !hasDied)
        {
            isAggroed = true;
            if (!hasBeenFrozen)
            {
                anim.speed = 3.0f;
                agent.speed = runSpeed;
            }
            
            anim.SetBool("isPathing", false);
            anim.SetBool("isAggroed", true);
            //normal damage reduction
            int damageTaken = 0;            
            damageTaken = edr.collisionDmgTaken(other, "NORMAL", myArmour);
            bossHealth -= damageTaken;

            //extra conditions will be dealt with here in this class...
            if (other.gameObject.name.Contains("Shield_Shot"))
            {
                if (!hasBeenFrozen)
                {
                    //Demon is weak to ice
                    hasBeenFrozen = true;
                    Instantiate(SE_DemonFrozen, other.transform.position, this.transform.rotation);
                    if (rend != null)
                    {
                        rend.material = glacier;
                    }
                    bossHealth = 20;
                    damage = 20;
                }
                anim.speed = 1;
                agent.speed = 1;
            }
            else if (other.gameObject.name.Contains("PlayerFRZOrbShot"))
            {
                if (!hasBeenFrozen)
                {
                    //Demon is weak to ice
                    hasBeenFrozen = true;
                    Instantiate(SE_DemonFrozen, other.transform.position, this.transform.rotation);
                    if (rend != null)
                    {
                        rend.material = glacier;
                    }
                    bossHealth = 10;
                }
                anim.speed = 1;
                agent.speed = 1;
                

            }

            if (bossHealth <= 0 && !hasDied)
            {
                death();
            }

            Destroy(other.gameObject);


        }
    }


    public void death()
    {

        anim.SetTrigger("isDying");
        agent.enabled = false;
        
        hasDied = true;

        if (hasBeenFrozen)
        {
            GAMEMANAGERSP.numScore += 10;
            Instantiate(SE_iceShatter, this.transform.position, this.transform.rotation);
            Instantiate(SE_DemonFrozen, this.transform.position, this.transform.rotation);
            Instantiate(SE_iceShatter, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        else
        {
            anim.speed = 1.0f;
            SelfDestructSP sd = this.gameObject.AddComponent<SelfDestructSP>() as SelfDestructSP;
            sd.lifeSpan = 2.0f;
            Instantiate(SE_DemonDeath, this.transform.position, this.transform.rotation);
            //very high score increase if you're skilled enough to kill without freezing first!
            GAMEMANAGERSP.numScore += 666;
        }

        //change loot
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
                    moveToPlayer();
                }
            }
            else
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

    private IEnumerator checkAggro()
    {
        cooldownTimer -= 0.03f;
        cooldownTimerFire -= 0.03f;


        yield return new WaitForSeconds(waitTime);

        distanceX = this.transform.position.x - target.transform.position.x;
        distanceZ = this.transform.position.z - target.transform.position.z;
       
        yield return new WaitForSeconds(waitTime);
        if ((distanceX > -6.5f && distanceX < 6.5f) && (distanceZ > -6.5f && distanceZ < 6.5f))
        {
            distanceY = this.transform.position.y - target.transform.position.y; //Only check y if we have to - optimization
            if (distanceY > -7.0f && distanceY < 1.0f) //Essentially they can see up but not down through the floor
            {
                isAggroed = true;
                anim.SetBool("isAggroed", true);
                if (shouldPlayAggroEffect)
                {
                    Instantiate(AggroSpecEffect, this.transform.position, aggroRot);
                    anim.SetTrigger("isRoaring");
                    Transform aggroRange = this.transform.Find("AggroRange");
                    Destroy(aggroRange.gameObject);
                    if (!hasBeenFrozen)
                    {
                        anim.speed = 3.0f;
                        agent.speed = runSpeed;
                    }
                    shouldPlayAggroEffect = false;
                }
            }
        }        
        yield return new WaitForSeconds(waitTime);

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
        cooldownTimer = pathingTime;
        yield return new WaitForSeconds(waitTime);
        
        
    }


    void moveToPlayer()
    {
        Vector3 targetPostition = new Vector3(target.transform.position.x,
                                       target.transform.position.y + 0.9f,
                                       target.transform.position.z);
        spawnLoc.transform.LookAt(targetPostition);


        //IN MELEE DISTANCE
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
                        Instantiate(SE_DemonMelee1, this.transform.position, this.transform.rotation);
                        break;
                    case 2:
                        Instantiate(SE_DemonMelee2, this.transform.position, this.transform.rotation);
                        break;
                }
            }

        }
        //IN SPELLCASTING DISTANCE
        else if ((distanceX > -8 && distanceX < 8) && (distanceZ > -8 && distanceZ < 8) && cooldownTimerFire < 0.01f)
        {
            anim.SetTrigger("isAttacking");
            float offset = Random.Range(-0.03f, 0.01f);
            Quaternion spawnRot = new Quaternion
                (spawnLoc.transform.rotation.x + offset,
                spawnLoc.transform.rotation.y + offset,
                spawnLoc.transform.rotation.z + offset,
                spawnLoc.transform.rotation.w);
            Instantiate(fireRangeEffect, spawnLoc.transform.position, spawnRot);
            cooldownTimerFire = cooldownFire;
        }
        //TOO FAR AWAY
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
        int thornsDmgReturned = dmgReduct.DamageReduction(damage);
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


}
