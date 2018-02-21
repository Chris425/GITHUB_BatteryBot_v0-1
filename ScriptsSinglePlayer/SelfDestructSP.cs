using UnityEngine;
using System.Collections;

public class SelfDestructSP : MonoBehaviour
{

    public float lifeSpan = 3.0f;



    public void DespawnUpdate()
    {

        lifeSpan -= Time.deltaTime;

        //when the time runs out, delete your special effect instance
        if (lifeSpan <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        DespawnUpdate();
    }
}
