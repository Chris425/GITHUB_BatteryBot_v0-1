using UnityEngine;
using System.Collections;

public class PowerUpRotatorSP : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        spinPowerUp();
    }

    void spinPowerUp()
    {
        transform.Rotate(new Vector3(30, 45, 60) *Time.deltaTime);
    }

   
}
