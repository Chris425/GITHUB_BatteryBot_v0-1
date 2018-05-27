using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.PostProcessing;

public class HeroController : MonoBehaviour
{
    public bool isPostProcEnabled;
    private Animator anim;
    private CharacterController controller;

    private GameObject bb;
    private GameObject PlayerBase;

    public float speed = 12.0F;
    public float jumpSpeed = 12.0F;
    public float gravity = 27.0F;
    private Vector3 moveDirection = Vector3.zero;
    public static int currScene;
    public static bool isPaused;
    private bool hasDoubleJumped;
    private bool isDoubleJumping;
    private float jumpTimer;
    private bool hasLeaped;
    private bool isLeaping;
    private float leapTimer;

    public Camera normalCam;
    public Camera farCam;
    public Camera firstPersCam;
    private int cameraCounter = 0;

    public GameObject respawnLoc;

    public GameObject SE_hit;
    public GameObject SE_Heal;
    public GameObject SE_basic_hit;
    public GameObject SE_hit_ice;
    public GameObject SE_hit_poison;
    public GameObject SE_GodMode;
    public GameObject SE_GroundHit1;
    public GameObject SE_GroundHit2;
    public GameObject SE_GroundHit3;


    public ParticleSystem IdleGasStream;
    public ParticleSystem WorkingGasStream;
    public GameObject GreatswordFire;


    //fall damage
    private float peakHeight;
    private float landingHeight;
    private float currHeight;
    private float fallThreshold = 12.5f;


    //status effects
    public static bool isPoisoned;
    private float poisonCooldownTimer;
    private float coldCooldownTimer;
    public static int poisonTicks = 10;
    private bool isDead = false;

    //inventory
    public GameObject Helm;
    public GameObject Armour;
    public GameObject RightBoot;
    public GameObject LeftBoot;
    public GameObject RightLegArmour;
    public GameObject LeftLegArmour;
    public GameObject shield;
    public GameObject axe;
    public GameObject gun;
    public GameObject bazooka;
    public GameObject GS;
    public GameObject jetBooster;
    public GameObject JetBooster_ARCANE;
    public GameObject GSShot;
    public GameObject GSShot_FIRE;
    public GameObject AxeShot;
    public GameObject AxeShot_LIGHTNING;
    public GameObject ShieldShot_ICE;
    public GameObject BoosterSE;
    public static bool hasLegs;
    public static bool hasBoots;
    public static bool hasHelm;
    public static bool hasArmour;
    public static bool hasShield;
    public static bool hasJetBooster;
    public static bool hasJetBooster_ARCANE;
    public static bool hasAxe;
    public static bool hasGun;
    public static bool hasBazooka;
    public static bool hasSilver;
    public static bool hasGS;
    public static bool hasGS_FIRE;
    public static bool hasAxe_LIGHTNING;
    public static bool hasShield_ICE;
    public static bool hasGun_MULTI;
    public static bool hasGun_POISON;
    public static bool hasGun_COLD;
    public static bool hasGun_FIRE;
    public static bool hasNPCBot;

    public static bool isSlot1 = false;
    public static bool isSlot2 = false;
    public static bool isSlot3 = false;
    public static bool isSlot4 = false;
    public static bool isSlot5 = false;

    //keys
    public static bool hasSkull_RED;
    public static bool hasSkull_PURPLE;
    public static bool hasSkull_BLUE;
    public static bool hasSkull_BRONZE;
    public static bool hasSkull_SILVER;
    public static bool hasSkull_GOLD;

    public static bool isSuperCharged = false;

    public GameObject HeroBeingControlled; //put the entity that is being controlled in this variable

    public Light ExtraLight;
    public Light GreenLight;
    public Light YellowLight;
    public Light RedLight;


    public GameObject warning;
    public GameObject SE_coldEffect;
    public GameObject warningCritical;
    public GameObject SE_DoubleJump;
    public GameObject SE_PlayerLeap;

    //gears & lives
    public static int Gears;
    public Text gearsValText;
    public Text respawnText;
    public Text livesValText;
    public Text scoreValText;
    public Text qualValText;


    //gun ammo
    public static int Ammo;
    public static int Lives;
    public Text ammoValText;
    public GameObject objToSpawn;//ctrl
    public GameObject objToSpawnSilver;//ctrl
    public GameObject objToSpawnGold; //ctrl
    public GameObject objToSpawnBlue; //shift
    public GameObject objToSpawnBlueGold;//shift
    public GameObject GunShot_MULTI;
    public GameObject GunShot_POISON;
    public GameObject GunShot_COLD;
    public GameObject GunShot_FIRE;
    public GameObject interactShot;
    public GameObject spawnLoc;
    //public float cooldown = 2.5f;    //deprecated as they didnt even work anyways. cooldown was 1 despite not setting it to 1??
    //public float specialCooldown = 6.5f;
    private float dashCooldown = 14.8f;
    private float cooldownTimer;
    private float interactTimer;
    private float arenaDrainTimer;
    private float dashCooldownTimer;
    private float doubleJumpTimer;
    private float batteryDrainCD = 2.0f;


    bool isBoosting = false;
    public bool isCold = false;

    //Battery life
    public static int battery = 100;
    public Text batteryValText;

    public Slider batterySlider;
    public Image Fill;  // assign in the editor the "Fill"

    //Pause Screen
    public Canvas pauseScreen;
    public Slider masterVolume;


    private GameObject bbMetalSurface;
    private Renderer rend;
    public Material bbGlacier;
    public Material bbMetal;


    //UI Inventory Elements
    public List<Texture> invSlots = new List<Texture>();
    public List<RawImage> emptyInvSlots = new List<RawImage>();



    private GameObject coldEffects;


    void Awake()
    {
        batteryEffects();
        Application.targetFrameRate = 60;
        String scene = SceneManager.GetActiveScene().name;
        //soooooo level 1 has terrible performance, removing vsync for it...
        if (scene == "LevelOne")
        {
            QualitySettings.vSyncCount = 0;
        }
        else
        {
            QualitySettings.vSyncCount = 1;
        }

    }

    public void pauseScreen_ExitButton()
    {
        //Just save and exit for now
        //Unpause first!
        Time.timeScale = 1.0f;
        isPaused = false;
        GetComponent<MouseLookSP>().enabled = true;
        pauseScreen.enabled = false;
        
        GAMEMANAGERSP.saveArenaScoreToFILE();
        SceneManager.LoadScene("Intro");
    }
    public void pauseScreen_MasterVolumeSlider()
    {
        //AudioListener.volume should be 0.0-1.0f; but if you go higher it works, but it just gets really distorted
        //Slider defaults to 100. Allowing up to 100 for now...
        AudioListener.volume = masterVolume.value / 100.0f;
    }

