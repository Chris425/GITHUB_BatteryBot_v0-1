using UnityEngine;
using System.Collections;

/*
*To be used for testing purposes only; they are automatically given at the ending screen on normal play
*/



public class Skull_metalPickUp : MonoBehaviour
{
    public GameObject SE_Gear;
    public string skullName;

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
            yield return new WaitForSeconds(1.0f);
            transform.Rotate(new Vector3(0.0f, 5.5f, 0.0f) * 5.0f * Time.deltaTime);
            yield return new WaitForSeconds(1.0f);
        }

    }


    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            if (skullName.ToUpper() == "BRONZE")
            {
                HeroControllerSP.hasSkull_BRONZE = true;
                Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
            else if (skullName.ToUpper() == "SILVER")
            {
                HeroControllerSP.hasSkull_SILVER = true;
                Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
            else if (skullName.ToUpper() == "GOLD")
            {
                HeroControllerSP.hasSkull_GOLD = true;
                Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }

        }
    }
}
