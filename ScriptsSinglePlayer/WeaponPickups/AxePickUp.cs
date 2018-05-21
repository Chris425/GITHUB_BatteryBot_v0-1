using UnityEngine;
using System.Collections;

public class AxePickUp : MonoBehaviour {
    
    public GameObject axe;
    public GameObject gun;
    public GameObject GS;

    // Use this for initialization
    void Start () {
	
	}
    // Update is called once per frame
    void Update()
    {
        StartCoroutine("spinPowerUp");
    }

    IEnumerator spinPowerUp()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(2.0f);
            transform.Rotate(new Vector3(0.0f, 0.0f, 3.3f) * 5.0f * Time.deltaTime);
            yield return new WaitForSeconds(3.0f);
        }

    }

    //Not in use
    //public void dequipWeapon()
    //{
    //    if (HeroControllerSP.hasGun)
    //    {
    //        Instantiate(gun, this.transform.position, this.transform.rotation);
    //    }
    //    else if (HeroControllerSP.hasAxe)
    //    {
    //        Instantiate(axe, this.transform.position, this.transform.rotation);
    //    }
    //    else if (HeroControllerSP.hasGS)
    //    {
    //        Instantiate(GS, this.transform.position, this.transform.rotation);
    //    }
    //}

    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            //list of right handed weapons which would conflict with each other.
            //Assumption - greatsword is 2h but will not conflict with shield; it's more of a buckler?
            // dequipWeapon();
            GAMEMANAGERSP.numScore += 2;
            GAMEMANAGERSP.numArenaScore += 2;
            HeroControllerSP.hasAxe = true;
            HeroControllerSP.isSlot1 = true;
            HeroControllerSP.isSlot2 = false; HeroControllerSP.isSlot3 = false;

            HeroController.hasAxe = true;
            HeroController.isSlot1 = true;
            HeroController.isSlot2 = false; HeroController.isSlot3 = false;
            Destroy(this.gameObject);
        }
    }
}
