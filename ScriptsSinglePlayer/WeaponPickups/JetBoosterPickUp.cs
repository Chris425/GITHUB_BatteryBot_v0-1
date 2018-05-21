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
        StartCoroutine("spinPowerUp");
    }

    IEnumerator spinPowerUp()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(2.0f);
            transform.Rotate(new Vector3(2.0f, 3.5f, 5.0f) * 5.0f * Time.deltaTime);
            yield return new WaitForSeconds(3.0f);
        }

    }
    //deprecated
    //public void dequipLeftHanded()
    //{
    //    if (HeroControllerSP.hasJetBooster)
    //    {
    //        Instantiate(jetBooster, this.transform.position, this.transform.rotation);
    //    }

    //}

    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            //list of left handed weapons which would conflict with each other.
            //dequipLeftHanded();
            GAMEMANAGERSP.numScore += 9;
            HeroControllerSP.hasJetBooster = true;
            HeroControllerSP.isSlot5 = true;
            HeroControllerSP.isSlot4 = false;
            
            GAMEMANAGERSP.numArenaScore += 9;
            HeroController.hasJetBooster = true;
            HeroController.isSlot5 = true;
            HeroController.isSlot4 = false;

            Destroy(this.gameObject);
        }
    }
}
