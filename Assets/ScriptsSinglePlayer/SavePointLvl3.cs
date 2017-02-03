using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointLvl3 : MonoBehaviour
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

            //assumption - only 2 save points per level
            if (saveNum == 1)
            {
                if (!GAMEMANAGERSP.hasLevelThreeSave2)
                {
                    GAMEMANAGERSP.hasLevelThreeSave1 = true;
                    Instantiate(SE_interact, this.transform.position, this.transform.rotation);
                }

            }
            else if (saveNum == 2)
            {
                GAMEMANAGERSP.hasLevelThreeSave1 = true;
                GAMEMANAGERSP.hasLevelThreeSave2 = true;
                Instantiate(SE_interact, this.transform.position, this.transform.rotation);
            }
        }
    }

}
