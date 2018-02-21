using UnityEngine;
using System.Collections;

public class LegArmourPickup : MonoBehaviour
{
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
            yield return new WaitForSeconds(2.0f);
            transform.Rotate(new Vector3(0.0f, 0.0f, 3.3f) * 5.0f * Time.deltaTime);
            yield return new WaitForSeconds(3.0f);
        }

    }



    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            HeroControllerSP.hasLegs = true;
            GAMEMANAGERSP.numScore += 45;
            Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
