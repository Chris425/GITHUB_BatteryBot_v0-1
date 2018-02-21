using UnityEngine;
using System.Collections;

public class BatterySpawnerSP : MonoBehaviour {
    public GameObject battery;
    public GameObject specEffect;
    public GameObject spawnLoc;

    private float time;

	// Use this for initialization
	void Start () {
	
	}

    void spawnBattery()
    {
        int posOffset = Random.Range(-4, 4);
        int rotOffset1 = Random.Range(1, 180);
        int rotOffset2 = Random.Range(1, 180);
        Vector3 spawnPos = new Vector3(this.transform.position.x + posOffset, this.transform.position.y, this.transform.position.z + posOffset);
        Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);

        Instantiate(specEffect, spawnPos, spawnRot);
        Instantiate(battery, spawnPos, spawnRot);
    }

 

    void updateTime()
    {
        time += 0.01f;
        //first 5 units of time - batteries will be plentiful
        if (time < 5.0)
        {
            if (Random.Range(1, 150) == 50)
            {
                spawnBattery();
            }
        }

        //larger range of random numbers generated... less likely to occur
        if (time > 5.0 && time < 10.0)
        {
            if (Random.Range(1, 400) == 50)
            {
                spawnBattery();
            }
        }
        
        if (time > 10.0 && time < 15.0)
        {
            if (Random.Range(1, 750) == 50)
            {
                spawnBattery();
            }
        }

        if (time > 15.0 && time < 20.0)
        {
            if (Random.Range(1, 1050) == 50)
            {
                spawnBattery();
            }
        }

        //hardmode
        if (time > 20.0 )
        {
            if (Random.Range(1, 1800) == 50)
            {
                spawnBattery();
            }
        }

    }

    // Update is called once per frame
    void Update () {
        updateTime();
	}
}
