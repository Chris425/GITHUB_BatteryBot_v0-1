using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverSP : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

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