    //Change quality settings (same as when you first start the game)
    //Also, post-processing kicks in for levels 3,4,5. 0,1,2 do not have it.
    //note that applyExpensiveChanges = false; it will apply on next scene load...
    public void pauseScreen_GraphicsBoost()
    {
        int currQual = QualitySettings.GetQualityLevel();
        if (currQual < 5)
        {
            //play gearGet sound
            currQual += 1;
            QualitySettings.SetQualityLevel(currQual, true);
        }
        else
        {
            //play error sound
        }

        setQualityText();
    }

    public void pauseScreen_GraphicsDrop()
    {
        int currQual = QualitySettings.GetQualityLevel();
        if (currQual > 0)
        {
            //play gearGet sound
            currQual -= 1;
            QualitySettings.SetQualityLevel(currQual, false);
        }
        else
        {
            //play error sound.
        }
        //remember player choice for next time
        setQualityText();
    }

    public void addPostProcessing()
    {
        isPostProcEnabled = true;
        GameObject[] gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
        for (int i = 0; i < gos.Length; i++)
            if (gos[i].name.Contains("Main Camera"))
            {
                gos[i].GetComponent<PostProcessingBehaviour>().enabled = true;
            }
    }

    public void removePostProcessing()
    {
        isPostProcEnabled = false;
        GameObject[] gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
        for (int i = 0; i < gos.Length; i++)
            if (gos[i].name.Contains("Main Camera"))
            {
                gos[i].GetComponent<PostProcessingBehaviour>().enabled = false;
            }
    }

    public void setQualityText()
    {
        int qual = QualitySettings.GetQualityLevel();
        switch (qual)
        {
            case 0:
                qualValText.text = "Fastest";
                removePostProcessing();
                break;
            case 1:
                qualValText.text = "Fast";
                removePostProcessing();
                break;
            case 2:
                qualValText.text = "Simple";
                removePostProcessing();
                break;
            case 3:
                qualValText.text = "Good";
                addPostProcessing();
                break;
            case 4:
                qualValText.text = "Beautiful";
                addPostProcessing();
                break;
            case 5:
                qualValText.text = "Fantastic";
                addPostProcessing();
                break;
            default:
                Debug.Log("ERROR");
                break;
        }

    }


    void Update()
    {

        //need to put this here so that it is decoupled from other code. Hopefully won't affect performance too much
        if ((Input.GetKeyDown("p") && !isPaused) || (Input.GetKeyDown(KeyCode.Escape) && !isPaused))
        {
            Time.timeScale = 0.0f;
            isPaused = true;
            GetComponent<MouseLookSP>().enabled = false;
            pauseScreen.enabled = true;
        }
        else if ((Input.GetKeyDown("p") && isPaused) || (Input.GetKeyDown(KeyCode.Escape) && isPaused))
        {
            Time.timeScale = 1.0f;
            isPaused = false;
            GetComponent<MouseLookSP>().enabled = true;
            pauseScreen.enabled = false;
        }

        if (!isPaused)
        {
            //all turned to coroutines because 60 updates per second is too much
            //and it's massively affecting performance...
            StartCoroutine("HeroMoveUpdate");
            if (!isDead)
            {
                ArenaBatteryDrain();
                StartCoroutine("TimerUpdate");
                StartCoroutine("UpdateSlider");
                batteryEffects();
                StartCoroutine("checkEquipment");
                StartCoroutine("checkStatusEffects");
            }

        }

    }

    private void ArenaBatteryDrain()
    {
        if (arenaDrainTimer < 0.0f)
        {
            battery -= 1;
            GAMEMANAGERSP.numArenaScore += 1;
            arenaDrainTimer = batteryDrainCD;
        }
    }

    private IEnumerator checkStatusEffects()
    {
        yield return new WaitForSeconds(1.7f);
        //Poison
        if (isPoisoned && poisonCooldownTimer <= 0.01f)
        {
            //Instatiate poison effect
            GameObject PoisonDOT = Instantiate(SE_hit_poison, this.transform.position, this.transform.rotation);
            PoisonDOT.transform.parent = this.gameObject.transform;
            battery -= 1;
            poisonTicks -= 1;
            poisonCooldownTimer = 3.0f;
        }

        if (poisonTicks <= 0)
        {
            isPoisoned = false;
            poisonTicks = 10;
        }

        //Cold
        if (isCold && coldCooldownTimer <= 0.01f)
        {
            isCold = false;
            Destroy(coldEffects);
            if (rend != null)
            {
                rend.material = bbMetal;
            }
        }
        yield return new WaitForSeconds(1.9f);
    }

    private IEnumerator UpdateSlider()
    {
        yield return new WaitForSeconds(1.5f);
        batterySlider.value = battery;
        if (isSuperCharged)
        {
            Fill.color = Color.white;
        }
        else
        {
            if (battery > 100 && battery < 133)
            {
                Fill.color = Color.Lerp(Color.green, Color.white, 0.20f);
            }
            else if (battery >= 133 && battery < 166)
            {
                Fill.color = Color.Lerp(Color.green, Color.white, 0.40f);
            }
            else if (battery >= 166 && battery < 200)
            {
                Fill.color = Color.Lerp(Color.green, Color.white, 0.60f);
            }
            else
            {
                Fill.color = Color.green;
            }



        }

        yield return new WaitForSeconds(1.5f);
    }

    //this needs to be in start as it doesn't work on OnEnable.
    //called when we come to a scene from a gameover, or finish a level.
    public void Start()
    {
        try
        {          
            setQualityText();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }

    }

