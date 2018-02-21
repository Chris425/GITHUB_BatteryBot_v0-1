using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargingSystemSP : MonoBehaviour {

    public GameObject Portal;
    public GameObject SE_ChargeUp;
    public GameObject SE_GearGet;
    public ParticleSystem ps;
    public Light myLight;
    public Slider chargeSlider;
    public GameObject chargeSliderObj;

    private bool hasPlayedFinalEffect;
    private int charge;

	// Use this for initialization
	void Start () {
        hasPlayedFinalEffect = false;
        chargeSliderObj.gameObject.SetActive(false);
        Portal.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Equals("BatteryBotTouch") && charge < 600)
        {
            checkIfStarted();
            incrementCharge(4);

        }
        else if (other.gameObject.name.Contains("Shot") && charge < 600)
        {
            checkIfStarted();
            incrementCharge(1);            
        }
        else if (charge >= 600)
        {
            if (!hasPlayedFinalEffect)
            {
                CreatePortal();              
            }
            
        }
    }


    public void checkIfStarted()
    {
        if (!chargeSliderObj.gameObject.activeSelf)
        {
            chargeSliderObj.gameObject.SetActive(true);
            Instantiate(SE_ChargeUp, this.transform.position, this.transform.rotation);
        }

    }

    public void incrementCharge(int power)
    {         

        charge += power;
        chargeSlider.value = charge;
        int randomNum = UnityEngine.Random.Range(1, 20);
        if (randomNum < 3)
        {
            Instantiate(SE_ChargeUp, this.transform.position, this.transform.rotation);            
        }
        else if (randomNum > 10)
        {
            increaseEffects(); //putting this here so that it only happens sometimes (3/10 chance) - save processing power...
        }

       
    }

    public void increaseEffects()
    {
        var em = ps.emission;
        em.rateOverTime = (int)(charge);
        myLight.intensity = charge * 0.035f;

        if (myLight.intensity < 2.5f)
        {
            myLight.color = Color.red;
        }
        else if (myLight.intensity > 2.5f && myLight.intensity < 5.0f)
        {
            myLight.color = Color.yellow;
        }
        else if (myLight.intensity > 5.0f)
        {
            myLight.color = Color.green;
        }
        
    }

    private void CreatePortal()
    {
        Instantiate(SE_GearGet, this.transform.position, this.transform.rotation);
        Portal.SetActive(true);
        hasPlayedFinalEffect = true;
        var em = ps.emission;
        em.rateOverTime = 10;
        myLight.color = Color.magenta;
    }


}
