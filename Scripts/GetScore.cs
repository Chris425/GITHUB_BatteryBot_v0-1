using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class GetScore : MonoBehaviour
{
    public Text currScoreUI;
    public Text highScoreUI;

    // Use this for initialization
    void Start()
    {
        populateScores();
    }

    private void populateScores()
    {
        int currScoreInt = GAMEMANAGERSP.numArenaScore;
        int highScoreInt = GAMEMANAGERSP.numArenaHighScore;
        if (currScoreInt > highScoreInt)
        {
            highScoreInt = currScoreInt;
        }
        currScoreUI.text = "Current score: " + currScoreInt;
        highScoreUI.text = "HIGH SCORE:" + highScoreInt;
    }

    // Update is called once per frame
    void Update()
    {
        keyPressGame();
    }

    private void keyPressGame()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {

            SceneManager.LoadScene("MainArena");


        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            SceneManager.LoadScene("Intro");

        }
    }
}
