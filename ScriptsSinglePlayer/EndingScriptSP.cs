using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingScriptSP : MonoBehaviour
{

    public Text score;
    private int scoreSilver;
    private int scoreGold;
    private Animator anim;

    // Use this for initialization
    void Start()
    {
        scoreSilver = 4000;
        scoreGold = 9000;
        anim = this.GetComponentInChildren<Animator>();


        //CDC note that this check is mostly redundant, also occurs during portal from final boss to choose an ending scene
        //this exists in OverworldSelectors.cs too!
        score.text = "Score: " + GAMEMANAGERSP.numScore;
        if (GAMEMANAGERSP.numScore < scoreSilver)
        {
            anim.applyRootMotion = false;
            HeroControllerSP.hasSkull_BRONZE = true;
            anim.SetTrigger("isBronze");
        }
        else if (GAMEMANAGERSP.numScore >= scoreSilver && GAMEMANAGERSP.numScore < scoreGold)
        {
            anim.applyRootMotion = false;
            HeroControllerSP.hasSkull_SILVER = true;
            anim.SetTrigger("isSilver");            
        }
        else
        {
            HeroControllerSP.hasSkull_GOLD = true;
            anim.SetTrigger("isGold");
        }

        GAMEMANAGERSP.saveCurrentState();
        //GAMEMANAGERSP.saveGameDataToFILE();
        
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
            //Go back to start with your new skull key.
            SceneManager.LoadScene("ExplorationOverworld");
        }
    }
}
