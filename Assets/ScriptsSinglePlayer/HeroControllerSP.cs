using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class HeroControllerSP : MonoBehaviour
{

    private Animator anim;

    public float speed = 12.0F;
    public float jumpSpeed = 12.0F;
    public float gravity = 27.0F;
    private Vector3 moveDirection = Vector3.zero;
    public static int currScene;

    public GameObject SE_hit;
    public GameObject SE_basic_hit;
    public GameObject SE_hit_ice;
    public GameObject SE_hit_poison;

    public ParticleSystem IdleGasStream;
    public ParticleSystem WorkingGasStream;
    public GameObject GreatswordFire;
    public GameObject AxeLightning;

    //status effects
    public static bool isPoisoned;
    private float poisonCooldownTimer;
    public static int poisonTicks = 10;

    //inventory
    public GameObject shield;
    public GameObject axe;
    public GameObject gun;
    public GameObject GS;
    public GameObject jetBooster;
    public GameObject JetBooster_ARCANE;
    public GameObject GSShot;
    public GameObject GSShot_FIRE;
    public GameObject AxeShot;
    public GameObject AxeShot_LIGHTNING;
    public GameObject ShieldShot_ICE;
    public GameObject BoosterSE;
    public static bool hasShield;
    public static bool hasJetBooster;
    public static bool hasJetBooster_ARCANE;
    public static bool hasAxe;
    public static bool hasGun;
    public static bool hasGS;
    public static bool hasGS_FIRE;
    public static bool hasAxe_LIGHTNING;
    public static bool hasShield_ICE;
    public static bool hasGun_MULTI;
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

    public static bool isSuperCharged = false;

    public GameObject HeroBeingControlled; //put the entity that is being controlled in this variable

    public Light ExtraLight;
    public Light GreenLight;
    public Light YellowLight;
    public Light RedLight;


    public GameObject warning;
    public GameObject warningCritical;
    private bool isPlayingSound = false; //I goofed. Here's a hack to fix it

    //gears
    public static int Gears;
    public Text gearsValText;


    //gun ammo

    public static int Ammo;
    public Text ammoValText;
    public GameObject objToSpawn;
    public GameObject GunShot_MULTI;
    public GameObject interactShot;
    public GameObject spawnLoc;
    //public float cooldown = 2.5f;    //deprecated as they didnt even work anyways. cooldown was 1 despite not setting it to 1??
    //public float specialCooldown = 6.5f;
    private float dashCooldown = 14.8f;
    private float cooldownTimer;
    private float interactTimer;
    private float dashCooldownTimer;
    
    bool isBoosting = false;

    //Battery life
    public static int battery = 100;
    public Text batteryValText;

    public Slider batterySlider;
    public Image Fill;  // assign in the editor the "Fill"
    

    //UI Inventory Elements
    public List<Texture> invSlots = new List<Texture>();
    public List<RawImage> emptyInvSlots = new List<RawImage>();




    void Update()
    {
        HeroMoveUpdate();
        TimerUpdate();
        UpdateSlider();
        checkEquipment();
        checkStatusEffects();
    }

    private void checkStatusEffects()
    {
        if (isPoisoned && poisonCooldownTimer <= 0.01f)
        {
            //Instatiate poison effect
            Instantiate(SE_hit_poison, this.transform.position, this.transform.rotation);
            battery -= 1;
            poisonTicks -= 1;
            poisonCooldownTimer = 3.0f;
        }

        if (poisonTicks <= 0)
        {
            isPoisoned = false;
            poisonTicks = 10;
        }
    }

    private void UpdateSlider()
    {
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
            else  if (battery >= 166 && battery < 200)
            {
                Fill.color = Color.Lerp(Color.green, Color.white, 0.60f);
            }
            else
            {
                Fill.color = Color.green;
            }
      
            
         
        }
    }


    public void OnEnable()
    {
        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        GreatswordFire.SetActive(false);
        AxeLightning.SetActive(false);

        gun.SetActive(false);
        shield.SetActive(false);
        GS.SetActive(false);
        axe.SetActive(false);
        jetBooster.SetActive(false);

        isPoisoned = false;

        battery = 100;
        speed = 15.0f;
        Ammo = 990;
        //Gears = 0; //gears currency will now persist across levels and through death. CDC 02-05-2017

        hasShield = true;
        hasAxe = true;
        hasGun = true;
        hasGS = true;
        hasGS_FIRE = true;
        hasAxe_LIGHTNING = true;
        hasGun_MULTI = true;
        hasShield_ICE = true;
        hasJetBooster = true;
        hasJetBooster_ARCANE = true;
        hasNPCBot = true;

        hasSkull_RED = true;
        hasSkull_BLUE = true;
        hasSkull_PURPLE = true;

        batteryValText.text = "" + battery + " %";
        ammoValText.text = "" + Ammo;
        gearsValText.text = "" + Gears;

        batterySlider.value = 100;

        //GameObject myCanvas = GameObject.Find("Canvas");
        //emptyInvSlots = myCanvas.GetComponentsInChildren<RawImage>();
    }


    public void OnCollisionEnter(Collision other)
    {
        //case when the player is hit by the caster
        if (other.gameObject.name.Contains("CasterShot"))
        {
            Instantiate(SE_hit, this.transform.position, this.transform.rotation);
            battery -= 12;
            Destroy(other.gameObject);
        }
        if (other.gameObject.name.Contains("CasterTurretShot"))
        {
            Instantiate(SE_hit, this.transform.position, this.transform.rotation);
            battery -= 6;
            Destroy(other.gameObject);
        }
        if (other.gameObject.name.Contains("WizBasic"))
        {
            Instantiate(SE_hit, this.transform.position, this.transform.rotation);
            battery -= 5;
            Destroy(other.gameObject);
        }
        if (other.gameObject.name.Contains("palaGround"))
        {
            Instantiate(SE_basic_hit, this.transform.position, this.transform.rotation); 
            battery -= 2;
            Destroy(other.gameObject);
        }
        if (other.gameObject.name.Contains("SummonerShot") || other.gameObject.name.Contains("WizardShot"))
        {
            Instantiate(SE_hit_ice, this.transform.position, this.transform.rotation);
            battery -= 5;
            Destroy(other.gameObject);
        }
        if (other.gameObject.name.Contains("wizardFire") )
        {
            Instantiate(SE_hit, this.transform.position, this.transform.rotation);
            battery -= 15;
            Destroy(other.gameObject);
        }
        if (other.gameObject.name.Contains("wizardPoison"))
        {
            //make new SE
            Instantiate(SE_hit, this.transform.position, this.transform.rotation);
            battery -= 2;
            //status effect of poison
            isPoisoned = true;
            //refresh number of ticks
            poisonTicks = 10;

            Destroy(other.gameObject);
        }
        if (other.gameObject.name.Contains("PoisonWellProjectiles"))
        {
            Instantiate(SE_hit_poison, this.transform.position, this.transform.rotation);
            battery -= 3;
            //status effect of poison
            isPoisoned = true;
            //refresh number of ticks
            poisonTicks = 5;
            
        }
        if (other.gameObject.name.Contains("GroundSlamProjectiles"))
        {
            Instantiate(SE_hit, this.transform.position, this.transform.rotation);
            battery -= 2;

        }
        


    }
    
    //CDC this will change as new stuff is added
    void checkEquipment()
    {
        //ACTIVE EQUIP
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

        //CHANGE INVENTORY UI ICONS
        if (hasAxe)
        {
            emptyInvSlots[0].texture = invSlots[0];
        }
        if (hasGun)
        {
            emptyInvSlots[1].texture = invSlots[1];
        }
        if (hasGS)
        {
            emptyInvSlots[2].texture = invSlots[2];
        }
        if (hasShield)
        {
            emptyInvSlots[3].texture = invSlots[3];
        }
        if (hasJetBooster)
        {
            emptyInvSlots[4].texture = invSlots[4];
        }
        if (hasAxe_LIGHTNING)
        {
            AxeLightning.SetActive(true);
            emptyInvSlots[5].texture = invSlots[5];
        }
        if (hasGun_MULTI)
        {
            //make passive glow effect for gun ? - cdc
            emptyInvSlots[6].texture = invSlots[6];
        }
        if (hasGS_FIRE)
        {           
            GreatswordFire.SetActive(true);
            emptyInvSlots[7].texture = invSlots[7];
        }        
        if (hasShield_ICE)
        {
            //make passive frost effect for shield ? - cdc
            emptyInvSlots[8].texture = invSlots[8];
        }
        if (hasJetBooster_ARCANE)
        {
            //make passive arcane effect for booster? - cdc
            emptyInvSlots[9].texture = invSlots[9];
        }
        if (hasSkull_BLUE)
        {
            emptyInvSlots[10].texture = invSlots[10];
        }
        if (hasSkull_PURPLE)
        {
            emptyInvSlots[11].texture = invSlots[11];
        }
        if (hasSkull_RED)
        {
            emptyInvSlots[12].texture = invSlots[12];
        }
        if (hasNPCBot)
        {
            emptyInvSlots[13].texture = invSlots[13];
        }

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

    }
    void TimerUpdate()
    {

        //let's increment the timers every update call
        cooldownTimer -= 0.03f;
        dashCooldownTimer -= 0.04f;
        poisonCooldownTimer -= 0.03f;
        interactTimer -= 0.03f;



        if (battery >= 200)
        {
            speed = 15.0f;
            isSuperCharged = true;
            ExtraLight.intensity = 8.0f;
            GreenLight.intensity = 8.0f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 0.0f;
            isPlayingSound = false;
        }
        else if (battery > 100 && battery < 199)
        {
            speed = 12.0f;
            ExtraLight.intensity = 0.0f;
            GreenLight.intensity = 8.0f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 0.0f;
            isSuperCharged = false;
        }
        

        //as the battery decreases we'll make subtle changes to the way the bot moves and looks
        if (battery % 5 == 0)
        {
            batteryEffects();
        }

        //GAME OVER if battery is empty (0)
        //or less than 0 (vampire bites may bring it below 0!)
        if (battery <= 0)
        {
            currScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("GameoverSP");
        }
    }

    //Every 10% battery loss, lose 10% speed
    //There are 3 lights - green, yellow, and red. The intensity will be increased/decreased depending on your battery life!
    void batteryEffects()
    {
       
        switch (battery)
        {
            
            case 100:
                speed = 11.0f;

                ExtraLight.intensity = 0.0f;
                GreenLight.intensity = 8.0f; // 8 is max light intensity
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;

                break;

            case 95:
                speed = 11.0f;


                GreenLight.intensity = 7.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 90:
                speed = 10.5f;


                GreenLight.intensity = 4.5f; 
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 85:
                speed = 10.5f;

                GreenLight.intensity = 3.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 80:
                speed = 10.0f;

                GreenLight.intensity = 2.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 75:
                speed = 10.0f;
                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 8.0f;
                RedLight.intensity = 0.0f;
                //sound the first warning alarm!
                if (!isPlayingSound)
                {
                    Instantiate(warning, this.transform.position, this.transform.rotation);
                    isPlayingSound = true;
                }


                break;

            case 70:
                speed = 9.5f;
                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 7.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 65:
                speed = 9.5f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 4.5f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 60:
                speed = 9f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 3.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 55:
                speed = 9.0f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 2.5f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 50:
                speed = 8.5f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 2.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 45:
                speed = 8.5f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 8.0f;
                //Red light, turn on warning again
                if (!isPlayingSound)
                {
                    Instantiate(warning, this.transform.position, this.transform.rotation);
                    isPlayingSound = true;
                }
                break;

            case 40:
                speed = 8.0f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 7.0f;
                isPlayingSound = false;
                break;

            case 35:
                speed = 8.0f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 4.5f;
                isPlayingSound = false;
                break;

            case 30:
                speed = 7.5f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 3.0f;
                isPlayingSound = false;
                break;

            case 25:
                speed = 7.0f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 2.5f;
                isPlayingSound = false;
                break;

            case 20:
                speed = 6.5f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 2.0f;
                isPlayingSound = false;
                break;

            case 15:
                speed = 5.5f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 1.0f;
                isPlayingSound = false;
                break;
            case 10:
                speed = 4.5f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.5f;

                //It's not looking too good
                if (!isPlayingSound)
                {
                    Instantiate(warningCritical, this.transform.position, this.transform.rotation);
                    isPlayingSound = true;
                }

                break;
            case 5:
                speed = 2.5f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;
        }
        if (isBoosting)
        {
            speed = speed += 20.0f;
            IdleGasStream.gameObject.SetActive(false);
        }
    }


    void HeroMoveUpdate()
    {
        //update text fields
        batteryValText.text = "" + battery + " %";
        ammoValText.text = "" + Ammo;
        gearsValText.text = "" + Gears;


        CharacterController controller = GetComponent<CharacterController>();
        // is the controller on the ground?
        if (controller.isGrounded)
        {

            //Feed moveDirection with input.
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;
            


            //Jumping set to A is actually fire1
            //Change this back to "Jump"
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                anim.SetTrigger("isJumping");
            }

            if (Input.GetKey("w") && speed > 7.5f)
            {
                anim.SetTrigger("isRunning");
            }
            else if(Input.GetKey("w") && speed <= 7.5f)
            {
                anim.SetTrigger("isWalking");
            }

            //CDC this will change when new weps are added
            if (Input.GetKey("1"))
            {
                isSlot1 = true;
                isSlot2 = false; isSlot3 = false; /*isSlot4 = false; isSlot5 = false;*/
                gun.SetActive(false);
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
                gun.SetActive(false);
            }
            else if (Input.GetKey("4"))
            {
                isSlot4 = true;
                /*isSlot1 = false; isSlot2 = false; isSlot3 = false;*/ isSlot5 = false;
            }
            else if (Input.GetKey("5"))
            {
                isSlot5 = true;
                /*isSlot1 = false; isSlot2 = false; isSlot3 = false;*/ isSlot4 = false;
            }

            if (Input.GetButton("Fire1") && Ammo > 0 && cooldownTimer < 0.01f  && hasGun && isSlot2)
            {
                anim.SetTrigger("isPunching");
                Instantiate(objToSpawn, spawnLoc.transform.position, this.transform.rotation);
                //reset cooldown.
                cooldownTimer = 0.3f; //cooldown
                Ammo -= 1;
            }
            else if (Input.GetButton("Fire1") && cooldownTimer < 0.01f && hasGS && isSlot3)
            {
                anim.SetTrigger("isSlashing");
                Instantiate(GSShot, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 1.3f;
                
            }
            else if (Input.GetButton("Fire1") && cooldownTimer < 0.01f && hasAxe && isSlot1)
            {
                anim.SetTrigger("isAxeHacking");
                Instantiate(AxeShot, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 0.8f;
                
            }


            //SPECIAL ATTACKS - X
            if (Input.GetKey("x") && cooldownTimer < 0.01f && hasAxe && hasAxe_LIGHTNING && isSlot1)
            {
                anim.SetTrigger("isAxeHacking");
                Instantiate(AxeShot_LIGHTNING, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 2.1f;
            }
            else if (Input.GetKey("x") && cooldownTimer < 0.01f && hasGS && hasGS_FIRE && isSlot3)
            {
                anim.SetTrigger("isSlashing");
                Instantiate(GSShot_FIRE, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 3.0f;
            }
            else if (Input.GetKey("x") && cooldownTimer < 0.01f && hasGun && hasGun_MULTI && isSlot2)
            {
                anim.SetTrigger("isPunching");
                Instantiate(GunShot_MULTI, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 2.0f;
               // Ammo -= 1;  // special uses no ammo!
            }

            //note that offhands will use C to differentiate
            if (Input.GetKey("c") && cooldownTimer < 0.01f && hasShield && hasShield_ICE && isSlot4)
            {
                anim.SetTrigger("isShieldBashing");
                Instantiate(ShieldShot_ICE, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 7.0f;
            }

            if (Input.GetKey("c") && cooldownTimer < 0.01f && hasJetBooster && hasJetBooster_ARCANE && isSlot5)
            {
                anim.SetTrigger("isShieldBashing");
                Instantiate(JetBooster_ARCANE, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 3.0f;
            }



            //interact button
            if (Input.GetKey("f") && interactTimer < 0.01f)
            {
                anim.SetTrigger("isPunching");
                Instantiate(interactShot, spawnLoc.transform.position, this.transform.rotation);
                interactTimer = 0.7f;
            }
            if (Input.GetKey("v") && cooldownTimer < 0.01f && hasShield && isSlot4)
            {
                anim.SetTrigger("isShieldBashing");
                //shield bash = same damage as an axe!
                Instantiate(AxeShot, spawnLoc.transform.position, this.transform.rotation);
                cooldownTimer = 2.0f;
            }
            if (Input.GetKey("v") && dashCooldownTimer < 0.01f && hasJetBooster && isSlot5)
            {
                anim.SetTrigger("isShieldBashing");
                Instantiate(GSShot, spawnLoc.transform.position, this.transform.rotation); //does damage as you charge forward
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
        if (Input.GetKey(KeyCode.Semicolon) == true && Input.GetKey(KeyCode.R) == true)
        {
            SceneManager.LoadScene("Intro");

        }

    }
    

}
