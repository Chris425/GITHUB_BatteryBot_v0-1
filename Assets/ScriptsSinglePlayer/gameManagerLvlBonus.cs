using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerLvlBonus : MonoBehaviour {

    // This is specifically for bonus level
    //when level bonus is loaded this script runs the start code
    // reference the universal game manager script to determine where to spawn the player and under what conditions

    public GameObject savePoint1;
    public GameObject savePoint2;

    void Start () {

        GameObject hero = GameObject.Find("PLAYERBASE");
        
        

        if (GAMEMANAGERSP.hasLevelBonusSave2)
        {
            //they would have these skulls by this point


            //move the hero to this position
            hero.transform.position = savePoint2.transform.position;
        }

        else if (GAMEMANAGERSP.hasLevelBonusSave1)
        {
            hero.transform.position = savePoint1.transform.position;
        }
	}
	
}
