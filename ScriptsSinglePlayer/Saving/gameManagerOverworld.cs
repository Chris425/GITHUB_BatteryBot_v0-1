using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerOverworld : MonoBehaviour {



    void Start () {


        GameObject xmarkLevelOne = GameObject.Find("XLevelMarkLvl1");
        GameObject checkmarkLevelOne = GameObject.Find("CheckLevelMarkLvl1");

        GameObject xmarkLevelTwo= GameObject.Find("XLevelMarkLvl2");
        GameObject checkmarkLevelTwo = GameObject.Find("CheckLevelMarkLvl2");

        GameObject xmarkLevelThree = GameObject.Find("XLevelMarkLvl3");
        GameObject checkmarkLevelThree= GameObject.Find("CheckLevelMarkLvl3");

        GameObject xmarkLevelBonus = GameObject.Find("XLevelMarkLvlBonus");
        GameObject checkmarkLevelBonus = GameObject.Find("CheckLevelMarkLvlBonus");

        GameObject World2Portal = GameObject.Find("LevelWellSelector-WorldTwo");
        
        
        //lvl1
        if (GAMEMANAGERSP.hasFinishedLevelOne)
        {
            checkmarkLevelOne.gameObject.SetActive(true);
            xmarkLevelOne.gameObject.SetActive(false);
            HeroControllerSP.hasSkull_PURPLE = true;
        }
        else
        {
            checkmarkLevelOne.gameObject.SetActive(false);
            xmarkLevelOne.gameObject.SetActive(true);
            HeroControllerSP.hasSkull_PURPLE = false;

        }

        //lvl2
        if (GAMEMANAGERSP.hasFinishedLevelTwo)
        {
            checkmarkLevelTwo.gameObject.SetActive(true);
            xmarkLevelTwo.gameObject.SetActive(false);
            HeroControllerSP.hasSkull_RED = true;
        }
        else
        {
            checkmarkLevelTwo.gameObject.SetActive(false);
            xmarkLevelTwo.gameObject.SetActive(true);
            HeroControllerSP.hasSkull_RED = false;
        }

        //lvl3
        if (GAMEMANAGERSP.hasFinishedLevelThree)
        {
            checkmarkLevelThree.gameObject.SetActive(true);
            xmarkLevelThree.gameObject.SetActive(false);
            HeroControllerSP.hasSkull_BLUE = true;
        }
        else
        {
            checkmarkLevelThree.gameObject.SetActive(false);
            xmarkLevelThree.gameObject.SetActive(true);
            HeroControllerSP.hasSkull_BLUE = false;
        }

        //bonus lvl
        if (GAMEMANAGERSP.hasFinishedLevelBonus)
        {
            checkmarkLevelBonus.gameObject.SetActive(true);
            xmarkLevelBonus.gameObject.SetActive(false);
            HeroControllerSP.hasSkull_BLUE = true;
            HeroControllerSP.hasSkull_RED = true;
            HeroControllerSP.hasSkull_PURPLE = true;
        }
        else
        {
            checkmarkLevelBonus.gameObject.SetActive(false);
            xmarkLevelBonus.gameObject.SetActive(true);
        }

    }
	
}
