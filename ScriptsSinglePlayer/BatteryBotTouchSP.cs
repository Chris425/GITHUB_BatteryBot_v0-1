using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryBotTouchSP : MonoBehaviour {

    public float speed = 5f;    
    bool isGoingUp;
    bool isGoingDown;

    private float maxHeight;
    private float minHeight;
    private float currHeight;
    private GameObject BatteryBot;

    // Use this for initialization
    void Start () {
        isGoingUp = true;
        isGoingDown = false;

        BatteryBot = GameObject.Find("BatteryBot");
      
    }
	
	// Update is called once per frame
	void Update () {

        //find this every frame since you may be jumping or falling, etc.
        maxHeight = BatteryBot.transform.position.y + 0.5f;
        minHeight = BatteryBot.transform.position.y - 0.5f;
        currHeight = this.transform.position.y;

        if (isGoingUp && currHeight <= maxHeight)
        {
            this.transform.Translate(new Vector3(0, speed, 0) * Time.deltaTime);

        }
        if (isGoingUp && currHeight > maxHeight)
        {
            isGoingDown = true;
            isGoingUp = false;
        }

        if (isGoingDown && currHeight >= minHeight)
        {
            this.transform.Translate(new Vector3(0, -speed, 0) * Time.deltaTime);
        }
        if (isGoingDown && currHeight < minHeight)
        {
            isGoingDown = false;
            isGoingUp = true;
        }


    }
}
