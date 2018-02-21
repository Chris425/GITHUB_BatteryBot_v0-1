using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerLvl3 : MonoBehaviour {

    // This is specifically for level 3 
    //when level three is loaded this script runs the start code
    // reference the universal game manager script to determine where to spawn the player and under what conditions

    public GameObject savePoint1;
    public GameObject savePoint2;
    private GameObject blockade;
    public static bool isMinoDead;
    public static bool isLightningMinoDead;

    void Start () {

        GameObject hero = GameObject.Find("PLAYERBASE");
        GameObject blockade = GameObject.Find("MinotaurBarrier");
        blockade.SetActive(true);



        if (GAMEMANAGERSP.hasLevelThreeSave2)
        {
            //they would have these skulls by this point
           

            //move the hero to this position
            hero.transform.position = savePoint2.transform.position;
            blockade.SetActive(false);
        }

        else if (GAMEMANAGERSP.hasLevelThreeSave1)
        {

            hero.transform.position = savePoint1.transform.position;
            hero.transform.rotation = savePoint1.transform.rotation;
            blockade.SetActive(false);
        }
	}

    //will be called on minotaur death
    //only actually works if both are dead
    public static void removeBarrier()
    {
        if (isMinoDead && isLightningMinoDead)
        {
            GameObject blockade = GameObject.Find("MinotaurBarrier");
            blockade.SetActive(false);
        }
    }
}
