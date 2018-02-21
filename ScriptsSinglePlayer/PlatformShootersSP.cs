using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformShootersSP : MonoBehaviour
{

    public GameObject warning;
    public GameObject explosion;
    public AudioClip warningSound;
    public AudioClip explosionSound;
    private AudioSource source;
    public List<GameObject> spawnLocs = new List<GameObject>();
    
    private float cooldownTimer = 0.0f;
    private int counter = 0;
    private int patternChoice;

    // Use this for initialization
    void OnEnable()
    {
        //CDC - this doesn't work because it adds it an undefined order. Doing it manually.

        //foreach (GameObject spawnLoc in GameObject.FindObjectsOfType<GameObject>())
        //{
        //    if (spawnLoc.name.Contains("GridSpawnLoc"))
        //    {
        //        spawnLocs.Add(spawnLoc);
        //    }
        //}
        
        source = GetComponent<AudioSource>();
        patternChoice = 1;
    }

    // Update is called once per frame
    void Update()
    {

        cooldownTimer -= 0.01f;



        if (!HeroControllerSP.isPaused)
        {
            if (cooldownTimer <= 0.01f)
            {
                if (counter % 2 == 0) //alternates between warning and explosion
                {
                    patternChoice = Random.Range(1, 8);

                    ChoosePatternWarning(patternChoice);

                    cooldownTimer = 3.0f; 
                    counter += 1;
                }
                else
                {

                    ChoosePatternExplosion(patternChoice);

                    cooldownTimer = 3.0f;
                    counter += 1;
                }
            }



        }
    }


    void ChoosePatternWarning(int numChoice)
    {
        switch (numChoice)
        {
            //Left
            case 1:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i <= 3))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.3f);
                break;
            //Middle
            case 2:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 4) && (i <= 7))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.3f);
                break;
            //Right
            case 3:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 8))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.3f);
                break;
            //Left and Right
            case 4:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 8) || (i <= 3))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.3f);
                break;
            //Left and Middle
            case 5:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 4) && (i <= 7) || (i <= 3))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.3f);
                break;
            //Middle and Right
            case 6:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 4) && (i <= 7) || (i >= 8))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.3f);
                break;
            //ALL YOLO
            case 7:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);                    
                }
                source.PlayOneShot(warningSound, 0.3f);
                break;   
            //just middle
            default:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 4) && (i <= 7))
                    {
                        Instantiate(warning, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.3f);
                break;
        }
    }

    void ChoosePatternExplosion(int numChoice)
    {
        switch (numChoice)
        {
            //Left
            case 1:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i <= 3))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(explosionSound, 0.7f);
                break;
            //Middle
            case 2:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 4) && (i <= 7))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(explosionSound, 0.7f);
                break;
            //Right
            case 3:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 8))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(explosionSound, 0.7f);
                break;
            //Left and Right
            case 4:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 8) || (i <= 3))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(explosionSound, 0.7f);
                break;
            //Left and Middle
            case 5:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 4) && (i <= 7) || (i <= 3))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(explosionSound, 0.7f);
                break;
            //Middle and Right
            case 6:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 4) && (i <= 7) || (i >= 8))
                    {
                        Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(explosionSound, 0.7f);
                break;
            //ALL YOLO
            case 7:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    Instantiate(explosion, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                }
                source.PlayOneShot(explosionSound, 0.7f);
                break;
            //just middle
            default:
                for (int i = 0; i < spawnLocs.Count; i++)
                {
                    if ((i >= 4) && (i <= 7))
                    {
                        Instantiate(explosionSound, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
                    }
                }
                source.PlayOneShot(warningSound, 0.7f);
                break;
        }
    }







}
