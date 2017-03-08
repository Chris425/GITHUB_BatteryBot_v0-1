using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotNPCSP : MonoBehaviour {

    private Animator anim;
    public GameObject target;
    private GameObject batteryBot;    
    UnityEngine.AI.NavMeshAgent agent;

    public float distanceX;
    public float distanceZ;
    public float distanceY;
    private float cooldown = 20.0f;
    private float cooldownTimer;

    public GameObject healCast;
    public GameObject PickedUp;
    public GameObject InteractNoise;
    private bool isActive;

    // Use this for initialization
    void OnEnable()
    {       
        isActive = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (isActive)
        {
            distanceX = this.transform.position.x - target.transform.position.x;
            distanceZ = this.transform.position.z - target.transform.position.z;
            distanceY = this.transform.position.y - target.transform.position.y;

            moveToPlayer();
        }
        
    }

    public void OnCollisionEnter(Collision other)
    {
        //interact to enable the NPC and gain its assistance
        if (!isActive)
        {
            //first time touching NPC
            if (other.gameObject.name.Contains("InteractShot"))
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = true;

                isActive = true;
                HeroControllerSP.hasNPCBot = true;
                Instantiate(PickedUp, this.transform.position, this.transform.rotation);


                anim = this.GetComponentInChildren<Animator>();
                anim.SetBool("isRunning", false);

                agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                target = GameObject.Find("FollowPoint");
                batteryBot = GameObject.Find("BatteryBot");
                anim.applyRootMotion = false;
            }
        }
        else
        {
            //when you interact with an already active NPC
            //enemy shots also do this
            anim.SetTrigger("Interacted");
            Instantiate(InteractNoise, this.transform.position, this.transform.rotation);
        }
        
    }

    private void moveToPlayer()
    {
        cooldownTimer -= 0.03f;



        if ((distanceX > -2 && distanceX < 2) && (distanceZ > -2 && distanceZ < 2) && (distanceY > -2 && distanceY < 2))
        {
            //we are in range
            anim.SetBool("isRunning", false);
            //only heal the player if they're hurt and you're in range
            if (HeroControllerSP.battery < 100 && cooldownTimer < 0.01f)
            {
                anim.SetTrigger("isCasting");
                //put special effect on player
                GameObject healSpecialEffect = Instantiate(healCast, batteryBot.transform.position, batteryBot.transform.rotation);                
                healSpecialEffect.transform.parent = batteryBot.gameObject.transform;
                HeroControllerSP.battery += 2;
                cooldownTimer = cooldown;
            }
            

        }
        else
        {
            if (agent.isActiveAndEnabled)
            {
                agent.SetDestination(target.transform.position);
                anim.SetBool("isRunning", true);
            }

        }

        if ((distanceX < -15 || distanceX > 15) && (distanceZ < -15 || distanceZ > 15) || (distanceY < -7 || distanceY > 7))
        {
            //Player is waaay too far away
            Vector3 telePoint = new Vector3(target.transform.position.x, batteryBot.transform.position.y, target.transform.position.z);
            agent.enabled = false;
            //teleport
            this.transform.position = telePoint;
            agent.enabled = true;
            agent.Resume();
            
            //CDC BUG - this introduces a bug if you are wall climbing. Can't snap to nav mesh when it teleports.
            //however it should not present itself during normal gameplay and during a build it doesn't stop running so it won't crash.
            
        }
    }
}
