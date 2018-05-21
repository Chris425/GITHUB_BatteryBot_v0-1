using UnityEngine;
using System.Collections;

public class Skull_RedPickup : MonoBehaviour
{
    public GameObject Pickup;
    public GameObject SE_Gear;

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
            HeroControllerSP.hasSkull_RED = true;
            HeroController.hasSkull_RED = true;
            GAMEMANAGERSP.numScore += 10;
            GAMEMANAGERSP.numArenaScore += 10;
            Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
