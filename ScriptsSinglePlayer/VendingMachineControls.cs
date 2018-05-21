using UnityEngine;
using System.Collections;

public class VendingMachineControls : MonoBehaviour {
    public int whichControllerNumberAmI;

    public AudioClip switchHit;
    public AudioClip errorSound;
    private AudioSource source;
    public GameObject spawnLoc;
    public GameObject SE_Explosion;

    public GameObject slot1Item;
    public GameObject slot2Item;
    public GameObject slot3Item;
    public GameObject slot4Item;
    public GameObject slot5Item;

    public int slot1Cost = 2;
    public int slot2Cost = 2;
    public int slot3Cost = 5;
    public int slot4Cost = 3;
    public int slot5Cost = 1;

    private bool isArena;


    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        GameObject arenaManager = GameObject.Find("GAMEMANAGER_ARENA");
        if (arenaManager != null)
        {
            isArena = true;
        }
        else
        {
            isArena = false;
        }
	}

    public void OnCollisionEnter(Collision other)
    {
        if (isArena)
        {
            processArenaLogic(other);
        }
        else
        {
            processSPLogic(other);
        }
        
    }

    // Update is called once per frame
    void Update ()
    {

    }

    private void processArenaLogic(Collision other)
    {
        if (other.gameObject.name.Contains("PlayerShot") || other.gameObject.name.Contains("InteractShot"))
        {

            if (whichControllerNumberAmI == 1)
            {
                if (HeroController.Gears >= slot1Cost)
                {
                    source.PlayOneShot(switchHit, 1.0f);
                    HeroController.Gears -= slot1Cost;
                    Instantiate(slot1Item, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    Instantiate(SE_Explosion, spawnLoc.transform.position, spawnLoc.transform.rotation);
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }

            else if (whichControllerNumberAmI == 2)
            {

                if (HeroController.Gears >= slot2Cost)
                {
                    source.PlayOneShot(switchHit, 1.0f);
                    HeroController.Gears -= slot2Cost;
                    Instantiate(slot2Item, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    Instantiate(SE_Explosion, spawnLoc.transform.position, spawnLoc.transform.rotation);
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }

            }
            else if (whichControllerNumberAmI == 3)
            {

                if (HeroController.Gears >= slot3Cost)
                {
                    source.PlayOneShot(switchHit, 1.0f);
                    HeroController.Gears -= slot3Cost;
                    Instantiate(slot3Item, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    Instantiate(SE_Explosion, spawnLoc.transform.position, spawnLoc.transform.rotation);
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }

            }

            else if (whichControllerNumberAmI == 4)
            {
                if (HeroController.Gears >= slot4Cost)
                {
                    source.PlayOneShot(switchHit, 1.0f);
                    HeroController.Gears -= slot4Cost;
                    Instantiate(slot4Item, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    Instantiate(SE_Explosion, spawnLoc.transform.position, spawnLoc.transform.rotation);
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }

            }
            else if (whichControllerNumberAmI == 5)
            {

                if (HeroController.Gears >= slot5Cost)
                {
                    source.PlayOneShot(switchHit, 1.0f);
                    HeroController.Gears -= slot5Cost;
                    Instantiate(slot5Item, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    Instantiate(SE_Explosion, spawnLoc.transform.position, spawnLoc.transform.rotation);
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }

            }

        }
    }


    private void processSPLogic(Collision other)
    {
        if (other.gameObject.name.Contains("PlayerShot") || other.gameObject.name.Contains("InteractShot"))
        {

            if (whichControllerNumberAmI == 1)
            {
                if (HeroControllerSP.Gears >= slot1Cost)
                {
                    source.PlayOneShot(switchHit, 1.0f);
                    HeroControllerSP.Gears -= slot1Cost;
                    Instantiate(slot1Item, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    Instantiate(SE_Explosion, spawnLoc.transform.position, spawnLoc.transform.rotation);
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }

            else if (whichControllerNumberAmI == 2)
            {

                if (HeroControllerSP.Gears >= slot2Cost)
                {
                    source.PlayOneShot(switchHit, 1.0f);
                    HeroControllerSP.Gears -= slot2Cost;
                    Instantiate(slot2Item, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    Instantiate(SE_Explosion, spawnLoc.transform.position, spawnLoc.transform.rotation);
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }

            }
            else if (whichControllerNumberAmI == 3)
            {

                if (HeroControllerSP.Gears >= slot3Cost)
                {
                    source.PlayOneShot(switchHit, 1.0f);
                    HeroControllerSP.Gears -= slot3Cost;
                    Instantiate(slot3Item, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    Instantiate(SE_Explosion, spawnLoc.transform.position, spawnLoc.transform.rotation);
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }

            }

            else if (whichControllerNumberAmI == 4)
            {
                if (HeroControllerSP.Gears >= slot4Cost)
                {
                    source.PlayOneShot(switchHit, 1.0f);
                    HeroControllerSP.Gears -= slot4Cost;
                    Instantiate(slot4Item, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    Instantiate(SE_Explosion, spawnLoc.transform.position, spawnLoc.transform.rotation);
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }

            }
            else if (whichControllerNumberAmI == 5)
            {

                if (HeroControllerSP.Gears >= slot5Cost)
                {
                    source.PlayOneShot(switchHit, 1.0f);
                    HeroControllerSP.Gears -= slot5Cost;
                    Instantiate(slot5Item, spawnLoc.transform.position, spawnLoc.transform.rotation);
                    Instantiate(SE_Explosion, spawnLoc.transform.position, spawnLoc.transform.rotation);
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }

            }

        }
    }
}
