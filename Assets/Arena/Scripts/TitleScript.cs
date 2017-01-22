using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class TitleScript : MonoBehaviour
{
    public Light light1;
    public Light light2;
    public GameObject Selector1;
    public GameObject Selector2;

    private bool isSelector1;
    private bool isSelector2;

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
      selectorChoice = 0;
        light1.intensity = 0.0f;
        light2.intensity = 0.0f;
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
        if (selectorChoice % 2 == 0)
        {
            isSelector1 = true;
            isSelector2 = false;
            light1.intensity = 8.0f;
            light2.intensity = 0.0f;            
            spinSelector(1);

        }
        //exploration mode, far right side
        else if (selectorChoice % 2 == 1)
        {
            isSelector1 = false;
            isSelector2 = true;
            light1.intensity = 0.0f;
            light2.intensity = 8.0f;           
            spinSelector(2);
        }
    }

    private void spinSelector(int selNum)
    {
        if (selNum == 1)
        {
            Selector1.transform.Rotate(new Vector3(30, 45, 60) * 2 *Time.deltaTime);
            Selector1.transform.Translate(new Vector3(0.1f, 0, 0) * 2 *Time.deltaTime);
        }
        else if (selNum == 2)
        {
            Selector2.transform.Rotate(new Vector3(30, 45, 60) * 2 * Time.deltaTime);
            Selector2.transform.Translate(new Vector3(0.1f, 0, 0) * 2 *Time.deltaTime);
        }
    }

    private void keyPressGame()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {
            if (isSelector1)
            {
                SceneManager.LoadScene("MainArena");
            }
            else if(isSelector2)
            {
                SceneManager.LoadScene("ExplorationOverworld");
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            int currScene = SceneManager.GetActiveScene().buildIndex;
            if (currScene == 2)
            {
                SceneManager.LoadScene("Intro");
            }
            else
            {
                Application.Quit();
            }
            
        }
        //change selectors
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            source.PlayOneShot(boop, 0.7f);
            if (selectorChoice > 0)
            {
                selectorChoice -= 1;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectorChoice += 1;
            source.PlayOneShot(boop, 0.7f);
        }
    }
}
