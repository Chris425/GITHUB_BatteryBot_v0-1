﻿using UnityEngine;
using System.Collections;

public class DoorScriptSP : MonoBehaviour {

    public AudioClip switchHit;
    public AudioClip errorSound;
    public AudioClip successSound;
    private AudioSource source;
    public string doorColour;

    public GameObject door;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void Awake()
    {
        source = GetComponent<AudioSource>();

    }


    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("PlayerShot") || other.gameObject.name.Contains("InteractShot"))
        {

            if (doorColour.ToUpper().Contains("RED"))
            {
                if (HeroControllerSP.hasSkull_RED || HeroControllerSP.hasSkull_BRONZE || HeroControllerSP.hasSkull_SILVER || HeroControllerSP.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }
            else if (doorColour.ToUpper().Contains("PURPLE") )
            {
                if (HeroControllerSP.hasSkull_PURPLE || HeroControllerSP.hasSkull_BRONZE || HeroControllerSP.hasSkull_SILVER || HeroControllerSP.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }
            else if (doorColour.ToUpper().Contains("BLUE"))
            {
                if (HeroControllerSP.hasSkull_BLUE || HeroControllerSP.hasSkull_BRONZE || HeroControllerSP.hasSkull_SILVER || HeroControllerSP.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }
            else if (doorColour.ToUpper().Contains("BRONZE") )
            {
                if (HeroControllerSP.hasSkull_BRONZE || HeroControllerSP.hasSkull_SILVER || HeroControllerSP.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }
            else if (doorColour.ToUpper().Contains("SILVER") )
            {
                if (HeroControllerSP.hasSkull_SILVER || HeroControllerSP.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }
            else if (doorColour.ToUpper().Contains("GOLD"))
            {
                if (HeroControllerSP.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }




        }

    }

}
