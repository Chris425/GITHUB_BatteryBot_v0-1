using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManagerLvlBonus : MonoBehaviour {

    // This is specifically for bonus level
    //when level bonus is loaded this script runs the start code
    // reference the universal game manager script to determine where to spawn the player and under what conditions

    public GameObject savePoint1;
    public GameObject savePoint2;
    public GameObject floor;
    public Text distanceToFloor;
    private GameObject hero;
    public Slider distanceSlider;
    public Image Fill;

    void Start () {

        hero = GameObject.Find("PLAYERBASE");
        distanceSlider.value = 100;


        if (GAMEMANAGERSP.hasLevelBonusSave2)
        {
            //they would have these skulls by this point


            //move the hero to this position
            hero.transform.position = savePoint2.transform.position;
            hero.transform.rotation = savePoint2.transform.rotation;
            floor.transform.position = new Vector3(floor.transform.position.x, 115.0f, floor.transform.position.z);
            Bonus_FloorRiseSP.raiseSpeed = 0.0055f;//lower the speed slightly if you get savepoints. It's easy mode.
        }

        else if (GAMEMANAGERSP.hasLevelBonusSave1)
        {
            hero.transform.position = savePoint1.transform.position;
            floor.transform.position = new Vector3(floor.transform.position.x, 80.6f, floor.transform.position.z);
            Bonus_FloorRiseSP.raiseSpeed = 0.0055f;
        }
	}


    void Update()
    {
        StartCoroutine("FindPlayerDistance");
        
    }

    private IEnumerator FindPlayerDistance()
    {
        float distance = hero.gameObject.transform.position.y - floor.gameObject.transform.position.y;
        if (distance > 20)
        {
            distanceToFloor.text = distance.ToString("n2");
            distanceSlider.value = distance;
            distanceToFloor.color = Color.green;
            Fill.color = Color.green;

        }
        else if (distance >= 10 && distance < 20)
        {
            distanceToFloor.text = distance.ToString("n2");
            distanceSlider.value = distance;
            distanceToFloor.color = Color.yellow;
            Fill.color = Color.yellow;
        }
        else if (distance >= 0.1f && distance < 10)
        {
            distanceToFloor.text = distance.ToString("n2");
            distanceSlider.value = distance;
            distanceToFloor.color = Color.red;
            Fill.color = Color.red;
        }
        else
        {
            distanceToFloor.text = "0.00";
            distanceSlider.value = 0.00f;
            distanceToFloor.color = Color.red;
            Fill.color = Color.red;
        }

        yield return new WaitForSeconds(1.0f);
    }




    }
