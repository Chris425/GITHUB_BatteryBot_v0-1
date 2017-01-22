using UnityEngine;
using System.Collections;
using System;

public class FloatingObject_GentleMovement : MonoBehaviour {

    public int FirstThreshold= 15;
    public int SecondThreshold = 85;
    public int ThirdThreshold = 110;
    public int MaxThreshold = 125;


    public float slowSpeed = 0.6f;
    public float normalSpeed = 1.1f;
    public float rotSpeed = 4.0f;
    int counter = 0;
    bool isGoingUp ;
    bool isGoingDown;

    // Use this for initialization
    void Start () {
        isGoingUp = true;
        isGoingDown = false;
    }
	
	// Update is called once per frame
	void Update () {
        rotate();
        if (isGoingUp && counter <= MaxThreshold)
        {
            if (counter < FirstThreshold)
            {
                moveUpSlowly();
            }
            else if (counter >= FirstThreshold && counter < SecondThreshold)
            {
                moveUp();
            }
            else if (counter >= SecondThreshold && counter < ThirdThreshold)
            {
                //imitate deceleration
                moveUpSlowly();
            }
            else if (counter >= ThirdThreshold)
            {
                //stop moving at peak
            }

            counter += 1;

        }
        if(isGoingUp && counter > MaxThreshold) 
        {
            counter = 0;
            isGoingDown = true;
            isGoingUp = false;
        }

        if (isGoingDown && counter <= MaxThreshold)
        {
            if (counter < FirstThreshold)
            {
                moveDownSlowly();
            }
            else if(counter >= FirstThreshold && counter < SecondThreshold)
            {
                moveDown();
            }
            else if (counter >= SecondThreshold && counter < ThirdThreshold)
            {
                //imitate deceleration
                moveDownSlowly();
            }
            else if (counter >= ThirdThreshold)
            {
                //stop moving at Valley
            }
            counter += 1;

        }
        if(isGoingDown && counter > MaxThreshold)
        {
            counter = 0;
            isGoingDown = false;
            isGoingUp = true;
        }


    }

    private void moveDown()
    {
        this.transform.Translate(new Vector3(0, -normalSpeed, 0) * Time.deltaTime);
    }

    private void moveDownSlowly()
    {
        this.transform.Translate(new Vector3(0, -slowSpeed, 0) * Time.deltaTime);
    }

    private void moveUp()
    {
        this.transform.Translate(new Vector3(0, normalSpeed, 0) * Time.deltaTime);
    }

    private void moveUpSlowly()
    {
        this.transform.Translate(new Vector3(0, slowSpeed, 0) * Time.deltaTime);
    }

    private void rotate()
    {
        transform.Rotate(new Vector3(0, rotSpeed, 0) * Time.deltaTime);
    }
}
