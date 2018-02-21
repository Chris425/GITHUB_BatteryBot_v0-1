using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManagerLvlFinalBoss : MonoBehaviour
{

    // This is specifically for final boss battle
    public GameObject floor;
    public Text distanceToFloor;
    private GameObject hero;
    public Slider distanceSlider;
    public Image Fill;

    void Start()
    {

        hero = GameObject.Find("PLAYERBASE");
        distanceSlider.value = 100;

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
