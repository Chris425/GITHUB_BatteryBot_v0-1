using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerLvl2 : MonoBehaviour {

    // This is specifically for level 2 
    //when level two is loaded this script runs the start code
    // references the universal game manager script to determine where to spawn the player and under what conditions

    public GameObject savePoint1;
    public GameObject savePoint2;

    void Start () {

        GameObject hero = GameObject.Find("PLAYERBASE");
        
        

        if (GAMEMANAGERSP.hasLevelTwoSave2)
        {
            //they would have these skulls by this point
          


            //move the hero to this position
            hero.transform.position = savePoint2.transform.position;
        }

        else if (GAMEMANAGERSP.hasLevelTwoSave1)
        {
            HeroControllerSP.hasSkull_BLUE = true;
            hero.transform.position = savePoint1.transform.position;
        }
	}
	
}
