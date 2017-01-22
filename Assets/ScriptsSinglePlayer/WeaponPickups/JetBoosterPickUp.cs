using UnityEngine;
using System.Collections;

public class JetBoosterPickUp : MonoBehaviour
{

    public GameObject jetBooster;


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

    public void dequipLeftHanded()
    {
        if (HeroControllerSP.hasJetBooster)
        {
            Instantiate(jetBooster, this.transform.position, this.transform.rotation);
        }

    }

    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            //list of left handed weapons which would conflict with each other.
            //dequipLeftHanded();
            HeroControllerSP.hasJetBooster = true;
            HeroControllerSP.isSlot5 = true;
            HeroControllerSP.isSlot4 = false;
            Destroy(this.gameObject);
        }
    }
}
