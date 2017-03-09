using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareScript : MonoBehaviour {

    public GameObject SE_frog;
    private float cooldownTimer;

    void Update()
    {
        if (cooldownTimer > 0.0f)
        {
            cooldownTimer -= 0.03f;
        }
        
    }

    public void OnEnable()
    {
        cooldownTimer = 0.0f;
    }


    public void OnCollisionEnter(Collision other)
    {
        //only have this functionality if the player has beaten all 3 levels
        if (cooldownTimer <= 0.01f && HeroControllerSP.hasSkull_BLUE)
        {
            Instantiate(SE_frog, this.transform.position, this.transform.rotation);
            cooldownTimer = 0.5f;
        }
        
    }
}
