using UnityEngine;
using System.Collections;
using System;

public class ForwardsBackwards : MonoBehaviour {

    public int FirstThreshold = 15;
    public int SecondThreshold = 85;
    public int ThirdThreshold = 110;
    public int MaxThreshold = 125;


    public float slowSpeed = 0.6f;
    public float normalSpeed = 1.1f;
    public float rotSpeed = 4.0f;
    int counter = 0;
    bool isGoingForwards;
    bool isGoingBackwards;

    // Use this for initialization
    void Start () {
        isGoingForwards = false;
        isGoingBackwards = true;

       
    }

   
    void Update () {

   

        rotate();
        if (isGoingForwards && counter <= MaxThreshold)
        {
            if (counter < FirstThreshold)
            {
                moveForwardsSlowly();
            }
            else if (counter >= FirstThreshold && counter < SecondThreshold)
            {
                moveForwards();
            }
            else if (counter >= SecondThreshold && counter < ThirdThreshold)
            {
                //imitate deceleration
                moveForwardsSlowly();
            }
            else if (counter >= ThirdThreshold)
            {
                //stop moving at peak
            }

            counter += 1;

        }
        if(isGoingForwards && counter > MaxThreshold) 
        {
            counter = 0;
            isGoingBackwards = true;
            isGoingForwards = false;
        }

        if (isGoingBackwards && counter <= MaxThreshold)
        {
            if (counter < FirstThreshold)
            {
                moveBackwardsSlowly();
            }
            else if(counter >= FirstThreshold && counter < SecondThreshold)
            {
                moveBackwards();
            }
            else if (counter >= SecondThreshold && counter < ThirdThreshold)
            {
                //imitate deceleration
                moveBackwardsSlowly();
            }
            else if (counter >= ThirdThreshold)
            {
                //stop moving at Valley
            }
            counter += 1;

        }
        if(isGoingBackwards && counter > MaxThreshold)
        {
            counter = 0;
            isGoingBackwards = false;
            isGoingForwards = true;
        }



        
    }


    private void moveBackwards()
    {
        this.transform.Translate(new Vector3(-normalSpeed, 0,0) * Time.deltaTime);
       
    }

    private void moveBackwardsSlowly()
    {
        this.transform.Translate(new Vector3(-slowSpeed, 0, 0) * Time.deltaTime);
       
    }

    private void moveForwards()
    {
        this.transform.Translate(new Vector3(normalSpeed, 0, 0) * Time.deltaTime);
        
    }

    private void moveForwardsSlowly()
    {
        this.transform.Translate(new Vector3(slowSpeed, 0, 0) * Time.deltaTime);
        
    }

    private void rotate()
    {
        transform.Rotate(new Vector3(0, -rotSpeed, 0) * Time.deltaTime);
    }
}
