using UnityEngine;
using System.Collections;

//to be placed on all ice blocks that you wish to be destroyable

public class IceBlockDestructionSP : MonoBehaviour {

    public GameObject SE_IceBreak;
    public int numHitsToBreak = 1;
    private int counter = 0;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    //CDC update this as more attacks exist
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("PlayerShot") || other.gameObject.name.Contains("GS_Shot") 
            || other.gameObject.name.Contains("Axe_Shot") || other.gameObject.name.Contains("Shield_Shot")
            || other.gameObject.name.Contains("FIRE") || other.gameObject.name.Contains("LIGHTNING"))  
        {
            counter += 1;
            if (counter >= numHitsToBreak)
            {
                //play an effect
                Instantiate(SE_IceBreak, this.transform.position, this.transform.rotation);
                //destroy this piece of ice
                Destroy(this.gameObject);
            }
            else
            {
                //consume the bullet
                Destroy(other.gameObject);
            }
            
            
        }
    }

    public void OnParticleCollision(GameObject particle)
    {
        if (particle.gameObject.name.Contains("FlameThrowerParticle"))
        {
            int FT_hitChance = Random.Range(0, 10);
            //fire attack heals fire wizard
            if (FT_hitChance > 3)
            {
                counter += 1;
                if (counter >= numHitsToBreak)
                {
                    //play an effect
                    Instantiate(SE_IceBreak, this.transform.position, this.transform.rotation);
                    
                    //destroy this piece of ice
                    Destroy(this.gameObject);
                }

            }
        }


       

    }

}
