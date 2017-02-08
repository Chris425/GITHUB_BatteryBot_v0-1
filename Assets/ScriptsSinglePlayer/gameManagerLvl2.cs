using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerLvl2 : MonoBehaviour {

    // This is specifically for level 2 
    //when level two is loaded this script runs the start code
    // references the universal game manager script to determine where to spawn the player and under what conditions


    //just one save point for level 2 due to non-linear nature of the maze.
    public GameObject savePoint1;
    public GameObject MiniMapCamera;
    public GameObject PlayerMapIcon;
    public GameObject hero;

    

    void onEnable () {
        hero = GameObject.Find("PLAYERBASE");


        if (GAMEMANAGERSP.hasLevelTwoSave1)
        {
            HeroControllerSP.hasSkull_BLUE = true;
            HeroControllerSP.hasSkull_PURPLE = true;
            HeroControllerSP.hasSkull_RED = true;
            hero.transform.position = savePoint1.transform.position;
        }
	}

    void Update()
    {
        updateMiniMap();
    }
    void updateMiniMap()
    {
        Vector3 newCamPos = new Vector3(hero.transform.position.x, MiniMapCamera.transform.position.y, hero.transform.position.z);
        Vector3 newPlayerIconPos = new Vector3(hero.transform.position.x, PlayerMapIcon.transform.position.y, hero.transform.position.z);
        //move the camera over the player
        MiniMapCamera.transform.position = newCamPos;
        PlayerMapIcon.transform.position = newPlayerIconPos;

    }
	
}
