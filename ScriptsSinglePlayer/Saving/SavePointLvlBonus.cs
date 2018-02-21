using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointLvlBonus : MonoBehaviour
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
                if (!GAMEMANAGERSP.hasLevelBonusSave2)
                {
                    GAMEMANAGERSP.hasLevelBonusSave1 = true;
                    GAMEMANAGERSP.saveCurrentState();
                    GAMEMANAGERSP.saveGameDataToFILE();
                    Instantiate(SE_interact, this.transform.position, this.transform.rotation);
                    Bonus_FloorRiseSP.raiseSpeed = 0.0055f; 
                    
                }

            }
            else if (saveNum == 2)
            {
                GAMEMANAGERSP.hasLevelBonusSave1 = true;
                GAMEMANAGERSP.hasLevelBonusSave2 = true;
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                Instantiate(SE_interact, this.transform.position, this.transform.rotation);
                Bonus_FloorRiseSP.raiseSpeed = 0.0055f;
            }
        }
    }

}
