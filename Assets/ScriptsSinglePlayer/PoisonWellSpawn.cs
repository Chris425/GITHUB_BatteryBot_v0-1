using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonWellSpawn : MonoBehaviour {
    private float cooldown = 1.2f;
    private float cooldownTimer = 0.0f;
    public GameObject[] spawnLocations;
    public GameObject poisonShot;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        cooldownTimer -= 0.01f;

        if (cooldownTimer <= 0.01f)
        {
            spawnIcicles();
        }
    }
    private void spawnIcicles()
    {
        foreach (GameObject spawnloc in spawnLocations)
        {
            Instantiate(poisonShot, spawnloc.transform.position, spawnloc.transform.rotation);
            cooldownTimer = cooldown;
        }
    }
}
