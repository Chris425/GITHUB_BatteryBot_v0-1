using UnityEngine;
using System.Collections;

public class ShieldBlockBossSP : MonoBehaviour
{

    public GameObject blockSound;
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
        int blockChance = Random.Range(1, 30);

        if (other.gameObject.name.Contains("Shot"))
        {
            //note that with this if statement we create the probability of being hit by modifying the numbers
            if (blockChance > 5)
            {
                //block succeeds
                Instantiate(blockSound, this.transform.position, this.transform.rotation);
                Destroy(other.gameObject);
            }

            //else block fails and you get hit as normal since we are not destroying the projectile            
        }

        

    }
}
