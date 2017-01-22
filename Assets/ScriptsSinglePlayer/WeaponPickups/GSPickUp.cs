using UnityEngine;
using System.Collections;

public class GSPickUp : MonoBehaviour
{
    
    public GameObject axe;
    public GameObject gun;
    public GameObject GS;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spinPowerUp();
    }

    void spinPowerUp()
    {
        transform.Rotate(new Vector3(0, 0, 30) * Time.deltaTime);
    }

    // Not used
    public void dequipWeapon()
    {
        if (HeroControllerSP.hasGun)
        {
            Instantiate(gun, this.transform.position, this.transform.rotation);
        }
        else if (HeroControllerSP.hasAxe)
        {
            Instantiate(axe, this.transform.position, this.transform.rotation);
        }
        else if (HeroControllerSP.hasGS)
        {
            Instantiate(GS, this.transform.position, this.transform.rotation);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            //list of right handed weapons which would conflict with each other.
            //Assumption - greatsword is 2h but will not conflict with shield; it's more of a buckler?
            //dequipWeapon();            
            HeroControllerSP.hasGS = true;
            HeroControllerSP.isSlot3 = true;
            HeroControllerSP.isSlot1 = false; HeroControllerSP.isSlot2 = false; 
            Destroy(this.gameObject);
        }
    }
}
