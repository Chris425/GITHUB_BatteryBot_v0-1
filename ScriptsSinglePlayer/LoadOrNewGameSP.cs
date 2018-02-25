using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class LoadOrNewGameSP : MonoBehaviour
{
    public Light light1;
    public Light light2;
    public Light light3;
    public GameObject Selector1;
    public GameObject Selector2;
    public GameObject Selector3;

    private bool isSelector1;
    private bool isSelector2;
    private bool isSelector3;

    public AudioClip boop;
    public Text statusText;
    public GameObject SE_ErrorSound;
    Color purpleColour;

    private bool hasShownWarning = false;

    private AudioSource source;

    int selectorChoice;

    void Awake()
    {
        purpleColour = new Color32(0xB1, 0xAB, 0xFF, 0xFF); 
        hasShownWarning = false;
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        selectorChoice = 0;
        light1.intensity = 0.0f;
        light2.intensity = 0.0f;
        light3.intensity = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        keyPressGame();
        handleSelectors();
    }

    private void handleSelectors()
    {
        
        //Load game far left
        if (selectorChoice % 3 == 0)
        {
            isSelector1 = true;
            isSelector2 = false;
            isSelector3 = false;
            light1.intensity = 8.0f;
            light2.intensity = 0.0f;
            light3.intensity = 0.0f;
            spinSelector(1);

        }
        //New game, far right
        else if (selectorChoice % 3 == 1)
        {
            isSelector1 = false;
            isSelector2 = true;
            isSelector3 = false;
            light1.intensity = 0.0f;
            light2.intensity = 8.0f;
            light3.intensity = 0.0f;
            spinSelector(2);
        }
        //go back. Middle, top.
        else if (selectorChoice % 3 == 2)
        {
            isSelector1 = false;
            isSelector2 = false;
            isSelector3 = true;
            light1.intensity = 0.0f;
            light2.intensity = 0.0f;
            light3.intensity = 8.0f;
            spinSelector(3);
        }
    }

    private void spinSelector(int selNum)
    {
        if (selNum == 1)
        {
            Selector1.transform.Rotate(new Vector3(30, 45, 60) * 2 * Time.deltaTime);
        }
        else if (selNum == 2)
        {
            Selector2.transform.Rotate(new Vector3(30, 45, 60) * 2 * Time.deltaTime);
        }
        else if (selNum == 3)
        {
            Selector3.transform.Rotate(new Vector3(30, 45, 60) * 2 * Time.deltaTime);
        }
    }



    public void button_Load()
    {
        try
        {
            GAMEMANAGERSP.loadGameDataFromFILE();
            SceneManager.LoadScene("ExplorationOverworld");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            Instantiate(SE_ErrorSound, this.transform.position, this.transform.rotation);
            statusText.text = "Could not find a valid save file. Try starting a new game. \r\n If the problem persists, contact the developer.";
            statusText.color = Color.red;
        }
    }
    public void button_New()
    {
        if (hasShownWarning)
        {
            //make new save - should all be empty. So clear GameManager specific variables first (these aren't cleared on gameover).
            GAMEMANAGERSP.hasFinishedLevelOne = false;
            GAMEMANAGERSP.hasFinishedLevelTwo = false;
            GAMEMANAGERSP.hasFinishedLevelThree = false;
            GAMEMANAGERSP.hasFinishedLevelFour = false;
            GAMEMANAGERSP.hasFinishedLevelBonus = false;
            GAMEMANAGERSP.numScore = 0;
            GAMEMANAGERSP.numQualitySetting = QualitySettings.GetQualityLevel();
            //now wipe gamestate like a normal gameover would.
            GAMEMANAGERSP.wipeGameState();
            //New game should also wipe skulls and bazooka though
            GAMEMANAGERSP.hasBazooka = false;
            GAMEMANAGERSP.hasSkull_GOLD = false;
            GAMEMANAGERSP.hasSkull_SILVER = false;
            GAMEMANAGERSP.hasSkull_BRONZE = false;
            GAMEMANAGERSP.saveGameDataToFILE();
            SceneManager.LoadScene("ExplorationOverworld");
        }
        else
        {
            hasShownWarning = true;
            statusText.text = "WARNING: THIS WILL OVERWRITE ANY DATA THAT CURRENTLY EXISTS.";
            Instantiate(SE_ErrorSound, this.transform.position, this.transform.rotation);
            statusText.color = Color.red;
        }
    }
    public void button_Back()
    {
        SceneManager.LoadScene("Intro");
    }


    private void keyPressGame()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {

                //LOAD GAME
                if (isSelector1)
                {
                    button_Load();
                }
                //NEW GAME
                else if (isSelector2)
                {
                    if (hasShownWarning)
                    {
                        button_New();
                    }
                    else
                    {
                        hasShownWarning = true;
                        statusText.text = "WARNING: THIS WILL OVERWRITE ANY DATA THAT CURRENTLY EXISTS.";
                        Instantiate(SE_ErrorSound, this.transform.position, this.transform.rotation);
                        statusText.color = Color.red;
                    }
                }
                //Back to intro
                else if (isSelector3)
                {
                    button_Back();
                }
            



        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (hasShownWarning)
            {
                hasShownWarning = false;
                statusText.text = "Load from an existing file or start a new game.";
                statusText.color = purpleColour;
            }
            else
            {
                String currScene = SceneManager.GetActiveScene().name;
                if (currScene.Equals("GameoverSP"))
                {
                    SceneManager.LoadScene("Intro");
                }
                else
                {
                    Application.Quit();
                }
            }


        }
        //change selectors
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            hasShownWarning = false;
            statusText.text = "Load from an existing file or start a new game.";
            statusText.color = purpleColour;

            source.PlayOneShot(boop, 0.7f);
            if (selectorChoice > 0)
            {
                selectorChoice -= 1;
            }

        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            hasShownWarning = false;
            statusText.text = "Load from an existing file or start a new game.";
            statusText.color = purpleColour;

            selectorChoice += 1;
            source.PlayOneShot(boop, 0.7f);
        }
    }
}
