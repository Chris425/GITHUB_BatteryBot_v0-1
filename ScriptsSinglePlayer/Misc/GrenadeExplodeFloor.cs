using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplodeFloor : MonoBehaviour {

    public GameObject BlueMultiOutput;



    public void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.name.Contains("PLAYERBASE"))
        {
            Instantiate(BlueMultiOutput, this.gameObject.transform.position, this.gameObject.transform.rotation);
            Destroy(this.gameObject);
            
        }
    }
}
