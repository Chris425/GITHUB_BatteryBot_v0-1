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
        int blockChance = 0;
        if (HeroControllerSP.hasHelm)
        {
             blockChance = Random.Range(1, 20);
        }
        else
        {
             blockChance = Random.Range(1, 40);
        }
        

        if (other.gameObject.name.Contains("CasterShot"))
        {            
            //note that with this if statement we create the probability of being hit by modifying the numbers
            if (blockChance > 8)
            {
                //block succeeds
                Instantiate(blockSound, this.transform.position, this.transform.rotation);
                Destroy(other.gameObject);
            }
            
            //else block fails and you get hit as normal since we are not destroying the projectile            
        }

        if (other.gameObject.name.Contains("WizBasic"))
        {
            if (blockChance > 22)
            {
                Instantiate(blockSound, this.transform.position, this.transform.rotation);
                Destroy(other.gameObject);
            }         
        }
        if (other.gameObject.name.Contains("palaGround"))
        {
            if (blockChance > 18)
            {
                Instantiate(blockSound, this.transform.position, this.transform.rotation);
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.name.Contains("SummonerShot") || other.gameObject.name.Contains("WizardShot"))
        {
            if (blockChance > 8)
            {
                Instantiate(blockSound, this.transform.position, this.transform.rotation);
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.name.Contains("wizardFire"))
        {
            if (blockChance > 21)
            {
                Instantiate(blockSound, this.transform.position, this.transform.rotation);
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.name.Contains("wizardPoison"))
        {
            if (blockChance > 10)
            {
                Instantiate(blockSound, this.transform.position, this.transform.rotation);
                Destroy(other.gameObject);
            }
        }

    }
}
