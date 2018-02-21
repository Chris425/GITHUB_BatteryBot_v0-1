using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamage : MonoBehaviour {


    public GameObject SE_LavaHit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter(Collision other)
    {
        int randomNum = Random.Range(0, 30);
        if (other.gameObject.name.Equals("BatteryBotTouch") && randomNum > 3)
        {
            HeroControllerSP.battery -= 1;
            if (randomNum > 24)
            {
                Instantiate(SE_LavaHit, other.transform.position, this.transform.rotation);
            }
        }

    }

}
