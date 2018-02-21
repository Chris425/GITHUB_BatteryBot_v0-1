using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineGrow : MonoBehaviour {

    public GameObject cube;
    private bool isGrowing;
    private bool isShrinking;
    private float growthFactor;

    // Use this for initialization
    void Start () {
        isShrinking = false;
        isGrowing = true;
        growthFactor = 0.2f;
    }
	
	// Update is called once per frame
	void Update () {
        if (cube.transform.localScale.x > 5.0f)
        {
            isGrowing = false;
            isShrinking = true;
        }
        else if (cube.transform.localScale.x < 0.5f)
        {
            isGrowing = true;
            isShrinking = false;
        }
        
        if (isGrowing)
        {
            StartCoroutine("Grow");
        }
        else if(isShrinking)
        {
            StartCoroutine("Shrink");
        }
        
    }

    IEnumerator Shrink()
    {
        for (int i = 0; i < 10; i++)
        {
            transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growthFactor;
            yield return new WaitForSecondsRealtime(10.5f);
        }

    }

    IEnumerator Grow()
    {
        for (int i = 0; i < 10; i++)
        {
            transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 0.2f;
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}
