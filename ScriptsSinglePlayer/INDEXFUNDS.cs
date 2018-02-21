using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INDEXFUNDS : MonoBehaviour {

	// Use this for initialization
	void Start () {

        float initial = 1000f;
        float interest = 0.0025f; //take percentage and divide by 12 for monthly
        float monthlyDeposit = 50f;
        calculateInterest(initial, interest, monthlyDeposit);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void calculateInterest(float initial, float interest, float monthlyDeposit)
    {

        float currAmount = initial;

        for (int i = 1; i < 49; i++)
        {
            currAmount += monthlyDeposit;
            float monthlyInt = currAmount * interest;
            currAmount += monthlyInt;
            Debug.Log("Month: " + i + " Amount: " + currAmount);

        }
        


    }
}
