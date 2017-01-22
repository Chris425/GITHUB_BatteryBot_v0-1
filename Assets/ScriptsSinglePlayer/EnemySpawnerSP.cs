using UnityEngine;
using System.Collections;

public class EnemySpawnerSP : MonoBehaviour {
    public GameObject vampireEnemy;
    public GameObject bossEnemy;
    public GameObject casterEnemy;
    public GameObject specEffect;
    public GameObject spawnLoc;

    private float time;

	// Use this for initialization
	void Start () {
	
	}

    void spawnEnemy()
    {
        Instantiate(specEffect, spawnLoc.transform.position, spawnLoc.transform.rotation);
        Instantiate(vampireEnemy, spawnLoc.transform.position, spawnLoc.transform.rotation);
    }

    void spawnBoss()
    {
        Instantiate(specEffect, spawnLoc.transform.position, spawnLoc.transform.rotation);
        Instantiate(bossEnemy, spawnLoc.transform.position, spawnLoc.transform.rotation);
    }

    void spawnCaster()
    {
        Instantiate(specEffect, spawnLoc.transform.position, spawnLoc.transform.rotation);
        Instantiate(casterEnemy, spawnLoc.transform.position, spawnLoc.transform.rotation);
    }

    void updateTime()
    {
        time += 0.01f;
        //first 5 units of time - enemies will be rare
        if (time < 5.0)
        {
            if (Random.Range(1, 1500) == 50)
            {
                spawnEnemy();
            }
        }
        
        if (time > 5.0 && time < 10.0)
        {
            if (Random.Range(1, 1250) == 50)
            {
                spawnEnemy();
            }
        }
        
        if (time > 10.0 && time < 15.0)
        {
            int rndm = Random.Range(1, 2000);
            if (rndm == 50)
            {
                spawnCaster();
            }
            else if (rndm == 3)
            {
                spawnEnemy();
                
            }
            else if (rndm == 4)
            {
                spawnBoss();
            }
        }

        if (time > 15.0 && time < 20.0)
        {
            int rndm = Random.Range(1, 1850);
            if (rndm == 50)
            {
                spawnEnemy();
            }
            else if (rndm == 3)
            {
                spawnCaster();
            }
            else if (rndm == 4)
            {
                spawnBoss();
            }
        }

        //hardmode
        if (time > 20.0 )
        {
            int rndm = Random.Range(1, 1000);
            if (rndm == 50)
            {
                spawnEnemy();
            }
            else if (rndm <= 4)
            {
                spawnCaster();
            }
            else if (rndm == 5)
            {
                spawnBoss();
            }
        }

    }

    // Update is called once per frame
    void Update () {
        updateTime();
	}
}
