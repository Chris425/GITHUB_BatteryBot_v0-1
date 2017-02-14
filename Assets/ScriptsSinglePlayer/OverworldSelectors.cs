using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OverworldSelectors : MonoBehaviour
{
    public GameObject currentSelector;
    //Simply spins the power up and moves it up and down.
    void Start()
    {

    }
    //if you interact with it...
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Shot"))
        {
            if (currentSelector.gameObject.name.Equals("SelectorOneToOverworld"))
            {
                GAMEMANAGERSP.hasFinishedLevelOne = true;
                SceneManager.LoadScene("ExplorationOverworld");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorTwoToOverworld"))
            {
                GAMEMANAGERSP.hasFinishedLevelTwo = true;
                SceneManager.LoadScene("ExplorationOverworld");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorThreeToOverworld"))
            {
                GAMEMANAGERSP.hasFinishedLevelThree = true;
                SceneManager.LoadScene("ExplorationOverworld");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorBonusToOverworld"))
            {
                GAMEMANAGERSP.hasFinishedLevelBonus = true;
                SceneManager.LoadScene("ExplorationOverworld");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorLvlOne"))
            {
                SceneManager.LoadScene("LevelOne");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorLvlTwo"))
            {
                SceneManager.LoadScene("LevelTwo");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorLvlThree"))
            {
                SceneManager.LoadScene("LevelThree");
            }
            else if (currentSelector.gameObject.name.Equals("SelectorLvlBonus"))
            {
                SceneManager.LoadScene("LevelBonus");
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
