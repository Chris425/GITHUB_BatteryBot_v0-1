using UnityEngine;
using System.Collections;

public class GS_FlamePickUp : MonoBehaviour
{
    public GameObject Pickup;
    public GameObject SE_Gear;
    private GameObject GreatswordFire;

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
        transform.Rotate(new Vector3(20, 35, 50) * Time.deltaTime);
    }



    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {       
            HeroControllerSP.hasGS_FIRE = true;
            Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