    public void OnEnable()
    {
        isDead = false;
        pauseScreen.enabled = false;

        arenaDrainTimer = batteryDrainCD;

        controller = GetComponent<CharacterController>();

        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        GreatswordFire.SetActive(false);


        normalCam.gameObject.SetActive(true);
        farCam.gameObject.SetActive(false);
        firstPersCam.gameObject.SetActive(false);


        isPaused = false;
        LeftBoot.SetActive(false); RightBoot.SetActive(false);
        LeftLegArmour.SetActive(false); RightLegArmour.SetActive(false);
        Helm.SetActive(false);
        Armour.SetActive(false);
        gun.SetActive(false);
        bazooka.SetActive(false);
        shield.SetActive(false);
        GS.SetActive(false);
        axe.SetActive(false);
        jetBooster.SetActive(false);


        isPoisoned = false;
        isCold = false;

        battery = 100;
        speed = 15.0f;
        Ammo = 0;
        //Gears = 0; //gears currency will now persist across levels and through death. CDC 02-05-2017

        hasBoots = false;
        hasLegs = false;
        hasHelm = false;
        hasArmour = false;
        hasShield = false;
        hasAxe = false;
        hasGun = false;
        hasGS = false;
        hasGS_FIRE = false;
        hasAxe_LIGHTNING = false;
        hasGun_MULTI = false;
        hasGun_POISON = false;
        hasGun_COLD = false;
        hasGun_FIRE = false;
        hasShield_ICE = false;
        hasJetBooster = false;
        hasJetBooster_ARCANE = false;
        hasNPCBot = false;

        hasSkull_RED = false;
        hasSkull_BLUE = false;
        hasSkull_PURPLE = false;

        batteryValText.text = "" + battery + " %";
        ammoValText.text = "" + Ammo;
        livesValText.text = "" + Lives;
        gearsValText.text = "" + Gears;
        scoreValText.text = "SCORE: " + GAMEMANAGERSP.numArenaScore; //will only exist for session, doesn't matter

        batterySlider.value = 100;


        PlayerBase = GameObject.Find("PLAYERBASE");
        //find beta_surface and assign it to our variable
        bb = GameObject.Find("BatteryBot");

        foreach (Transform child in bb.transform)
        {
            if (child.gameObject.name.Contains("Beta_Surface"))
            {
                rend = child.gameObject.GetComponent<Renderer>();
                bbMetalSurface = child.gameObject;
            }
        }

        batteryEffects();

        //GameObject myCanvas = GameObject.Find("Canvas");
        //emptyInvSlots = myCanvas.GetComponentsInChildren<RawImage>();
    }

