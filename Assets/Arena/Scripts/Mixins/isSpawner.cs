using UnityEngine;
using System.Collections;

public class isSpawner : Mixin {

	public GameObject objToSpawn; //spawn copy of me pls...
    public GameObject spawnLoc; // this will prevent Mario (err Malcolm) from being spun around by the fireball
	public float spawnRate;
	private float t;

	public void Spawn()
	{
		if (objToSpawn != null)
		{
            //spawn it to spawnLoc instead!
			Instantiate(objToSpawn, spawnLoc.transform.position, this.transform.rotation);
		}
		else
			Debug.Log("isSpawner::Spawn(): Error, objToSpawn is null");
	}

	public void SpawnUpdate()
	{
		Transform parentTForm = this.gameObject.transform.parent;
		if (parentTForm != null)
		{
			isEquipSlot ies = parentTForm.gameObject.GetComponent<isEquipSlot>();
			if (ies != null)
			{
				t -= Time.deltaTime;
				if (t < 0.0f)
				{
					t = spawnRate;
					Spawn();
				}
			}
		}
	}

	// Use this for initialization
	void Start () {

		t = spawnRate;
	}
	
	// Update is called once per frame
	void Update () {

		// if this spawner is equipped on a player, in a slot
		// then spawn every 2 seconds
		//SpawnUpdate();
	}
}
