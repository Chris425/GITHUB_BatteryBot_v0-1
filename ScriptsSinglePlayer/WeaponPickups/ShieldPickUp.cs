using UnityEngine;
using System.Collections;

public class ShieldPickUp : MonoBehaviour
{

    public GameObject shield;
    

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
            transform.Rotate(new Vector3(0.0f, 0.0f, 3.3f) * 5.0f * Time.deltaTime);
            yield return new WaitForSeconds(3.0f);
        }

    }

    public void dequipLeftHanded()
    {
        if (HeroControllerSP.hasShield)
        {
            Instantiate(shield, this.transform.position, this.transform.rotation);
        }
        
    }

    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            //list of left handed weapons which would conflict with each other.
            //dequipLeftHanded();
            GAMEMANAGERSP.numScore += 5;
            HeroControllerSP.hasShield = true;
            HeroControllerSP.isSlot4 = true;
            HeroControllerSP.isSlot5 = false;
            Destroy(this.gameObject);
        }
    }
}
