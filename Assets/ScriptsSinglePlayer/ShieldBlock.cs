using UnityEngine;
using System.Collections;

public class ShieldBlock : MonoBehaviour {

    public GameObject blockSound;
    public GameObject sparkSE;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter(Collision other)
    {
        //quite simply, if a shot hits the shield it will be destroyed. Therefore it won't hit the player.
        // casterShot may change in the future - or make all enemy projectiles named with this convention -CDC
        if (other.gameObject.name.Contains("CasterShot"))
        {
            Debug.Log("Successfully blocked projectile!!");
            Instantiate(blockSound, this.transform.position, this.transform.rotation);
            Destroy(other.gameObject);
        }
    }
}
