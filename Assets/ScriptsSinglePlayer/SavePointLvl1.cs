using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointLvl1 : MonoBehaviour
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
                if (!GAMEMANAGERSP.hasLevelOneSave2)
                {
                    GAMEMANAGERSP.hasLevelOneSave1 = true;
                    Instantiate(SE_interact, this.transform.position, this.transform.rotation);
                }
                
                

            }
            else if (saveNum == 2)
            {
                GAMEMANAGERSP.hasLevelOneSave1 = true;
                GAMEMANAGERSP.hasLevelOneSave2 = true;
                Instantiate(SE_interact, this.transform.position, this.transform.rotation);
            }
        }
    }

}
