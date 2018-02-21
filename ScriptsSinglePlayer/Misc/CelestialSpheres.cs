using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialSpheres : MonoBehaviour {

    public GameObject SE_CelestialSphereParticles;
    // Update is called once per frame
    void Update () {
        StartCoroutine("spawnParticles");
    }

    private IEnumerator spawnParticles()
    {
        yield return null;
        yield return null;
        int randomNum = Random.Range(1,21);
        yield return null;
        yield return null;
        if (randomNum > 19)
        {
            yield return null;
            yield return null;
            Instantiate(SE_CelestialSphereParticles, this.transform.position, this.transform.rotation);
            yield return null;
            yield return null;
        }
        yield return null;
        yield return null;
    }
}
