using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttackSpawn : MonoBehaviour
{
    private float cooldown = 0.5f;
    private float cooldownTimer = 0.0f;
    public GameObject[] spawnLocations;
    public GameObject groundShot;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        cooldownTimer -= 0.01f;

        if (cooldownTimer <= 0.01f)
        {
            spawnGroundShot();
        }
    }
    private void spawnGroundShot()
    {
        foreach (GameObject spawnloc in spawnLocations)
        {
            Instantiate(groundShot, spawnloc.transform.position, spawnloc.transform.rotation);
            cooldownTimer = cooldown;
        }
    }
}
