using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointLvl2 : MonoBehaviour
{

    public int saveNum;
    public GameObject SE_interact;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Shot"))
        {

            //assumption - one save for level 2 to avoid issues due to non-linear exploration
            if (saveNum == 1)
            {
                GAMEMANAGERSP.hasLevelTwoSave1 = true;
                Instantiate(SE_interact, this.transform.position, this.transform.rotation);
            }
        }
    }

}
