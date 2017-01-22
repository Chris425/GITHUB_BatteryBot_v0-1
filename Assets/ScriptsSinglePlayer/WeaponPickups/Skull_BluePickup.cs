using UnityEngine;
using System.Collections;

public class Skull_BluePickup : MonoBehaviour
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
        spinPowerUp();
    }

    void spinPowerUp()
    {
        transform.Rotate(new Vector3(0, 55, 0) * Time.deltaTime);
    }



    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            HeroControllerSP.hasSkull_BLUE = true;
            Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
