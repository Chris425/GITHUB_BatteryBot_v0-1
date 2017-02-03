using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerLvl1 : MonoBehaviour {

    // This is specifically for level 1 
    //when level one is loaded this script runs the start code
    // reference the universal game manager script to determine where to spawn the player and under what conditions

    public GameObject savePoint1;
    public GameObject savePoint2;

    void Start () {

        GameObject hero = GameObject.Find("PLAYERBASE");
        
        

        if (GAMEMANAGERSP.hasLevelOneSave2)
        {
            //they would have these skulls by this point
            HeroControllerSP.hasSkull_BLUE = true;
            HeroControllerSP.hasSkull_RED = true;
            HeroControllerSP.hasSkull_PURPLE = true;
            //move the hero to this position
            hero.transform.position = savePoint2.transform.position;
        }

        else if (GAMEMANAGERSP.hasLevelOneSave1)
        {
            HeroControllerSP.hasSkull_BLUE = true;
            hero.transform.position = savePoint1.transform.position;
        }
	}
	
}
