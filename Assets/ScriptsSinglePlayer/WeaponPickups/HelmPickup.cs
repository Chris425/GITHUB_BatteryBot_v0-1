using UnityEngine;
using System.Collections;

public class HelmPickup : MonoBehaviour {
    

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        spinPowerUp();
    }

    void spinPowerUp()
    {
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
    }

    

    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {            
            HeroControllerSP.hasHelm = true;
           
            Destroy(this.gameObject);
        }
    }
}
