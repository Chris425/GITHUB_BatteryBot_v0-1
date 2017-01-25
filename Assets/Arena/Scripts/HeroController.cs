using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{

    private Animator anim;

    public float speed = 15.0F;
    public float jumpSpeed = 12.0F;
    public float gravity = 27.0F;
    private Vector3 moveDirection = Vector3.zero;

    public GameObject SE_hit;

    public static bool isSuperCharged = false;

    public GameObject HeroBeingControlled; //put the entity that is being controlled in this variable

    public Light ExtraLight;
    public Light GreenLight;
    public Light YellowLight;
    public Light RedLight;


    public GameObject warning;
    public GameObject warningCritical;
    private bool isPlayingSound = false; //I goofed. Here's a hack to fix it

    //gun ammo
    public static int Ammo;
    public Text ammoValText;
    public GameObject objToSpawn;
    public GameObject spawnLoc;
    public float cooldown = 0.9f;
    private float cooldownTimer;

    //Battery life
    public static int battery = 100;
    public Text batteryValText;


    public static float time;
    public Text timerValText;

    public float batteryTimer;
    

    void Update()
    {
        HeroMoveUpdate();
        TimerUpdate();
    }


    public void OnEnable()
    {
        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;

        battery = 100;
        speed = 15.0f;
        Ammo = 10;
        time = 0;

        batteryValText.text = "BATTERY REMAINING: " + battery + " %";
        timerValText.text = "TIME: " + time;
        ammoValText.text = "AMMO: " + Ammo;
        

    }


    public void OnCollisionEnter(Collision other)
    {
        //case when the player is hit by the caster
        if (other.gameObject.name.Contains("CasterShot"))
        {
            Instantiate(SE_hit, this.transform.position, this.transform.rotation);
            battery -= 15;
            Destroy(other.gameObject);
        }
    }
            void TimerUpdate()
    {

        //let's increment the timers every update call
        time += 0.01f;
        batteryTimer += 0.01f;
        cooldownTimer -= 0.04f;

        //after 350 miliseconds you lose 1% battery (almost as bad as my old phone battery)
        if (batteryTimer > 0.35f) 
        {
            battery -= 1;
            batteryTimer = 0.0f;
        }

        if (battery >= 200)
        {
            speed = 20.0f;
            isSuperCharged = true;
            ExtraLight.intensity = 8.0f;
            GreenLight.intensity = 8.0f;
            YellowLight.intensity = 0.0f;
            RedLight.intensity = 0.0f;
            isPlayingSound = false;
        }
        else if (battery > 100 && battery < 199)
        {
            speed = 15.0f;
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
            SceneManager.LoadScene("Gameover");
        }
    }

    //Every 10% battery loss, lose 10% speed
    //There are 3 lights - green, yellow, and red. The intensity will be increased/decreased depending on your battery life!
    void batteryEffects()
    {
       
        switch (battery)
        {
            
            case 100:
                speed = 15.0f;

                ExtraLight.intensity = 0.0f;
                GreenLight.intensity = 8.0f; // 8 is max light intensity
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;

                break;

            case 95:
                speed = 14.25f;


                GreenLight.intensity = 7.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 90:
                speed = 13.5f;


                GreenLight.intensity = 4.5f; 
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 85:
                speed = 12.75f;

                GreenLight.intensity = 3.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 80:
                speed = 12.0f;

                GreenLight.intensity = 2.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 75:
                speed = 11.25f;
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
                speed = 10.5f;
                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 7.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 65:
                speed = 11.0f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 4.5f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 60:
                speed = 9.75f;

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
                speed = 8.25f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 2.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;

            case 45:
                speed = 7.5f;

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
                speed = 6.75f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 7.0f;
                isPlayingSound = false;
                break;

            case 35:
                speed = 6.0f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 4.5f;
                isPlayingSound = false;
                break;

            case 30:
                speed = 5.25f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 3.0f;
                isPlayingSound = false;
                break;

            case 25:
                speed = 4.5f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 2.5f;
                isPlayingSound = false;
                break;

            case 20:
                speed = 3.75f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 2.0f;
                isPlayingSound = false;
                break;

            case 15:
                speed = 3.0f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 1.0f;
                isPlayingSound = false;
                break;
            case 10:
                speed = 2.25f;

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
                speed = 1.5f;

                GreenLight.intensity = 0.0f;
                YellowLight.intensity = 0.0f;
                RedLight.intensity = 0.0f;
                isPlayingSound = false;
                break;
        }
    }


    void HeroMoveUpdate()
    {
        //update text fields
        batteryValText.text = "BATTERY REMAINING: " + battery + " %";
        timerValText.text = "TIME: " + time.ToString("n2");
        ammoValText.text = "AMMO: " + Ammo;


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
             
            if (Input.GetButton("Fire1") && Ammo > 0 && cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isPunching");
                Instantiate(objToSpawn, spawnLoc.transform.position, this.transform.rotation);
                //reset cooldown.
                cooldownTimer = cooldown;
                Ammo -= 1;
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
            SceneManager.LoadScene("Intro");

        }
        
    }
    

}
