﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour {

    public GameObject elevator;
    public int maxElevatorHeight = 150;
    float velocity = 0.22f;
    bool isMovingUp = false;
    bool isMovingDown = false;
    bool isAtTop = false;
    bool isAtBottom = true;
    int counter = 0;

    public AudioClip switchHit;

    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();

    }

    public void OnCollisionEnter(Collision other)
    {
        //case when your player projectile hits the elevator controller
        //But will only do stuff if you are not currently moving
        if (other.gameObject.name.Contains("PlayerShot") || other.gameObject.name.Contains("InteractShot"))
        {
            source.PlayOneShot(switchHit, 1.0f);
            Debug.Log(this.gameObject.name + "was hit by" + other.gameObject.name);
            if (isAtBottom)
            {
                isMovingUp = true;
            }
            else
            {
                isMovingDown = true;
            }           
        }
    }

    public void moveUp()
    {
        if (counter < maxElevatorHeight)
        {
           elevator.transform.Translate(new Vector3(0, velocity, 0));
            counter++;
        }
        else
        {
            counter = 0;
            isMovingUp = false;
            isAtTop = true;
            isAtBottom = false;
        }
       
    }

    public void moveDown()
    {
        if (counter < maxElevatorHeight)
        {
            elevator.transform.Translate(new Vector3(0, -velocity, 0));
            counter++;
        }
        else
        {
            counter = 0;
            isMovingDown = false;
            isAtTop = false;
            isAtBottom = true;
        }
    }
    // Use this for initialization
    void Start () {
        
    }

	// Update is called once per frame
	void Update () {

        if (isMovingUp)
        {
            moveUp();
        }
        else if (isMovingDown)
        {
            moveDown();
        }
	}
}
