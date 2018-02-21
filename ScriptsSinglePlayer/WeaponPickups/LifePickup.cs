using UnityEngine;
using System.Collections;

public class LifePickup : MonoBehaviour
{
    public int lifeWorth = 1;
    public GameObject SE_Gear;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


   

    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            //Increase Hero's gear count. Sort of a currency.        
            HeroControllerSP.Lives += lifeWorth;
            GAMEMANAGERSP.numScore += 100;
            Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
