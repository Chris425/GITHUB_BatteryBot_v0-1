using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class TitleScript : MonoBehaviour
{
    public GameObject SE_introMusicBot;
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

    private AudioSource source;

    int selectorChoice;

    void Awake()
    {
        
        source = GetComponent<AudioSource>();

    }


    // Use this for initialization
    void Start()
    {
        //find intromusic gameobject
        //if not exists then make else do nothing
        if (GAMEMANAGERSP.shouldMakeMusicBot == true)
        {
            Instantiate(SE_introMusicBot);
            GAMEMANAGERSP.shouldMakeMusicBot = false;
        }


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
        //change modulus 2 to 3 if you add another mode... -CDC
        //arena mode, far left side
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
       //quit
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
        //exploration mode, far right side
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
            Selector1.transform.Rotate(new Vector3(30, 45, 60) * 2 *Time.deltaTime);
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



    public void button_GoToArena()
    {
        //see if arena file currently exists
        String arenaSaveFilepath = GAMEMANAGERSP.arenaPath;
        if (!File.Exists(arenaSaveFilepath))
        {
            GAMEMANAGERSP.numArenaScore = 0;
            GAMEMANAGERSP.numArenaHighScore = 0;
            //write zeros to file.
            StreamWriter writer = new StreamWriter(arenaSaveFilepath, false);
            writer.WriteLine("0");
            writer.Close();
        }
        else
        {
            //file exists - so take that int and mark it as the current high score.
            StreamReader reader = new StreamReader(arenaSaveFilepath);
            try
            {
                string currHighScore = reader.ReadLine();
                reader.Close();
                GAMEMANAGERSP.numArenaScore = 0;
                GAMEMANAGERSP.numArenaHighScore = Int32.Parse(currHighScore);
                GAMEMANAGERSP.saveArenaScoreToFILE();
            }
            catch (Exception ex)
            {

                Debug.Log(ex.ToString());
                Debug.Log("Failed to find current high score, setting it to 0.");
            }
        }
        //load main arena
        SceneManager.LoadScene("MainArena");
    }
    public void button_GoToLoadOrNew()
    {
        SceneManager.LoadScene("Intro_LoadOrNew");
    }
    public void button_Quit()
    {
        Application.Quit();
    }

    private void keyPressGame()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {
            if (isSelector1)
            {
                button_GoToArena();
            }
            else if (isSelector2)
            {
                button_GoToLoadOrNew();
            }
            else if(isSelector3)
            {
                Debug.Log("EX");
                button_Quit();
                
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            String currScene = SceneManager.GetActiveScene().name;
            if (currScene.Equals("GameoverSP"))
            {
                SceneManager.LoadScene("Intro");
            }
            else
            {
                button_Quit();
            }
            
        }
        //change selectors
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            source.PlayOneShot(boop, 0.7f);
            if (selectorChoice > 0)
            {
                selectorChoice -= 1;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectorChoice += 1;
            source.PlayOneShot(boop, 0.7f);
        }
    }
}
