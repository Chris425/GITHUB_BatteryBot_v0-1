using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointLvl4 : MonoBehaviour
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
                if (!GAMEMANAGERSP.hasLevelFourSave2)
                {
                    GAMEMANAGERSP.hasLevelFourSave1 = true;
                    GAMEMANAGERSP.saveCurrentState();
                    GAMEMANAGERSP.saveGameDataToFILE();
                    Instantiate(SE_interact, this.transform.position, this.transform.rotation);
                }

            }
            else if (saveNum == 2)
            {
                HeroControllerSP.hasSkull_RED = true;
                HeroControllerSP.hasSkull_PURPLE = true;

                GAMEMANAGERSP.hasLevelFourSave1 = true;
                GAMEMANAGERSP.hasLevelFourSave2 = true;
                GAMEMANAGERSP.saveCurrentState();
                GAMEMANAGERSP.saveGameDataToFILE();
                Instantiate(SE_interact, this.transform.position, this.transform.rotation);
            }
        }
    }

}
