﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SafetyDanceGridSP : MonoBehaviour {

    public GameObject warning;
    public GameObject explosion;
    public AudioClip warningSound;
    public AudioClip explosionSound;
    private AudioSource source;
    public List<GameObject> spawnLocs = new List<GameObject>();

    public static bool isActive = true;
    public static bool isTransitionPhase = true;
    private float cooldownTimer = 0.0f;
    private int counter = 0;
    private int patternChoice;
   
    // Use this for initialization
    void Start () {
        //CDC - this doesn't work because it adds it an undefined order. Doing it manually.

        //foreach (GameObject spawnLoc in GameObject.FindObjectsOfType<GameObject>())
        //{
        //    if (spawnLoc.name.Contains("GridSpawnLoc"))
        //    {
        //        spawnLocs.Add(spawnLoc);
        //    }
        //}

        source = GetComponent<AudioSource>();
        patternChoice = 1;
    }
	
	// Update is called once per frame
	void Update () {

        cooldownTimer -= 0.01f;

       

        if (isActive)
        {
            if (cooldownTimer <= 0.01f)
            {
                if (counter % 2 == 0) //alternates between warning and explosion
                {
                    patternChoice = Random.Range(1, 9);

                    ChoosePatternWarning(patternChoice);

                    cooldownTimer = 2.0f; 
                    counter += 1;
                }
                else
                {

                    ChoosePatternExplosion(patternChoice);

                    cooldownTimer = 2.0f; 
                    counter += 1;
                }
            }
            
                
            
        }
	}

    
    void ChoosePatternWarning(int numChoice)
    {
        switch (numChoice)
        {
            //Horizontal Lines
            case 1:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i <= 7) || (i >=16 && i <=23) || (i >= 32 && i <= 39))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.5f);
                break;
           //Wide Horizontal Lines
            case 2:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i <= 15) || (i >= 32))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.5f);
                break;
            //Wide Horizontal, middle only
            case 3:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i > 15 && i < 32))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.5f);
                break;
            //Vertical Lines
            case 4:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i== 0) || (i == 4) || (i == 7) || (i % 8 == 0) || (i % 8 == 3) || (i % 8 == 4) || (i  % 8 == 7))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.5f);
                break;
            //Wide Vertical Lines
            case 5:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i == 0) || (i == 1) || (i == 6) || (i == 7) || (i % 8 == 0) || (i % 8 == 1) || (i % 8 == 6) || (i % 8 == 7))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.5f);
                break;
             //Wide Vertical Line, Middle
            case 6:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i == 2) || (i == 3) || (i == 4) || (i == 5) || (i % 8 == 2) || (i % 8 == 3) || (i % 8 == 4) || (i % 8 == 5))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.5f);
                break;
            //Outer Ring
            case 7:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i <=15) || (i >= 32) || (i % 8 == 0) || (i % 8 == 1) || (i % 8 == 6) || (i % 8 == 7))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.5f);
                break;
            //Inner Ring
            case 8:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 18 && i <=21) || (i >= 26 && i <= 29))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.5f);
                break;
            //Checkered Lines
            default:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.5f);
                break;
        }
    }

    void ChoosePatternExplosion(int numChoice)
    {
        switch (numChoice)
        {
            case 1:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i <= 7) || (i >= 16 && i <= 23) || (i >= 32 && i <= 39))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                        source.PlayOneShot(explosionSound, 0.1f);
                    }
                }

                break;
            case 2:

                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i <= 15) || (i >= 32))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                        source.PlayOneShot(explosionSound, 0.1f);
                    }
                }
                break;
            case 3:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i > 15 && i < 32))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                        source.PlayOneShot(explosionSound, 0.1f);
                    }
                }
                break;
            case 4:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i == 0) || (i == 4) || (i == 7) || (i % 8 == 0) || (i % 8 == 3) || (i % 8 == 4) || (i % 8 == 7))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                        source.PlayOneShot(explosionSound, 0.1f);
                    }
                }
                break;
            case 5:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i == 0) || (i == 1) || (i == 6) || (i == 7) || (i % 8 == 0) || (i % 8 == 1) || (i % 8 == 6) || (i % 8 == 7))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                        source.PlayOneShot(explosionSound, 0.1f);
                    }
                }
                break;
            case 6:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i == 2) || (i == 3) || (i == 4) || (i == 5) || (i % 8 == 2) || (i % 8 == 3) || (i % 8 == 4) || (i % 8 == 5))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                        source.PlayOneShot(explosionSound, 0.1f);
                    }
                }
                break;
            case 7:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i <= 15) || (i >= 32) || (i % 8 == 0) || (i % 8 == 1) || (i % 8 == 6) || (i % 8 == 7))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                        source.PlayOneShot(explosionSound, 0.1f);
                    }
                }
                break;
            case 8:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 18 && i <= 21) || (i >= 26 && i <= 29))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                        source.PlayOneShot(explosionSound, 0.1f);
                    }
                }
                break;
            default:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                        source.PlayOneShot(explosionSound, 0.1f);
                    }
                }
                break;
        }
    }







}
