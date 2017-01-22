using UnityEngine;
using System.Collections;

public class SelfDestructSP : MonoBehaviour
{

    public float lifeSpan = 3.0f;
    private float t;    // timer

    public void OnEnable()
    {
        t = lifeSpan;

    }


    public void DespawnUpdate()
    {

        t -= Time.deltaTime;

        //when the time runs out, delete your special effect instance
        if (t <= 0.0f)
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
