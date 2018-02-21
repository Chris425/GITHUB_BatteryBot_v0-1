using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_FloorRiseSP : MonoBehaviour
{
    public static float raiseSpeed = 0.0075f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!HeroControllerSP.isPaused)
        {
            StartCoroutine("raiseFloor");
        }
        
    }


    private IEnumerator raiseFloor()
    {
        this.gameObject.transform.Translate(0, raiseSpeed, 0 * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Equals("BatteryBotTouch"))
        {
            HeroControllerSP.battery -= 9999;
        }
        else if (other.gameObject.name.Equals("temp1"))
        {
            //do nothing - don't delete this one.
        }
        else
        {
            Destroy(other.gameObject);
        }
        
    }
}
