using UnityEngine;
using System.Collections;
using System;

public class FrozenOrbSP : MonoBehaviour {

    public GameObject[] spawnLocations;
    public GameObject iceShot;
    public GameObject spinner;

    private float cooldown = 0.4f;
    private float cooldownTimer = 0.0f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        moveActualOrb();

        cooldownTimer -= 0.01f;

        if (cooldownTimer <= 0.01f)
        {
            spawnIcicles();
        }
    }

    private void moveActualOrb()
    {
        //rotate
        spinner.transform.Rotate(new Vector3(0, 130, 0) * 2 * Time.deltaTime);
        //move in one direction
        this.transform.Translate(new Vector3(0, 0, 5f) * Time.deltaTime);
    }

    private void spawnIcicles()
    {
        foreach (GameObject spawnloc in spawnLocations)
        {
            Instantiate(iceShot, spawnloc.transform.position, spawnloc.transform.rotation);
            cooldownTimer = cooldown;
        }
    }
}
