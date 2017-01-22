using UnityEngine;
using System.Collections;
using System;

public class GentleMovement2 : MonoBehaviour {

    public int maxMovements = 145;
    int counter = 0;
    bool isGoingUp ;
    bool isGoingDown;

    // Use this for initialization
    void Start () {
        isGoingUp = false;
        isGoingDown = true;
    }
	
	// Update is called once per frame
	void Update () {
        rotate();
        if (isGoingUp && counter <= maxMovements)
        {
            if (counter < 15)
            {
                moveUpSlowly();
            }
            else if (counter >= 15 && counter < 105)
            {
                moveUp();
            }
            else if (counter >= 105 && counter < 120)
            {
                //imitate deceleration
                moveUpSlowly();
            }
            else if (counter >= 120)
            {
                //stop moving at peak
            }

            counter += 1;

        }
        if(isGoingUp && counter > maxMovements) 
        {
            counter = 0;
            isGoingDown = true;
            isGoingUp = false;
        }

        if (isGoingDown && counter <= maxMovements)
        {
            if (counter < 15)
            {
                moveDownSlowly();
            }
            else if(counter >= 15 && counter < 105)
            {
                moveDown();
            }
            else if (counter >= 105 && counter < 120)
            {
                //imitate deceleration
                moveDownSlowly();
            }
            else if (counter >= 110)
            {
                //stop moving at Valley
            }
            counter += 1;

        }
        if(isGoingDown && counter > maxMovements)
        {
            counter = 0;
            isGoingDown = false;
            isGoingUp = true;
        }


    }

    private void moveDown()
    {
        this.transform.Translate(new Vector3(0, -0.2f,0) * Time.deltaTime);
    }

    private void moveDownSlowly()
    {
        this.transform.Translate(new Vector3(0, -0.05f, 0) * Time.deltaTime);
    }

    private void moveUp()
    {
        this.transform.Translate(new Vector3(0, 0.2f, 0) * Time.deltaTime);
    }

    private void moveUpSlowly()
    {
        this.transform.Translate(new Vector3(0, 0.05f, 0) * Time.deltaTime);
    }

    private void rotate()
    {
        transform.Rotate(new Vector3(0, 0.8f, 0) * Time.deltaTime);
    }
}