    public void handleCameras()
    {

        if (cameraCounter % 3 == 0)
        {
            normalCam.gameObject.SetActive(true);
            farCam.gameObject.SetActive(false);
            firstPersCam.gameObject.SetActive(false);
        }
        else if (cameraCounter % 3 == 1)
        {
            normalCam.gameObject.SetActive(false);
            farCam.gameObject.SetActive(true);
            firstPersCam.gameObject.SetActive(false);
        }
        else
        {
            normalCam.gameObject.SetActive(false);
            farCam.gameObject.SetActive(false);
            firstPersCam.gameObject.SetActive(true);
        }
        //remember graphics choice...
        if (isPostProcEnabled)
        {
            addPostProcessing();
        }
        else
        {
            removePostProcessing();
        }
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.name.Contains("EXPLODER"))
        {
            // Instantiate(SE_basic_hit, this.transform.position, this.transform.rotation);
            battery -= 3;
        }
        //Destroy(other);

    }

    public void OnCollisionEnter(Collision other)
    {
        if (!isDead)
        {
            bool wasHitByEnemy = false;
            int currBattery = HeroController.battery;
            //only call this when you're hit
            StartCoroutine("UpdateSlider");
            batteryEffects();
            //case when the player is hit by the caster
            if (other.gameObject.name.Contains("CasterShot"))
            {
                wasHitByEnemy = true;
                Instantiate(SE_hit, this.transform.position, this.transform.rotation);
                if (!hasArmour)
                {
                    battery -= 8;
                }
                else
                {
                    battery -= 12;
                }

                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("CasterTurretShot"))
            {
                wasHitByEnemy = true;
                Instantiate(SE_hit, this.transform.position, this.transform.rotation);
                if (!hasArmour)
                {
                    battery -= 6;
                }
                else
                {
                    battery -= 2;
                }
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("WizBasic"))
            {
                wasHitByEnemy = true;
                Instantiate(SE_hit, this.transform.position, this.transform.rotation);
                if (!hasArmour)
                {
                    battery -= 5;
                }
                else
                {
                    battery -= 2;
                }
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("palaGround"))
            {
                wasHitByEnemy = true;
                Instantiate(SE_basic_hit, this.transform.position, this.transform.rotation);
                if (!hasArmour)
                {
                    battery -= 3;
                }
                else
                {
                    battery -= 1;
                }
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("SummonerShot") || other.gameObject.name.Contains("WizardShot"))
            {
                wasHitByEnemy = true;
                Instantiate(SE_hit_ice, this.transform.position, this.transform.rotation);
                if (!hasArmour)
                {
                    battery -= 5;
                }
                else
                {
                    battery -= 2;
                }
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.ToUpper().Contains("WIZARDFIRE"))
            {
                wasHitByEnemy = true;
                Instantiate(SE_hit, this.transform.position, this.transform.rotation);
                if (!hasArmour)
                {
                    battery -= 15;
                }
                else
                {
                    battery -= 10;
                }
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("wizardPoison"))
            {
                wasHitByEnemy = true;
                //make new SE
                Instantiate(SE_hit, this.transform.position, this.transform.rotation);
                battery -= 2;
                //status effect of poison
                isPoisoned = true;
                //refresh number of ticks
                poisonTicks = 10;

                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("PoisonWellProjectiles"))
            {
                Instantiate(SE_hit_poison, this.transform.position, this.transform.rotation);
                if (!hasArmour)
                {
                    battery -= 3;
                }
                else
                {
                    battery -= 1;
                }
                //status effect of poison
                isPoisoned = true;
                //refresh number of ticks
                poisonTicks = 5;

            }
            else if (other.gameObject.name.Contains("GroundSlamProjectiles"))
            {
                Instantiate(SE_hit, this.transform.position, this.transform.rotation);
                battery -= 2;

            }
            else if (other.gameObject.name.Contains("SafetyDance"))
            {
                wasHitByEnemy = true;
                Instantiate(SE_hit, this.transform.position, this.transform.rotation);
                if (!hasArmour)
                {
                    battery -= 25;
                }
                else
                {
                    battery -= 15;
                }
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("lvl3BossSlowProj"))
            {
                wasHitByEnemy = true;
                //Only freeze the player if theyre not already frozen
                if (!isCold)
                {
                    isCold = true;
                    coldCooldownTimer = 7.0f;
                    Instantiate(warning, this.transform.position, this.transform.rotation);
                    coldEffects = Instantiate(SE_coldEffect, this.transform.position, this.transform.rotation);
                    coldEffects.transform.parent = this.gameObject.transform;
                }
                //if you are already cold, simply refresh the duration but do not remake the cold effects prefab
                else
                {
                    coldCooldownTimer = 7.0f;
                    Instantiate(warning, this.transform.position, this.transform.rotation);
                }

                //deal damage
                Instantiate(SE_hit, this.transform.position, this.transform.rotation);
                if (!hasArmour)
                {
                    battery -= 2;
                }
                else
                {
                    battery -= 1;
                }
                Destroy(other.gameObject);
                //make the player's texture change.
                if (rend != null)
                {
                    rend.material = bbGlacier;
                }
            }

            if (wasHitByEnemy) { makeHurtSE(); }

        }

    }

    public void death()
    {
        //ensure it doesn't happen many times during one death
        if (!isDead)
        {
            //se_groundhit3 is sort of the critical hit effect. Add robot voice to it
            Instantiate(SE_GroundHit3, this.transform.position, this.transform.rotation);
            Instantiate(SE_GroundHit2, this.transform.position, this.transform.rotation);
            Instantiate(SE_GroundHit1, this.transform.position, this.transform.rotation);
            
            deathAndRebirth(true);
            currScene = SceneManager.GetActiveScene().buildIndex;
            batterySlider.value = 0;
            batteryValText.text = "ERROR";
            //put r to respawn text and then disable it when reloading scene (onawake)?
            if (Lives < 0)
            {
                Lives = 0;
                GAMEMANAGERSP.saveArenaScoreToFILE();

                //go back to overworld on gameover instead of current scene
                //currScene = SceneManager.GetSceneByName("ExplorationOverworld").buildIndex; 
                currScene = 2; //HARDCODED CAUSE YOU'RE BAD CDC

                SceneManager.LoadSceneAsync("Gameover");
            }
        }
    }

    //disable mouselook and character controller if died, else enable
    public void deathAndRebirth(bool hasDied)
    {
        if (hasDied)
        {
            GetComponent<MouseLookSP>().enabled = false;
            controller.enabled = false;
            anim.SetTrigger("isDead");
            respawnText.text = "Press \"R\" to respawn...";
            isDead = true;
            Lives -= 1;
            battery = 100; //put this to 100 before saving it...
            GAMEMANAGERSP.saveArenaScoreToFILE();
        }
        else
        {
            GetComponent<MouseLookSP>().enabled = true;
            controller.enabled = true;
            respawnText.text = "";
            isDead = false;
            anim.SetTrigger("rebirth");
            battery = 100;
            this.gameObject.transform.position = respawnLoc.transform.position;
        }   
        
    }

    //soooooooo since melee enemies hurt you in their script this will only happen on range projectiles, yolo. fix later? -CDC
    public void makeHurtSE()
    {
        int randomNum = UnityEngine.Random.Range(0, 20);
        if (randomNum <= 3) { Instantiate(SE_GroundHit1, this.transform.position, this.transform.rotation); }
        else if (randomNum >= 5 && randomNum <= 10) { Instantiate(SE_GroundHit2, this.transform.position, this.transform.rotation); }
        else if (randomNum >= 12 && randomNum <= 14) { Instantiate(SE_GroundHit3, this.transform.position, this.transform.rotation); }
    }

    //CDC this will change as new stuff is added
    private IEnumerator checkEquipment()
    {
        yield return new WaitForSeconds(2.0f);
        //ACTIVE EQUIP
        if (GAMEMANAGERSP.hasBazooka)
        {
            if (hasAxe && isSlot1)
            {
                axe.SetActive(true);
                bazooka.SetActive(false);
                GS.SetActive(false);
                gun.SetActive(false);
            }
            if (hasGun && isSlot2)
            {
                bazooka.SetActive(true);
                axe.SetActive(false);
                GS.SetActive(false);
                gun.SetActive(false);
            }
            if (hasGS && isSlot3)
            {
                GS.SetActive(true);
                bazooka.SetActive(false);
                axe.SetActive(false);
                gun.SetActive(false);
            }
        }
        else
        {
            if (hasAxe && isSlot1)
            {
                axe.SetActive(true);
                gun.SetActive(false);
                GS.SetActive(false);
            }
            if (hasGun && isSlot2)
            {
                gun.SetActive(true);
                axe.SetActive(false);
                GS.SetActive(false);
            }
            if (hasGS && isSlot3)
            {
                GS.SetActive(true);
                gun.SetActive(false);
                axe.SetActive(false);
            }
        }

        //cdc may change if new left handed stuff implemented
        if (hasShield && !isSlot5)
        {
            shield.SetActive(true);
            jetBooster.SetActive(false);
        }
        if (hasJetBooster && !isSlot4)
        {
            shield.SetActive(false);
            jetBooster.SetActive(true);
            IdleGasStream.gameObject.SetActive(true);
        }

        if (hasArmour)
        {
            //armour will reduce damage
            Armour.SetActive(true);
        }
        if (hasHelm)
        {
            //Helm wings increases jump speed
            Helm.SetActive(true);
            jumpSpeed = 18.0f;
        }
        if (hasBoots)
        {
            //Winged boots allow for double jumps and midair thrust, see movement update method
            LeftBoot.SetActive(true);
            RightBoot.SetActive(true);
        }
        if (hasLegs)
        {
            //legs provide defense and deal a small amount of thorns damage back to enemy per attack.
            LeftLegArmour.SetActive(true);
            RightLegArmour.SetActive(true);
        }

        //CHANGE INVENTORY UI ICONS
        if (hasAxe) { emptyInvSlots[0].texture = invSlots[0]; }
        else { emptyInvSlots[0].texture = invSlots[30]; } //Bring it back to empty.
        if (hasGun && hasBazooka) { emptyInvSlots[1].texture = invSlots[23]; }
        else if (hasGun) { emptyInvSlots[1].texture = invSlots[1]; }
        else { emptyInvSlots[1].texture = invSlots[30]; }

        if (hasGS) { emptyInvSlots[2].texture = invSlots[2]; }
        else { emptyInvSlots[2].texture = invSlots[30]; }
        if (hasShield) { emptyInvSlots[3].texture = invSlots[3]; }
        else { emptyInvSlots[3].texture = invSlots[30]; }
        if (hasJetBooster) { emptyInvSlots[4].texture = invSlots[4]; }
        else { emptyInvSlots[4].texture = invSlots[30]; }
        if (hasAxe_LIGHTNING) { emptyInvSlots[5].texture = invSlots[5]; }
        else { emptyInvSlots[5].texture = invSlots[30]; }
        if (hasGun_MULTI) { emptyInvSlots[6].texture = invSlots[6]; }  //make passive glow effect for gun ? - cdc    
        else if (hasGun_POISON) { emptyInvSlots[6].texture = invSlots[17]; }
        else if (hasGun_COLD) { emptyInvSlots[6].texture = invSlots[18]; }
        else if (hasGun_FIRE) { emptyInvSlots[6].texture = invSlots[19]; }
        else { emptyInvSlots[6].texture = invSlots[30]; }
        if (hasGS_FIRE) { GreatswordFire.SetActive(true); emptyInvSlots[7].texture = invSlots[7]; }
        else { emptyInvSlots[7].texture = invSlots[30]; }
        if (hasShield_ICE) { emptyInvSlots[8].texture = invSlots[8]; } //make passive frost effect for shield ? - cdc
        else { emptyInvSlots[8].texture = invSlots[30]; }
        if (hasJetBooster_ARCANE) { emptyInvSlots[9].texture = invSlots[9]; } //make passive arcane effect for booster? - cdc
        else { emptyInvSlots[9].texture = invSlots[30]; }
        if (hasSkull_BRONZE || hasSkull_SILVER || hasSkull_GOLD)
        {
            if (hasSkull_GOLD) { emptyInvSlots[10].texture = invSlots[22]; emptyInvSlots[11].texture = invSlots[22]; emptyInvSlots[12].texture = invSlots[22]; }
            else if (hasSkull_SILVER) { emptyInvSlots[10].texture = invSlots[21]; emptyInvSlots[11].texture = invSlots[21]; emptyInvSlots[12].texture = invSlots[21]; }
            else if (hasSkull_BRONZE) { emptyInvSlots[10].texture = invSlots[20]; emptyInvSlots[11].texture = invSlots[20]; emptyInvSlots[12].texture = invSlots[20]; }


        }
        else
        {
            //Don't consider old skulls if metal skulls present
            if (hasSkull_BLUE) { emptyInvSlots[10].texture = invSlots[10]; }
            else { emptyInvSlots[10].texture = invSlots[30]; }
            if (hasSkull_PURPLE) { emptyInvSlots[11].texture = invSlots[11]; }
            else { emptyInvSlots[11].texture = invSlots[30]; }
            if (hasSkull_RED) { emptyInvSlots[12].texture = invSlots[12]; }
            else { emptyInvSlots[12].texture = invSlots[30]; }
        }
        if (hasNPCBot) { emptyInvSlots[13].texture = invSlots[13]; }
        else { emptyInvSlots[13].texture = invSlots[30]; }

        //cooldowns and debuffs
        //cdc change the position of these boxes if you do buffs, debuffs and cooldown rows?
        if (isPoisoned)
        {
            emptyInvSlots[16].texture = invSlots[16];
        }
        else
        {
            emptyInvSlots[16].texture = invSlots[29]; //go back to clear box
        }

        if (cooldownTimer > 0.01f)
        {
            emptyInvSlots[14].texture = invSlots[14];
        }
        else
        {
            emptyInvSlots[14].texture = invSlots[29];
        }

        if (dashCooldownTimer > 0.01f)
        {
            emptyInvSlots[15].texture = invSlots[15];
        }
        else
        {
            emptyInvSlots[15].texture = invSlots[29];
        }

        yield return new WaitForSeconds(1.5f);
    }
    private IEnumerator TimerUpdate()
    {
        yield return new WaitForSeconds(1.7f);
        //let's increment the timers using a co-routine to prevent it from occurring too often
        // * time.deltatime ensures it happens at the same rate regardless of your computer's processing power.
        cooldownTimer -= 1.3f * Time.deltaTime;
        dashCooldownTimer -= 1.4f * Time.deltaTime;
        poisonCooldownTimer -= 1.3f * Time.deltaTime;
        coldCooldownTimer -= 1.3f * Time.deltaTime;
        interactTimer -= 1.3f * Time.deltaTime;
        arenaDrainTimer -= 1.3f * Time.deltaTime;


        if (battery >= 200)
        {
            speed = 15.0f;
            isSuperCharged = true;
            ExtraLight.intensity = 3.0f;
            GreenLight.intensity = 6.0f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 0.0f;
        }
        else if (battery > 100 && battery < 199)
        {
            speed = 12.0f;
            ExtraLight.intensity = 0.0f;
            GreenLight.intensity = 6.0f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 0.0f;
            isSuperCharged = false;
        }
        else
        {
            isSuperCharged = false;
        }

        //On death, show death anim and show text to respawn.
        if (battery < -1)
        {
            death();
        }

        yield return new WaitForSeconds(1.7f);
    }

    //There are 3 lights - green, yellow, and red. The intensity will be increased/decreased depending on your battery life!
    public void batteryEffects()
    {

        if (battery >= 90 && battery <= 100)
        {
            speed = 11.0f;

            ExtraLight.intensity = 0.0f;
            GreenLight.intensity = 8.0f; // 8 is max light intensity
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 0.0f;

        }
        else if (battery >= 80 && battery <= 90)
        {
            speed = 10.5f;


            GreenLight.intensity = 4.5f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 0.0f;

        }
        else if (battery > 70 && battery <= 80)
        {
            speed = 10.0f;

            GreenLight.intensity = 2.0f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 0.0f;

        }
        else if (battery >= 60 && battery <= 70)
        {
            speed = 9f;

            GreenLight.intensity = 0.0f;
            YellowLight.intensity = 3.0f;
            RedLight.intensity = 0.0f;


        }
        else if (battery >= 50 && battery <= 60)
        {
            speed = 8.5f;

            GreenLight.intensity = 0.0f;
            YellowLight.intensity = 2.0f;
            RedLight.intensity = 0.0f;

        }
        else if (battery > 40 && battery <= 50)
        {
            speed = 8.0f;

            GreenLight.intensity = 0.0f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 7.0f;


        }
        else if (battery >= 30 && battery <= 40)
        {
            speed = 7.5f;

            GreenLight.intensity = 0.0f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 3.0f;



        }
        else if (battery > 20 && battery <= 30)
        {
            speed = 6.5f;

            GreenLight.intensity = 0.0f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 2.0f;


        }
        else if (battery >= 10 && battery <= 20)
        {
            speed = 4.5f;

            GreenLight.intensity = 0.0f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 0.5f;


        }
        else if (battery > 0 && battery <= 10)
        {
            speed = 2.5f;

            GreenLight.intensity = 0.0f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 0.0f;

        }

        //boost only
        if (isBoosting)
        {
            speed = speed += 20.0f;
            IdleGasStream.gameObject.SetActive(false);
        }
        //cold only
        if (isCold)
        {
            speed = 1.75f;
            //cold AND boosting
            if (isBoosting)
            {
                speed = speed += 3.0f;
                IdleGasStream.gameObject.SetActive(false);
            }
        }


    }


    private IEnumerator HeroMoveUpdate()
    {
        if (!isDead)
        {
            if (doubleJumpTimer > 0.00f)
            {
                doubleJumpTimer -= 0.1f;
            }
            //update text fields
            batteryValText.text = "" + battery + " %";
            ammoValText.text = "" + Ammo;
            livesValText.text = "" + Lives;
            gearsValText.text = "" + Gears;
            scoreValText.text = "SCORE: " + GAMEMANAGERSP.numArenaScore;

            yield return null;

            // is the controller on the ground?
            if (controller.isGrounded)
            {
                //reset double jump when on the ground. 

                hasDoubleJumped = false;
                isDoubleJumping = false;
                jumpTimer = 2.0f;


                //reset charge when on the ground.

                hasLeaped = false;
                isLeaping = false;
                leapTimer = 2.0f;


                currHeight = PlayerBase.transform.localPosition.y;

                if (peakHeight >= fallThreshold)
                {
                    //this means you just landed
                    float delta = peakHeight - currHeight;
                    float damageDone = (delta - 12.5f) * 4.3f;
                    if (damageDone > 0.0f && !hasHelm)
                    {
                        HeroController.battery -= (int)damageDone;
                        int randomNum = UnityEngine.Random.Range(0, 10);
                        if (randomNum <= 3) { Instantiate(SE_GroundHit1, this.transform.position, this.transform.rotation); }
                        else if (randomNum >= 6) { Instantiate(SE_GroundHit2, this.transform.position, this.transform.rotation); }
                        else { Instantiate(SE_GroundHit3, this.transform.position, this.transform.rotation); }
                    }

                }
                peakHeight = 0.0f;

                //Feed moveDirection with input.
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                //Multiply it by speed.
                moveDirection *= speed;

                //Jumping set to A is actually fire1
                //Change this back to "Jump"
                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                    anim.SetTrigger("isJumping");

                    doubleJumpTimer = 2.50f;

                }

                //movement
                if (Input.GetKey("w") && speed > 7.5f && !isCold)
                {
                    anim.SetTrigger("isRunning");
                }
                else if (Input.GetKey("w") && (speed <= 7.5f || isCold)) //when you're cold from a projectile you'll do the walk animation.
                {
                    anim.SetTrigger("isWalking");
                }
                else if (Input.GetKey("a"))
                {
                    anim.SetTrigger("strafeLeft");
                }
                else if (Input.GetKey("d"))
                {
                    anim.SetTrigger("strafeRight");
                }
                else if (Input.GetKey("s"))
                {
                    anim.SetTrigger("backwards");
                }

            }
            else
            {
                peakHeight = currHeight;

                //Double jump implementation - only occur once midair, and if you have boots upgrade
                //getButtonDown feels bad but all options suck!
                if (Input.GetButtonDown("Jump") && hasDoubleJumped == false && hasBoots && doubleJumpTimer < 0.1f)
                {
                    isDoubleJumping = true;
                    hasDoubleJumped = true;
                    anim.SetTrigger("isJumping");
                    Instantiate(SE_DoubleJump, spawnLoc.transform.position, this.transform.rotation);
                }

                if (isDoubleJumping && jumpTimer > 0.0f && doubleJumpTimer < 0.1f)
                {
                    moveDirection.y = jumpSpeed;
                    //use deltaTime to ensure it's the same for all systems. Time.deltaTime is a small number < 1 
                    jumpTimer -= 15.0f * Time.deltaTime;

                }


                //Midair Leap implementation
                if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && hasLeaped == false && hasBoots)
                {
                    isLeaping = true;
                    hasLeaped = true;
                    anim.SetTrigger("isLeaping");
                    GameObject smoke = Instantiate(SE_PlayerLeap, spawnLoc.transform.position, this.transform.rotation);
                    smoke.transform.parent = this.gameObject.transform;
                }

                if (isLeaping && leapTimer > 0.0f)
                {

                    float leapDirection = this.transform.eulerAngles.y;
                    Debug.Log(leapDirection);
                    //Let's approximate to one of 8 possible directions
                    //Then dial in the exact number
                    //North 90
                    if (leapDirection >= 67.5 && leapDirection <= 112.5)
                    {
                        Vector3 chargeDirection = new Vector3(0.5f, 0.15f, 0);
                        controller.Move(chargeDirection);

                    }
                    //NorthEast 135
                    else if (leapDirection >= 112.5 && leapDirection <= 157.5)
                    {
                        Vector3 chargeDirection = new Vector3(0.5f, 0.15f, -0.5f);
                        controller.Move(chargeDirection);

                    }
                    //East 180
                    else if (leapDirection >= 157.5 && leapDirection <= 202.5)
                    {
                        Vector3 chargeDirection = new Vector3(0, 0.15f, -0.5f);
                        controller.Move(chargeDirection);

                    }
                    //SouthEast 225
                    else if (leapDirection >= 202.5 && leapDirection <= 247.5)
                    {
                        Vector3 chargeDirection = new Vector3(-0.5f, 0.15f, -0.5f);
                        controller.Move(chargeDirection);

                    }
                    //South 270
                    else if (leapDirection >= 247.5 && leapDirection <= 292.5)
                    {
                        Vector3 chargeDirection = new Vector3(-0.5f, 0.15f, 0);
                        controller.Move(chargeDirection);

                    }
                    //SouthWest 315
                    else if (leapDirection >= 292.5 && leapDirection <= 337.5)
                    {
                        Vector3 chargeDirection = new Vector3(-0.5f, 0.15f, 0.5f);
                        controller.Move(chargeDirection);

                    }
                    //West 360
                    else if ((leapDirection >= 337.5 && leapDirection <= 360) || (leapDirection >= 0 && leapDirection <= 22.5)) //tricky ;)
                    {
                        Vector3 chargeDirection = new Vector3(0, 0.15f, 0.5f);
                        controller.Move(chargeDirection);

                    }
                    //NorthWest 45
                    else if (leapDirection >= 22.5 && leapDirection <= 67.5)
                    {
                        Vector3 chargeDirection = new Vector3(0.5f, 0.15f, 0.5f);
                        controller.Move(chargeDirection);

                    }
                    else
                    {
                        Debug.Log("PROBLEM WITH LEAP...");
                    }


                    //in either case decrement timer
                    leapTimer -= 5.0f * Time.deltaTime;

                }


            }
            //CDC this will change when new weps are added
            if (Input.GetKey("1"))
            {
                isSlot1 = true;
                isSlot2 = false; isSlot3 = false; /*isSlot4 = false; isSlot5 = false;*/
                if (hasBazooka)
                { bazooka.SetActive(false); }
                else { gun.SetActive(false); }
                GS.SetActive(false);
            }
            else if (Input.GetKey("2"))
            {
                isSlot2 = true;
                isSlot1 = false; isSlot3 = false; /*isSlot4 = false; isSlot5 = false;*/
                axe.SetActive(false);
                GS.SetActive(false);
            }
            else if (Input.GetKey("3"))
            {
                isSlot3 = true;
                isSlot1 = false; isSlot2 = false; /*isSlot4 = false; isSlot5 = false;*/
                axe.SetActive(false);
                if (hasBazooka)
                { bazooka.SetActive(false); }
                else { gun.SetActive(false); }
            }
            else if (Input.GetKey("4"))
            {
                isSlot4 = true;
                /*isSlot1 = false; isSlot2 = false; isSlot3 = false;*/
                isSlot5 = false;
            }
            else if (Input.GetKey("5"))
            {
                isSlot5 = true;
                /*isSlot1 = false; isSlot2 = false; isSlot3 = false;*/
                isSlot4 = false;
            }


            //Camera
            if (Input.GetKeyDown("-"))
            {
                cameraCounter += 1;
                handleCameras();
            }

            //gunshot normal
            if (Input.GetButton("Fire1") && Ammo > 0 && cooldownTimer < 0.01f && hasGun && isSlot2)
            {
                if (!hasSilver && !hasBazooka)
                {
                    anim.SetTrigger("isPunching");
                    Instantiate(objToSpawn, spawnLoc.transform.position, this.transform.rotation);
                    cooldownTimer = 0.3f; //cooldown
                    Ammo -= 1;
                }
                else if (hasSilver && !hasBazooka)
                {
                    anim.SetTrigger("isPunching");
                    Instantiate(objToSpawnSilver, spawnLoc.transform.position, this.transform.rotation);
                    cooldownTimer = 0.5f; //cooldown
                }
                else if (hasBazooka)
                {
                    anim.SetTrigger("isPunching");
                    Instantiate(objToSpawnGold, spawnLoc.transform.position, this.transform.rotation);
                    cooldownTimer = 0.6f; //cooldown
                }

            }
            else if (Input.GetButton("Fire1") && cooldownTimer < 0.01f && hasGS && isSlot3)
            {
                anim.SetTrigger("isSlashing");
                Instantiate(GSShot, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 0.85f;

            }
            else if (Input.GetButton("Fire1") && cooldownTimer < 0.01f && hasAxe && isSlot1)
            {
                anim.SetTrigger("isAxeHacking");
                Instantiate(AxeShot, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 0.5f;
            }

            //blue shot
            if (Input.GetButton("Fire2") && Ammo > 3 && cooldownTimer < 0.01f && hasGun && isSlot2 && (hasGun_MULTI || hasGun_COLD || hasGun_POISON || hasGun_FIRE))
            {
                if (!hasSilver && !hasBazooka)
                {
                    //all gun upgrades allow strong blue shots using shift.                              
                    anim.SetTrigger("isPunching");
                    Instantiate(objToSpawnBlue, spawnLoc.transform.position, this.transform.rotation);

                    cooldownTimer = 0.35f;
                    Ammo -= 4;
                }
                else if (hasSilver && !hasBazooka)
                {
                    anim.SetTrigger("isPunching");
                    Instantiate(objToSpawnBlue, spawnLoc.transform.position, this.transform.rotation);

                    cooldownTimer = 0.35f;
                    Ammo -= 4;
                }
                else if (hasBazooka)
                {
                    anim.SetTrigger("isPunching");
                    Instantiate(objToSpawnBlueGold, spawnLoc.transform.position, this.transform.rotation);

                    cooldownTimer = 1.0f;
                }

            }

            yield return null;

            //SPECIAL ATTACKS - X
            if (Input.GetButton("Fire3") && cooldownTimer < 0.01f && hasAxe && hasAxe_LIGHTNING && isSlot1)
            {
                anim.SetTrigger("isLightning");
                Instantiate(AxeShot_LIGHTNING, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 1.5f;
            }
            else if (Input.GetButton("Fire3") && cooldownTimer < 0.01f && hasGS && hasGS_FIRE && isSlot3)
            {
                anim.SetTrigger("isSlashing");
                Instantiate(GSShot_FIRE, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 1.8f;
            }
            else if (Input.GetButton("Fire3") && cooldownTimer < 0.01f && hasGun && isSlot2)
            {
                if (hasGun_MULTI)
                {
                    anim.SetTrigger("isPunching");
                    Instantiate(GunShot_MULTI, spawnLoc.transform.position, this.transform.rotation);
                    cooldownTimer = 1.5f;
                    // Ammo -= 1;  // special uses no ammo!
                }
                else if (hasGun_COLD)
                {
                    anim.SetTrigger("isPunching");
                    Instantiate(GunShot_COLD, spawnLoc.transform.position, this.transform.rotation);
                    cooldownTimer = 4.3f;
                }
                else if (hasGun_POISON)
                {
                    anim.SetTrigger("isPunching");
                    Instantiate(GunShot_POISON, spawnLoc.transform.position, this.transform.rotation);
                    cooldownTimer = 3.2f;
                }
                else if (hasGun_FIRE)
                {
                    anim.SetTrigger("isPunching");
                    GameObject flame = Instantiate(GunShot_FIRE, spawnLoc.transform.position, this.transform.rotation);
                    flame.transform.parent = this.gameObject.transform;
                    cooldownTimer = 3.0f;
                }

            }

            //note that offhands will use C to differentiate
            if (Input.GetButton("Fire4") && cooldownTimer < 0.01f && hasShield && hasShield_ICE && isSlot4)
            {
                anim.SetTrigger("isShieldBashing");
                Instantiate(ShieldShot_ICE, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 6.5f;
            }

            if (Input.GetButton("Fire4") && cooldownTimer < 0.01f && hasJetBooster && hasJetBooster_ARCANE && isSlot5)
            {
                anim.SetTrigger("isShieldBashing");
                Instantiate(JetBooster_ARCANE, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 2.0f;
            }

            //heal ability
            if (Input.GetButton("Heal") && cooldownTimer < 0.01f && Ammo >= 50)
            {
                anim.SetTrigger("isHealing");
                Ammo -= 50;
                cooldownTimer = 4.5f;
                if (battery > 74) { battery = 100; }
                else { battery += 20; }
                GameObject myHeal = Instantiate(SE_Heal, this.transform.position, this.transform.rotation);
                myHeal.transform.parent = this.gameObject.transform;
            }

            yield return null;
            //interact button
            if (Input.GetButton("Interact") && interactTimer < 0.01f)
            {
                anim.SetTrigger("isPunching");

                Instantiate(interactShot, spawnLoc.transform.position, this.transform.rotation);
                interactTimer = 0.5f;
            }
            if (Input.GetButton("Fire5") && cooldownTimer < 0.01f && hasShield && isSlot4)
            {
                anim.SetTrigger("isShieldBashing");
                //shield bash = same damage as an axe!
                Instantiate(AxeShot, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 1.0f;
            }
            if (Input.GetButton("Fire5") && dashCooldownTimer < 0.01f && hasJetBooster && isSlot5)
            {
                anim.SetTrigger("isShieldBashing");
                Instantiate(GSShot, spawnLoc.transform.position, this.transform.rotation); //does damage as you move forward with booster
                Instantiate(BoosterSE, spawnLoc.transform.position, this.transform.rotation);
                dashCooldownTimer = dashCooldown;
                isBoosting = true;
                WorkingGasStream.gameObject.SetActive(true);
                IdleGasStream.gameObject.SetActive(false);
            }

            //stop boosting after a while
            if (dashCooldownTimer < 5.0f)
            {
                isBoosting = false;
                WorkingGasStream.gameObject.SetActive(false);
                IdleGasStream.gameObject.SetActive(true);
            }



            //Applying gravity to the controller
            moveDirection.y -= gravity * Time.deltaTime;
            //Making the character move
            controller.Move(moveDirection * Time.deltaTime);



            //RESET GAME
            if ((Input.GetKey(KeyCode.LeftShift) == true || Input.GetKey(KeyCode.RightShift) == true) && Input.GetKey(KeyCode.R) == true)
            {
                int currScene = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currScene);

            }
            //DEV OPTIONS
            // ; + r resets back to intro
            // ; + n brings you to level n
            if (Input.GetKey(KeyCode.Semicolon) == true)
            {
                if (Input.GetKey(KeyCode.R)) { GAMEMANAGERSP.shouldMakeMusicBot = true; SceneManager.LoadScene("Intro"); }
                if (Input.GetKey(KeyCode.O)) { SceneManager.LoadScene("ExplorationOverworld"); }
                if (Input.GetKey(KeyCode.Alpha1)) { SceneManager.LoadScene("LevelOne"); }
                if (Input.GetKey(KeyCode.Alpha2)) { SceneManager.LoadScene("LevelTwo"); }
                if (Input.GetKey(KeyCode.Alpha3)) { SceneManager.LoadScene("LevelThree"); }
                if (Input.GetKey(KeyCode.Alpha4)) { SceneManager.LoadScene("LevelFour"); }
                if (Input.GetKey(KeyCode.Alpha5)) { SceneManager.LoadScene("FinalBossBattle"); }
                if (Input.GetKey(KeyCode.Alpha6)) { SceneManager.LoadScene("Ending_3Bronze"); }
                if (Input.GetKey(KeyCode.Alpha7)) { SceneManager.LoadScene("Ending_2Silver"); }
                if (Input.GetKey(KeyCode.Alpha8)) { SceneManager.LoadScene("Ending_1Gold"); }
                if (Input.GetKey(KeyCode.B)) { SceneManager.LoadScene("LevelBonus"); }
                //GOD MODES 
                if (Input.GetKey(KeyCode.A)) { Ammo += 20; }
                if (Input.GetKey(KeyCode.H)) { battery += 5; }
                if (Input.GetKey(KeyCode.DownArrow)) { battery -= 5; }
                if (Input.GetKey(KeyCode.G) && cooldownTimer < 0.01f)
                {
                    cooldownTimer = 0.5f;
                    enableGodMode();
                    GameObject godmode = Instantiate(SE_GodMode, this.transform.position, this.transform.rotation);
                    godmode.transform.parent = this.gameObject.transform;
                }
                //TEMPORARY CODE CDC
                if (Input.GetKey(KeyCode.PageUp)) { hasGun_MULTI = false; hasGun_COLD = true; hasGun_POISON = false; hasGun_FIRE = false; checkEquipment(); }
                if (Input.GetKey(KeyCode.PageDown)) { hasGun_MULTI = false; hasGun_COLD = false; hasGun_POISON = true; hasGun_FIRE = false; checkEquipment(); }
                if (Input.GetKey(KeyCode.Pause)) { hasGun_MULTI = false; hasGun_COLD = false; hasGun_POISON = false; hasGun_FIRE = true; checkEquipment(); }

                if (Input.GetKey(KeyCode.N))
                {
                    enableNormalMode();
                }

                if (Input.GetKey(KeyCode.KeypadPlus) && cooldownTimer < 0.01f)
                {
                    MouseLookSP.increaseSensitivity(0.3f);
                    Debug.Log("X is  " + MouseLookSP.getSensitivityX().ToString() + "Y is " + MouseLookSP.getSensitivityY().ToString());
                    cooldownTimer = 0.2f;
                }
                if (Input.GetKey(KeyCode.KeypadMinus) && cooldownTimer < 0.01f)
                {
                    MouseLookSP.decreaseSensitivity(0.3f);
                    Debug.Log("X is  " + MouseLookSP.getSensitivityX().ToString() + "Y is " + MouseLookSP.getSensitivityY().ToString());
                    cooldownTimer = 0.2f;
                }

            }
            yield return null;
        }
        else if (isDead)
        {
            if (Input.GetKey(KeyCode.R) || Input.GetButton("Submit"))
            {                
                deathAndRebirth(false);
            }
        }


    }



    private void enableGodMode()
    {


        isPoisoned = false;
        isCold = false;


        battery = 999;
        Ammo = 999;
        Gears = 999;
        hasBoots = true;
        hasLegs = true;
        hasHelm = true;
        hasArmour = true;
        hasShield = true;
        hasAxe = true;
        hasGun = true;
        hasBazooka = true;
        hasBazooka = true;
        hasSilver = false;
        hasSilver = false;
        hasGS = true;
        hasGS_FIRE = true;
        hasAxe_LIGHTNING = true;
        hasGun_MULTI = true;
        hasShield_ICE = true;
        hasJetBooster = true;
        hasJetBooster_ARCANE = true;
        hasSkull_GOLD = true;

        HeroController.isSlot2 = true;
        HeroController.isSlot4 = true;
        HeroController.isSlot1 = false;
        HeroController.isSlot3 = false;
        HeroController.isSlot5 = false;

        Lives = 99;
        
        //no saving if you do godmode here in arena, cheater

        StartCoroutine("UpdateSlider");
        //StartCoroutine("batteryEffects");
        batteryEffects();

        StartCoroutine("checkEquipment");


    }

    private void enableNormalMode()
    {

        Lives = 5;
        isPoisoned = false;
        isCold = false;


        battery = 100;
        Ammo = 100;
        Gears = 100;
        hasBoots = false;
        LeftBoot.SetActive(false); RightBoot.SetActive(false);
        hasLegs = false;
        LeftLegArmour.SetActive(false); RightLegArmour.SetActive(false);
        hasHelm = false;
        Helm.SetActive(false);
        hasArmour = false;
        Armour.SetActive(false);
        hasShield = false;
        hasAxe = false;
        hasGun = true;
        hasBazooka = false;
        hasBazooka = false;
        hasSilver = false;
        hasSilver = false;
        hasGS = false;
        hasGS_FIRE = false;
        hasAxe_LIGHTNING = false;
        hasGun_MULTI = false;
        hasShield_ICE = false;
        hasJetBooster = false;
        hasJetBooster_ARCANE = false;
        hasSkull_GOLD = false;


        HeroController.isSlot2 = true;
        HeroController.isSlot4 = false;
        HeroController.isSlot1 = false;
        HeroController.isSlot3 = false;
        HeroController.isSlot5 = false;
        
        StartCoroutine("UpdateSlider");
        batteryEffects();
        StartCoroutine("checkEquipment");
    }

}
