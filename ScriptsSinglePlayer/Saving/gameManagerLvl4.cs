using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerLvl4 : MonoBehaviour {

    // This is specifically for level 4
    //when level four is loaded this script runs the start code
    // reference the universal game manager script to determine where to spawn the player and under what conditions

    public GameObject savePoint1;
    public GameObject savePoint2;




    void Start () {

        GameObject hero = GameObject.Find("PLAYERBASE");



        if (GAMEMANAGERSP.hasLevelFourSave2)
        {          

            //move the hero to this position
            hero.transform.position = savePoint2.transform.position;
        }

        else if (GAMEMANAGERSP.hasLevelFourSave1)
        {

            hero.transform.position = savePoint1.transform.position;
            hero.transform.rotation = savePoint1.transform.rotation;
        }
	}





}
