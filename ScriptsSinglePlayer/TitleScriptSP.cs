using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScriptSP : MonoBehaviour
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
           //Go back to whichever scene you died from!
            SceneManager.LoadScene(HeroControllerSP.currScene);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Intro");
        }
    }
}
