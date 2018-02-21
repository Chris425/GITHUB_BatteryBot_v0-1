using UnityEngine;
using System.Collections;

public class ExplosionEffectSP : MonoBehaviour {
    public Vector3 growthRate;
    public GameObject explosion;

    // Use this for initialization
    void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update ()
    {
        explosion.transform.localScale += growthRate;
    }

   
}
