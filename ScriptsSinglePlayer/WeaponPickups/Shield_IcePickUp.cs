﻿using UnityEngine;
using System.Collections;

public class Shield_IcePickUp : MonoBehaviour
{
    public GameObject Pickup;
    public GameObject SE_Gear;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("spinPowerUp");
    }

    IEnumerator spinPowerUp()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(2.0f);
            transform.Rotate(new Vector3(2.0f, 3.5f, 5.0f) * 5.0f * Time.deltaTime);
            yield return new WaitForSeconds(3.0f);
        }

    }



    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            HeroControllerSP.hasShield_ICE = true;
            GAMEMANAGERSP.numScore += 22;

            HeroController.hasShield_ICE = true;
            GAMEMANAGERSP.numArenaScore += 22;
            Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
