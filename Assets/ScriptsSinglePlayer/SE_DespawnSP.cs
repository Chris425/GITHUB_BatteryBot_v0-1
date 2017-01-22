using UnityEngine;
using System.Collections;

public class SE_DespawnSP : MonoBehaviour
{

    public float lifeSpan = 2.0f;
    private float t;    // timer

    public void OnEnable()
    {
        t = lifeSpan;

    }

    // Use this for initialization
    void Start()
    {

    }

    public void SEDespawnUpdate()
    {

        t -= Time.deltaTime;

        if (t <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        SEDespawnUpdate();
    }
}
