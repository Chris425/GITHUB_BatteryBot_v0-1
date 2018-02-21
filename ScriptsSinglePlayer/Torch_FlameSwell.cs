using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Torch_FlameSwell : MonoBehaviour
{

    private bool isExpanding;
    private int counter;
    private int endPoint;
    private Vector3 growthRate;
    public List<GameObject> SpikeList;

    // Use this for initialization
    void Start()
    {
        isExpanding = true;
        counter = 0;
        endPoint = (Random.Range(20, 45));
        growthRate = new Vector3(0, 0.01f, 0);
        

    }

    // Update is called once per frame
    void Update()
    {

        if (isExpanding)
        {
            swellFlame();
        }
        else
        {
            shrinkFlame();
        }
    }



    void swellFlame()
    {
        foreach (GameObject spike in SpikeList)
        {
            spike.transform.localScale += growthRate;
        }
        counter++;
        if (counter > endPoint)
        {
            isExpanding = false;
            endPoint = (Random.Range(3, 11));
        }
    }

    void shrinkFlame()
    {
        foreach (GameObject spike in SpikeList)
        {
            spike.transform.localScale -= growthRate;
        }
        counter--;
        if (counter == 0)
        {
            isExpanding = true;
            endPoint = (Random.Range(3, 11));
        }
    }

}
