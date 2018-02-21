using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OverworldSelectors : MonoBehaviour
{
    public GameObject SE_WrongSound;
    public GameObject currentSelector;
    //Simply spins the power up and moves it up and down.
    void Start()
    {

    }
    //if you interact with it...
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Interact"))
        {
            if (currentSelector.gameObject.name.Equals("SelectorOneToOverworld"))
            {
                GAMEMANAGERSP.numScore += 100;
                GAMEMANAGERSP.hasFinishedLevelOne = true;
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                SceneManager.LoadScene("ExplorationOverworld");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorTwoToOverworld"))
            {
                GAMEMANAGERSP.numScore += 200;
                GAMEMANAGERSP.hasFinishedLevelTwo = true;
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                SceneManager.LoadScene("ExplorationOverworld");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorThreeToOverworld"))
            {
                GAMEMANAGERSP.numScore += 300;
                GAMEMANAGERSP.hasFinishedLevelThree = true;
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                SceneManager.LoadScene("ExplorationOverworld");
            }
            //This is located after boss battle in lvl 4, technically a different scene!
            else if (currentSelector.gameObject.name.Equals("SelectorFinalToEnding")) 
            {
                int scoreSilver = 4000; //CDC this exists in EndingScriptSP.cs too!
                int scoreGold = 9000;
                GAMEMANAGERSP.numScore += 500;
                GAMEMANAGERSP.hasFinishedLevelFour = true;
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();

                if (GAMEMANAGERSP.numScore < scoreSilver)
                {
                    SceneManager.LoadScene("Ending_3Bronze");
                }
                else if (GAMEMANAGERSP.numScore >= scoreSilver && GAMEMANAGERSP.numScore < scoreGold)
                {
                    SceneManager.LoadScene("Ending_2Silver");
                }
                else
                {
                    SceneManager.LoadScene("Ending_1Gold");
                }
            }
            else if (currentSelector.gameObject.name.Equals("SelectorBonusToOverworld"))
            {
                GAMEMANAGERSP.numScore += 400;
                GAMEMANAGERSP.hasFinishedLevelBonus = true;
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                SceneManager.LoadScene("ExplorationOverworld");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorLvlOne"))
            {
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                SceneManager.LoadScene("LevelOne");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorLvlTwo"))
            {
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                SceneManager.LoadScene("LevelTwo");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorLvlThree"))
            {
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                SceneManager.LoadScene("LevelThree");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorLvlFour"))
            {
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                SceneManager.LoadScene("LevelFour");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorLvlBonus"))
            {
                if (GAMEMANAGERSP.numScore >= 2000)
                {
                    GAMEMANAGERSP.saveCurrentState();
                    GAMEMANAGERSP.saveGameDataToFILE();
                    SceneManager.LoadScene("LevelBonus");
                }
                else
                {
                    Instantiate(SE_WrongSound, this.transform.position, this.transform.rotation);
                }

            }
            else if (currentSelector.gameObject.name.Equals("FinalBossBattle"))
            {
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                SceneManager.LoadScene("FinalBossBattle");
            }

        }
        else if (other.gameObject.name.Contains("Frog"))
        {
            //when frog is knocked into level four...
            if (currentSelector.gameObject.name.Equals("SelectorFourToOverworld"))
            {
                HeroControllerSP.battery += 50;
                HeroControllerSP.Gears += 500;
                HeroControllerSP.Ammo += 1000;
                GAMEMANAGERSP.hasFinishedLevelFour = true;
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                SceneManager.LoadScene("ExplorationOverworld");
            }
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        spinPowerUp();
        //movePowerUp();   //causes horizontal movement over time unfortunately due to rotation. It's not up and down as desired.
    }

    void spinPowerUp()
    {
        currentSelector.transform.Rotate(new Vector3(30, 45, 60) * Time.deltaTime);
    }

    //CDC - find out how to move it up and down relative to ground; not up relative to its current orientation.
    //void movePowerUp()
    //{
    //    currentSelector.transform.Translate(new Vector3(0.03f, 0, 0) * Time.deltaTime);
    //}
}
