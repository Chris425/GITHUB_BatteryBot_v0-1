using DigitalRuby.RainMaker;
using System;
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
    public GameObject RainPrefab;
    private bool hasGottenBlueSkull;
    private bool hasGottenRedSkull;
    private bool hasGottenPurpleSkull;
    private bool hasGottenSavePoint;
    //bools for each skull gotten and save point.

    private RainScript rainScript;

    void Awake () {
        hero = GameObject.Find("PLAYERBASE");

        hasGottenBlueSkull = false;
        hasGottenRedSkull = false; 
        hasGottenPurpleSkull = false;
        hasGottenSavePoint = false;

        if (GAMEMANAGERSP.hasLevelTwoSave1)
        {
            HeroControllerSP.hasSkull_BLUE = true;
            HeroControllerSP.hasSkull_PURPLE = true;
            HeroControllerSP.hasSkull_RED = true;
            hero.transform.position = savePoint1.transform.position;

            hasGottenBlueSkull = true;
            hasGottenRedSkull = true;
            hasGottenPurpleSkull = true;
            hasGottenSavePoint = true;
        }

        rainScript = RainPrefab.GetComponent<RainScript>();
	}

    void Update()
    {
        updateMiniMap();
        updateRainMaker();
    }

    private void updateRainMaker()
    {
        if (HeroControllerSP.hasSkull_BLUE && !hasGottenBlueSkull)
        {
            rainScript.RainIntensity += 0.2f;
            hasGottenBlueSkull = true;
        }
        if (HeroControllerSP.hasSkull_RED && !hasGottenRedSkull)
        {
            rainScript.RainIntensity += 0.2f;
            hasGottenRedSkull = true;
        }
        if (HeroControllerSP.hasSkull_PURPLE && !hasGottenPurpleSkull)
        {
            rainScript.RainIntensity += 0.2f;
            hasGottenPurpleSkull = true;
        }
        if (GAMEMANAGERSP.hasLevelTwoSave1 && !hasGottenSavePoint)
        {
            rainScript.RainIntensity += 0.2f;
            hasGottenSavePoint = true;
        }
        //if herocontroller.hasblueskull && hasn't done this already
        //rainmaker.intensity += value


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
