
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FinalBossSP : MonoBehaviour
{
    private bool isPhase1;
    private bool isPhase2;
    private bool isPhase3;
    private bool istransition1to2;
    private bool istransition2to3;

    private bool hasDoneTransition1to2;
    private bool hasDoneTransition2to3;

    private float cooldownTimer;
    private float behaviourModeTimer;
    private float specialCooldownTimer;
    private float pathingCooldownTimer;

    public Slider bossHealthSlider;
    private int maxBossHealth = 900;
    public int bossHealth = 900;
    public float standardSpeed = 5.5f;
    public GameObject spawnLoc;
    public GameObject target;
    public float distanceX;
    public float distanceZ;
    public float distancePathX;
    public float distancePathZ;
    private int destinationChoice;
    private bool hasDied;
    private GameObject myDests;
    public GameObject portalToEnding;

    //debug texts
    public Text modeText;
    public Text axeText;
    public Text gsText;
    public Text healthValText;
    public Text p1Text;
    public Text p2Text;
    public Text p3Text;
    public Text trans1to2Text;
    public Text trans2to3Text;
    public Text behaviourModeTimerText;
    public Text pathingCooldownTimerText;
    public Text cooldownTimerText;
    public Text specialCooldownTimerText;

    //boss inventory
    public GameObject bossFlame; //fire effect like caster when gs flame active.
    public GameObject bossLightning; //lightning effect when axe active
    public GameObject GreatswordFire; //effect on gs itself   
    public GameObject gun;
    public GameObject GS;
    public GameObject shield;
    public GameObject Axe;


    //Special effects
    public GameObject gunSpecBlueEffect;
    public GameObject GunSpecEffect;
    public GameObject SE_Hit_Poison;
    public GameObject BloodSpecEffect; 
    public GameObject SE_Thorns;
    public GameObject SE_PowerUp;
    public GameObject SE_GodModeBoss;
    public GameObject SE_CoverExplosion;
    public GameObject SE_FinalBossTransitionLoot;
    public GameObject SE_FinalBossDeath;
    public GameObject SE_RedExplosionSP;
    public GameObject SE_FinalTransitionLightning;
    public GameObject SE_FinalTransitionFire;
    public GameObject SE_FinalBigSlamAttack;

    //boss attacks
    public GameObject basicGunProj;
    public GameObject multiGunProj;
    public GameObject GSShot;
    public GameObject AxeShot;//note that these 2 are the same as herocontroller and shoot proj... but dmg is hardcoded when in range like a vamp. yolo?
    public GameObject WizShotFire;
    public GameObject LProjLastBoss;
    public GameObject bossLightningMulti;
    public GameObject summonedVampire;
    public GameObject summonedSummonerBoss;
    private bool isSpawningCircles;
    public GameObject FinalSlamAttackWell;
    
    private int myArmour = 1;
    private EnemyDamageReductionSP edr;
    
    //melee damage numbers
    private int gsDamage;
    private int axeDamage;


    public GameObject pathDests;
    private List<Vector3> pathDestsVectors;
    private List<Vector3> minionSpawnVectors;

    public int mode;
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;

    //various public SE_

    //Text to update for debugging
    private bool isDebugging;

//if isDebugging - have text on screen set to different timers, which mode, booleans!!
//isDebugging = true if metal skll
     void OnEnable()
    {

        //portal to ending - enable on death
        portalToEnding.SetActive(false);
        hasDied = false;

        isPhase1 = true;
        isPhase2 = false;
        isPhase3 = false;
        istransition1to2 = false;
        istransition2to3 = false;
        behaviourModeTimer = 40.0f;

        //damage
        gsDamage = 10;
        axeDamage = 5;

        cooldownTimer = 10.0f;

        hasDoneTransition1to2 = false;
        hasDoneTransition2to3 = false;

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.Find("BatteryBot");

        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;

        if (HeroControllerSP.hasSkull_GOLD)
        {
            isDebugging = true;
        }

        //start in gun mode always
        gun.SetActive(true);
        shield.SetActive(true);
        Axe.SetActive(false);
        GS.SetActive(false);
        bossFlame.SetActive(false);
        bossLightning.SetActive(false);
        GreatswordFire.SetActive(false);


        //cover p1
        pathDestsVectors = new List<Vector3>();        
        myDests = Instantiate(pathDests, this.transform.position, this.transform.rotation);
        foreach (Transform dest in myDests.transform)
        {
            pathDestsVectors.Add(dest.position);
        }
        //obtain references to minion spawn locations
        minionSpawnVectors = new List<Vector3>();

        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.name.Contains("SpawnPortal"))
            {
                minionSpawnVectors.Add(obj.gameObject.transform.position);
            }
        }

        Debug.Log("Number of spawnLoc portals - " + minionSpawnVectors.Count);


    }

    void Start()
    {
        //assumption - GameManager object exists once in a scene, and contains exactly one EnemyDamageReductionSP script.
        UnityEngine.Object[] temp = FindObjectsOfType(typeof(EnemyDamageReductionSP));
        edr = temp[0] as EnemyDamageReductionSP;

    }

    void Update()
    {
        StartCoroutine("Timers");
        StartCoroutine("PhaseBehaviour");
        StartCoroutine("UpdateSlider");
        if (isDebugging)
        {
            StartCoroutine("UpdateCanvas");
        }

    }

    private IEnumerator UpdateCanvas()
    {
        modeText.text = "Mode: " + mode % 2;
        yield return null;
        axeText.text = "Axe melee damage: " + axeDamage;
        yield return null;
        gsText.text = "GS melee damage: " + gsDamage;
        yield return null;
        healthValText.text = "Boss Health: " + bossHealth;
        yield return null;
        p1Text.text = "Phase 1: " + isPhase1;
        yield return null;
        p2Text.text = "Phase 2: " + isPhase2;
        yield return null;
        p3Text.text = "Phase 3: " + isPhase3;
         yield return null;
        trans1to2Text.text = "Transition 1-2: " + istransition1to2;
        yield return null;
        trans2to3Text.text = "Transition 2-3: " + istransition2to3;
        yield return null;
        behaviourModeTimerText.text = "Behaviour mode timer: " + behaviourModeTimer.ToString("n2"); ;
        yield return null;
        pathingCooldownTimerText.text = "pathing cooldown timer: " + pathingCooldownTimer.ToString("n2"); ;
        yield return null;
        cooldownTimerText.text = "Cooldown timer: " + cooldownTimer.ToString("n2"); ;
        yield return null;
        specialCooldownTimerText.text = "Special cooldown timer: " + specialCooldownTimer.ToString("n2"); ;
        yield return null;

    }
    private IEnumerator Timers() //-=1.5f at 60fps feels like approx 1 second
    {
        yield return null;
        behaviourModeTimer -= 1.5f * Time.deltaTime;
        yield return null;
        cooldownTimer -= 1.5f * Time.deltaTime;
        yield return null;
        pathingCooldownTimer -= 1.5f * Time.deltaTime;
        yield return null;
        specialCooldownTimer -= 1.5f * Time.deltaTime;
        if (behaviourModeTimer < 0.0f)
        {
            mode += 1;
            behaviourModeTimer = Random.Range(0.0f, 40.0f) + 30.0f;
            int currMode = mode % 2;
            //you may decide later to just have a generic mode change anim instead of a customized one... CDC
            if (currMode == 0 && !hasDied)
            {
                GameObject modeChange = Instantiate(SE_FinalTransitionLightning, this.transform.position, this.transform.rotation);
                modeChange.transform.parent = this.gameObject.transform;
                isSpawningCircles = false;
            }
            else
            {
                GameObject modeChange = Instantiate(SE_FinalTransitionFire, this.transform.position, this.transform.rotation);
                modeChange.transform.parent = this.gameObject.transform;
                isSpawningCircles = false;
            }
        }
        yield return null;

    }

    private IEnumerator PhaseBehaviour()
    {

        yield return null;
        int currMode = mode % 2;
	
	    yield return null;
	    if(isPhase1)
        {
            behaviourP1(currMode);
            yield return null;
        }        
	    else if(istransition1to2)
        {
            transition_behaviourP1toP2();
            yield return null;
        }        
	    else if(isPhase2)
        {
            behaviourP2(currMode);
            yield return null;
        }        
	    else if(istransition2to3)
        {
            transition_behaviourP2toP3();
            yield return null;
        }       
	    else if(isPhase3)
        {
            if (!hasDied)
            {
                behaviourP3(currMode);
            }
        }
        yield return null;
    }
    
    //Movement - either you're taking cover and shooting or it's walking towards player, essentially.
    //The differences in walking to player come in abilities used

    public void movementToCover()
    {
        Vector3 targetPostition = new Vector3(target.transform.position.x,
               target.transform.position.y + 0.4f,
               target.transform.position.z);
        spawnLoc.transform.LookAt(targetPostition);
        
        //OnEnable you generated list of possible cover locations. Move to one of them for a specific duration.
        distancePathX = this.transform.position.x - pathDestsVectors[destinationChoice].x;
        distancePathZ = this.transform.position.z - pathDestsVectors[destinationChoice].z;

        if (pathingCooldownTimer < 0.1f)
        {
            beginPathing();
        }
        //stop walk anim when you reach your destination        
        else if ((distancePathX > -2.5 && distancePathX < 2.5) && (distancePathZ > -2.5 && distancePathZ < 2.5))
        {

            this.transform.LookAt(targetPostition);
            agent.isStopped = true;

            int decision = Random.Range(0, 100);
            //proj & sometimes multi
            if (decision < 75 && cooldownTimer < 0.0f)
            {
                anim.SetTrigger("isPunching");
                float offset = Random.Range(-0.03f, 0.01f);
                Quaternion spawnRot = new Quaternion
                    (spawnLoc.transform.rotation.x + offset,
                    spawnLoc.transform.rotation.y + offset,
                    spawnLoc.transform.rotation.z + offset,
                    spawnLoc.transform.rotation.w);
                Instantiate(basicGunProj, spawnLoc.transform.position, spawnRot);
                cooldownTimer = 0.33f;
            }
            else if (decision >= 75 && specialCooldownTimer < 0.0f)
            {
                anim.SetTrigger("isPunching");
                Instantiate(multiGunProj, spawnLoc.transform.position, spawnLoc.transform.rotation);
                specialCooldownTimer = 5.0f;
                cooldownTimer = 3.0f;
            }

        }
        else
        {
            anim.SetTrigger("isRunning");
        }
    }

    public void beginPathing()
    {
        //choose random pathing loc from our list of destinations
        destinationChoice = UnityEngine.Random.Range(0, (pathDestsVectors.Count - 1));
        //navigate towards it. Use a cooldown so that the npc isn't constantly walking!
        if (agent.isActiveAndEnabled)
        {
            agent.ResetPath();
            agent.SetDestination(pathDestsVectors[destinationChoice]);
            this.transform.LookAt(pathDestsVectors[destinationChoice]);
        }
        pathingCooldownTimer = 15.0f;
    }

    public void movementToPlayer()
    {
        Vector3 targetPostition = new Vector3(target.transform.position.x,
               target.transform.position.y,
               target.transform.position.z);
        spawnLoc.transform.LookAt(targetPostition);
        this.transform.LookAt(targetPostition);
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);
       
    }




    //@param mode can be 0 or 1
    public void behaviourP1(int mode)
    {
        //PHASE 1 GUN & SHIELD
        if (mode == 0)
        {
            agent.speed = standardSpeed * 2.0f;
            //start in gun mode always
            gun.SetActive(true);
            shield.SetActive(true);
            Axe.SetActive(false);
            GS.SetActive(false);
            bossFlame.SetActive(false);
            bossLightning.SetActive(false);
            GreatswordFire.SetActive(false);
            //movement code taken care of first
            movementToCover();

            //attacking code taken care of once boss is actually at destination


        }
        //PHASE 1 GREATSWORD NO FIRE
        else
        {
            agent.speed = standardSpeed * 1.0f;
            //start in gun mode always
            gun.SetActive(false);
            shield.SetActive(false); //shield off p1; gs is 2handed for now.
            Axe.SetActive(false);
            GS.SetActive(true);
            bossFlame.SetActive(false);
            bossLightning.SetActive(false);
            GreatswordFire.SetActive(false);
            //movement code taken care of first
            movementToPlayer();


            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;

            int decision = Random.Range(0, 100);
            if(cooldownTimer < 0.0f && decision < 70)
            {
                if ((distanceX > -1.7 && distanceX < 1.7) && (distanceZ > -1.7 && distanceZ < 1.7) && cooldownTimer < 0.01f)
                {
                    //in melee range, attack
                    anim.SetTrigger("isSlashing");
                    //we are in range. Start reducing battery. Less if Player has shield!
                    DamageReduction(gsDamage);

                    cooldownTimer = 6.0f;
                    Instantiate(GSShot, spawnLoc.transform.position, this.transform.rotation);
                }
                else
                {
                    anim.SetTrigger("isRunning");
                }
            }
            if (specialCooldownTimer < 0.0f && decision >=70)
            {
                //power up
                anim.SetTrigger("isHealing");
                axeDamage += 2;
                gsDamage += 2;
                specialCooldownTimer = 25.0f;
                GameObject myPowerUp = Instantiate(SE_PowerUp, this.transform.position, this.transform.rotation);
                myPowerUp.transform.parent = this.gameObject.transform;
            }

        }

    }

    public void transition_behaviourP1toP2()
    {
        
        anim.SetTrigger("isKneeling");

        if (cooldownTimer < 0.0f)
        {
            agent.isStopped = false;
            agent.enabled = true;
            //remove cover, instantiate expl on ea
            foreach (Transform dest in myDests.transform)
            {
                Instantiate(SE_CoverExplosion, dest.position, dest.transform.rotation);
                Destroy(dest.gameObject);
            }

            GameObject godmode = Instantiate(SE_GodModeBoss, this.transform.position, this.transform.rotation);
            godmode.transform.parent = this.gameObject.transform;
            istransition1to2 = false;
            anim.SetTrigger("DoneTransition");
            isPhase2 = true;
            //small cd so it doesn't murder you right away
            cooldownTimer = 2.0f;
            specialCooldownTimer = 2.0f;
        }

    }

    public void behaviourP2(int mode)
    {
        //PHASE 2 AXE AND SHIELD
        if (mode == 0)
        {
            movementToPlayer();

            agent.speed = standardSpeed * 0.85f;
            
            gun.SetActive(false);
            shield.SetActive(true);
            Axe.SetActive(true);
            GS.SetActive(false);
            bossFlame.SetActive(false);
            bossLightning.SetActive(false);
            GreatswordFire.SetActive(false);

            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;

            int decision = Random.Range(0, 100);

            //MELEE DISTANCE
            if ((distanceX > -1.7 && distanceX < 1.7) && (distanceZ > -1.7 && distanceZ < 1.7) && cooldownTimer < 0.01f)
            {
                //normal melee hit
                if (decision > 40)
                {
                    anim.SetTrigger("isAxeHacking");
                    DamageReduction(axeDamage);

                    cooldownTimer = 4.0f;
                    Instantiate(AxeShot, spawnLoc.transform.position, this.transform.rotation);
                }
                //axe lightning multi
                else if (decision <= 40)
                {
                    float offsetX = Random.Range(-3.00f, 3.00f);
                    float offsetZ = Random.Range(-3.00f, 3.00f);
                    Instantiate(bossLightningMulti, target.transform.position, this.transform.rotation);
                    anim.SetTrigger("isLightning");
                    specialCooldownTimer = 7.0f;
                    cooldownTimer = 2.0f;
                }
               
            }
            //RANGED DISTANCE
            else
            {               
                anim.SetTrigger("isRunning");
                //summon
                if (decision < 20 && specialCooldownTimer < 0.01f)
                {
                    anim.SetTrigger("isHealing");
                    foreach (Vector3 minionSpawnPos in minionSpawnVectors)
                    {
                        Instantiate(summonedVampire, minionSpawnPos, this.gameObject.transform.rotation); //rotation doesn't matter for a spherical explosion :p
                        Instantiate(SE_RedExplosionSP, minionSpawnPos, this.gameObject.transform.rotation);
                    }
                    specialCooldownTimer = 12.0f;
                    cooldownTimer = 3.0f; //normal cd timer too so you get a break...
                }
                //single proj
                else if (decision >= 20 && decision < 75 && cooldownTimer < 0.01f)
                {
                    float offset = Random.Range(-0.2f, 0.1f);
                    Quaternion spawnRot = new Quaternion
                        (spawnLoc.transform.rotation.x + offset,
                        spawnLoc.transform.rotation.y + offset,
                        spawnLoc.transform.rotation.z + offset,
                        spawnLoc.transform.rotation.w);
                    Instantiate(LProjLastBoss, spawnLoc.transform.position, spawnRot);
                    anim.SetTrigger("isAxeHacking");
                    cooldownTimer = 0.8f;
                }
                //axe lightning multi - with offset so it doesn't immediately kill you
                else if (decision >= 75 && specialCooldownTimer < 0.01f)
                {
                    float offsetX = Random.Range(-3.00f, 3.00f);
                    float offsetZ = Random.Range(-3.00f, 3.00f);
                    Instantiate(bossLightningMulti, target.transform.position, this.transform.rotation);
                    anim.SetTrigger("isLightning");
                    specialCooldownTimer = 7.0f;
                    cooldownTimer = 2.0f;
                }
            }

            
        }
        //PHASE 2 GS, GS FLAME AND SHIELD
        else
        {
            movementToPlayer();


            agent.speed = standardSpeed * 1.35f;
            gun.SetActive(false);
            shield.SetActive(true);
            Axe.SetActive(false);
            GS.SetActive(true);
            bossFlame.SetActive(false);
            bossLightning.SetActive(false);
            GreatswordFire.SetActive(true);

            int decision = Random.Range(0, 100);
            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;

            //MELEE DISTANCE
            if ((distanceX > -1.7 && distanceX < 1.7) && (distanceZ > -1.7 && distanceZ < 1.7) && cooldownTimer < 0.01f)
            {
                if (decision < 75 && cooldownTimer < 0.01f)
                {
                    anim.SetTrigger("isSlashing");
                    DamageReduction(gsDamage);

                    cooldownTimer = 5.0f;
                    Instantiate(GSShot, spawnLoc.transform.position, this.transform.rotation);
                }
                else if (decision >= 75 && specialCooldownTimer < 0.01f)
                {
                    //power up
                    anim.SetTrigger("isHealing");
                    axeDamage += 2;
                    gsDamage += 2;
                    specialCooldownTimer = 25.0f;
                    GameObject myPowerUp = Instantiate(SE_PowerUp, this.transform.position, this.transform.rotation);
                    myPowerUp.transform.parent = this.gameObject.transform;

                }
                

            }
            //RANGED DISTANCE
            else
            {
                anim.SetTrigger("isRunning");
                //wizfire multi proj
                if (decision < 75 && cooldownTimer < 0.01f)
                {
                    anim.SetTrigger("isSlashing");
                    float offset = Random.Range(-0.15f, 0.08f);
                    Quaternion spawnRot = new Quaternion
                        (spawnLoc.transform.rotation.x + offset,
                        spawnLoc.transform.rotation.y + offset,
                        spawnLoc.transform.rotation.z + offset,
                        spawnLoc.transform.rotation.w);
                    Instantiate(WizShotFire, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    cooldownTimer = 7.0f;
                    
                }
                else if (decision >= 75 && specialCooldownTimer < 0.01f)
                {
                    //power up
                    anim.SetTrigger("isHealing");
                    axeDamage += 2;
                    gsDamage += 2;
                    specialCooldownTimer = 25.0f;
                    GameObject myPowerUp = Instantiate(SE_PowerUp, this.transform.position, this.transform.rotation);
                    myPowerUp.transform.parent = this.gameObject.transform;

                }
            }
        }

    }

    public void transition_behaviourP2toP3()
    {
        anim.SetTrigger("isKneeling");
        
        if (cooldownTimer < 0.0f)
        {
            agent.enabled = true;
            agent.isStopped = false;            
            anim.SetTrigger("isHealing");
            foreach (Vector3 minionSpawnPos in minionSpawnVectors)
            {
                //24 Because why not
                if (HeroControllerSP.hasSkull_GOLD)
                {
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation);
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation);
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation);
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation);
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation);
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation);

                }
                if (HeroControllerSP.hasSkull_SILVER)//silver. Spawn 12!!!
                {
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation); 
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation); 
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation); 
                }
                else if (HeroControllerSP.hasSkull_BRONZE) //bronze - 8 total...
                {
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation); 
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation); 
                }
                else //normal & bronze - summon 4 total
                {
                    Instantiate(summonedSummonerBoss, minionSpawnPos, this.gameObject.transform.rotation); 
                }
                
                Instantiate(SE_RedExplosionSP, minionSpawnPos, this.gameObject.transform.rotation);
            }
            agent.enabled = true;
            GameObject godmode = Instantiate(SE_GodModeBoss, this.transform.position, this.transform.rotation);
            godmode.transform.parent = this.gameObject.transform;
            istransition2to3 = false;
            anim.SetTrigger("DoneTransition");
            isPhase3 = true;

            cooldownTimer = 2.0f;
            specialCooldownTimer = 2.0f;
        }
    }


    public void behaviourP3(int mode) //extends p2
    {
        if (mode == 0 && !hasDied)
        {
            movementToPlayer();

            agent.speed = standardSpeed * 0.85f;
            gun.SetActive(false);
            shield.SetActive(true);
            Axe.SetActive(true);
            GS.SetActive(false);
            bossFlame.SetActive(false);
            bossLightning.SetActive(true);
            GreatswordFire.SetActive(false);

            int decision = Random.Range(0, 100);
            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;

            //MELEE DISTANCE
            if ((distanceX > -1.7 && distanceX < 1.7) && (distanceZ > -1.7 && distanceZ < 1.7) && cooldownTimer < 0.01f)
            {
                //normal melee hit
                if (decision > 40)
                {
                    anim.SetTrigger("isAxeHacking");
                    DamageReduction(axeDamage);

                    cooldownTimer = 2.0f;
                    Instantiate(AxeShot, spawnLoc.transform.position, this.transform.rotation);
                }
                //axe lightning multi
                else if (decision <= 40)
                {
                    float offsetX = Random.Range(-2.00f, 2.00f);
                    float offsetZ = Random.Range(-2.00f, 2.00f);
                    Instantiate(bossLightningMulti, target.transform.position, this.transform.rotation);
                    anim.SetTrigger("isLightning");
                    specialCooldownTimer = 5.0f;
                    cooldownTimer = 1.0f;
                }

            }
            //RANGED DISTANCE
            else if(!hasDied)
            {
                anim.SetTrigger("isRunning");
                //summon
                if (decision < 20 && specialCooldownTimer < 0.01f)
                {
                    anim.SetTrigger("isHealing");
                    foreach (Vector3 minionSpawnPos in minionSpawnVectors)
                    {
                        Instantiate(summonedVampire, minionSpawnPos, this.gameObject.transform.rotation); // x3
                        Instantiate(summonedVampire, minionSpawnPos, this.gameObject.transform.rotation);
                        Instantiate(summonedVampire, minionSpawnPos, this.gameObject.transform.rotation);
                        Instantiate(SE_RedExplosionSP, minionSpawnPos, this.gameObject.transform.rotation);//rotation doesn't matter for a spherical explosion :p
                    }
                    specialCooldownTimer = 12.0f;
                    cooldownTimer = 3.0f; //normal cd timer too so you get a break...
                }
                //single proj
                else if (decision >= 20 && decision < 75 && cooldownTimer < 0.01f)
                {
                    float offset = Random.Range(-0.2f, 0.1f);
                    Quaternion spawnRot = new Quaternion
                        (spawnLoc.transform.rotation.x + offset,
                        spawnLoc.transform.rotation.y + offset,
                        spawnLoc.transform.rotation.z + offset,
                        spawnLoc.transform.rotation.w);
                    Instantiate(LProjLastBoss, spawnLoc.transform.position, spawnRot);
                    anim.SetTrigger("isAxeHacking");
                    cooldownTimer = 0.30f;
                }
                //axe lightning multi - with offset so it doesn't immediately kill you
                else if (decision >= 75 && specialCooldownTimer < 0.01f)
                {
                    float offsetX = Random.Range(-2.00f, 2.00f);
                    float offsetZ = Random.Range(-2.00f, 2.00f);
                    Instantiate(bossLightningMulti, target.transform.position, this.transform.rotation);
                    anim.SetTrigger("isLightning");
                    specialCooldownTimer = 5.0f;
                    cooldownTimer = 1.0f;
                }
            }

        }
        else
        {
            agent.speed = standardSpeed * 1.35f;
            
            gun.SetActive(false);
            shield.SetActive(true);
            Axe.SetActive(false);
            GS.SetActive(true);
            bossFlame.SetActive(true);
            bossLightning.SetActive(false);
            GreatswordFire.SetActive(true);
            
            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;
            int decision = Random.Range(0, 100);

            //spawning circles - use special cd timer to track when it's over, use normal cd timer to spawn fire circles
            if (isSpawningCircles)
            {
                if (specialCooldownTimer < 0.00f || behaviourModeTimer < 0.5f)//stop if mode change or special cd is up.
                {
                    isSpawningCircles = false;
                    anim.SetTrigger("DoneTransition"); //bug - ensure it doesn't get stuck in kneeling pose...
                }
                if (cooldownTimer < 0.01f)
                {
                    int randomOffset1 = Random.Range(-25, 25);
                    int randomOffset2 = Random.Range(-25, 25);
                    Vector3 wellPos = new Vector3(this.transform.position.x + randomOffset1, this.transform.position.y - 0.4f, this.transform.position.z + randomOffset2);
                    Instantiate(FinalSlamAttackWell, wellPos, this.transform.rotation);
                    cooldownTimer = 2.2f;
                }
            }
            //normal behaviour
            else
            {                
                movementToPlayer();

                //MELEE DISTANCE
                if ((distanceX > -1.7 && distanceX < 1.7) && (distanceZ > -1.7 && distanceZ < 1.7) && cooldownTimer < 0.01f)
                {
                    if (decision < 75 && cooldownTimer < 0.01f)
                    {
                        anim.SetTrigger("isSlashing");
                        DamageReduction(gsDamage);
                        Debug.Log(gsDamage);

                        cooldownTimer = 3.0f;
                        Instantiate(GSShot, spawnLoc.transform.position, this.transform.rotation);
                    }
                    else if (decision >= 75 && specialCooldownTimer < 0.01f)
                    {
                        //power up
                        anim.SetTrigger("isHealing");
                        axeDamage += 2;
                        gsDamage += 2;
                        specialCooldownTimer = 15.0f;
                        GameObject myPowerUp = Instantiate(SE_PowerUp, this.transform.position, this.transform.rotation);
                        myPowerUp.transform.parent = this.gameObject.transform;

                    }


                }
                //RANGED DISTANCE
                else
                {
                    anim.SetTrigger("isRunning");
                    //wizfire multi proj
                    if (decision < 60 && cooldownTimer < 0.01f)
                    {
                        anim.SetTrigger("isSlashing");
                        float offset = Random.Range(-0.15f, 0.08f);
                        Quaternion spawnRot = new Quaternion
                            (spawnLoc.transform.rotation.x + offset,
                            spawnLoc.transform.rotation.y + offset,
                            spawnLoc.transform.rotation.z + offset,
                            spawnLoc.transform.rotation.w);
                        Instantiate(WizShotFire, spawnLoc.transform.position, spawnLoc.transform.rotation);
                        cooldownTimer = 5.5f;

                    }
                    else if ((decision >= 60 && decision < 85) && specialCooldownTimer < 0.01f)
                    {
                        //power up
                        anim.SetTrigger("isHealing");
                        axeDamage += 2;
                        gsDamage += 2;
                        specialCooldownTimer = 15.0f;
                        GameObject myPowerUp = Instantiate(SE_PowerUp, this.transform.position, this.transform.rotation);
                        myPowerUp.transform.parent = this.gameObject.transform;

                    }
                    else if (decision >= 85 && specialCooldownTimer < 0.01f)
                    {
                        //Spawn minotaur style red circles
                        isSpawningCircles = true;
                        specialCooldownTimer = 18.0f;
                        anim.SetTrigger("isHealing");
                        anim.SetTrigger("isHealing");
                        
                        GameObject slamSE = Instantiate(SE_FinalBigSlamAttack, this.transform.position, this.transform.rotation);
                        slamSE.transform.parent = this.gameObject.transform;
                    }
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
            int damageTaken = 0;

            damageTaken = edr.collisionDmgTaken(other, "NORMAL", myArmour);
            bossHealth -= damageTaken;

            //consume playershot
            Destroy(other.gameObject);


        }

        if (bossHealth < (maxBossHealth * 0.75f) && !hasDoneTransition1to2)
        {
            hasDoneTransition1to2 = true;
            istransition1to2 = true;
            isPhase1 = false;
            cooldownTimer = 11.0f;
            Instantiate(SE_FinalBossTransitionLoot, this.transform.position, this.transform.rotation);
        }
        if (bossHealth < (maxBossHealth * 0.40f) && !hasDoneTransition2to3)
        {
            hasDoneTransition2to3 = true;
            istransition2to3 = true;
            isPhase2 = false;
            cooldownTimer = 11.0f;
            Instantiate(SE_FinalBossTransitionLoot, this.transform.position, this.transform.rotation);
        }
        if (bossHealth < 1 && !hasDied)
        {
            death();
        }

    }

    private IEnumerator UpdateSlider()
    {
        yield return null;
        bossHealthSlider.value = bossHealth;
        yield return null;
    }

    private void DamageReduction(int attack)
    {
        DamageReductionScript dmgReduct = new DamageReductionScript();
        //when attacking player, enemy may take thorns damage
        int thornsDmgReturned = dmgReduct.DamageReduction(attack);
        if (thornsDmgReturned > 0)
        {
            Quaternion PlainRot = new Quaternion(90, 0, 0, 90);
            Vector3 plainHeight = new Vector3(target.transform.position.x, target.transform.position.y + 0.5f, target.transform.position.z);
            GameObject thorns = Instantiate(SE_Thorns, plainHeight, PlainRot) as GameObject;
            thorns.transform.SetParent(target.transform);
            bossHealth -= thornsDmgReturned;
        }


        if (bossHealth <= 0 && !hasDied)
        {
            death();
        }
    }

    public void death()
    {

        gun.SetActive(false);
        shield.SetActive(false);
        Axe.SetActive(false);
        GS.SetActive(false);
        bossFlame.SetActive(false);
        bossLightning.SetActive(false);
        GreatswordFire.SetActive(false);


        GAMEMANAGERSP.numScore += 500;
        anim.SetTrigger("isDead");
        agent.enabled = false;
        SelfDestructSP sd = this.gameObject.AddComponent<SelfDestructSP>() as SelfDestructSP;
        sd.lifeSpan = 10.0f;
        Instantiate(SE_FinalBossDeath, this.transform.position, this.transform.rotation);

        hasDied = true;

        //delete all enemies in field
        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.name.Contains("VampireSummonedFinal") || obj.name.Contains("SummonerBossFinal"))
            {
                Instantiate(SE_CoverExplosion, obj.transform.position, obj.transform.rotation);
                Destroy(obj.gameObject);
            }
        }


        portalToEnding.SetActive(true);
        


    }

}