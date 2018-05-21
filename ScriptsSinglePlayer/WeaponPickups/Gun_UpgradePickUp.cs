using UnityEngine;
using System.Collections;

public class Gun_UpgradePickUp : MonoBehaviour
{
    public GameObject Pickup;
    public GameObject SE_Gear;

    public bool isMulti;
    public bool isPoison;
    public bool isCold;
    public bool isFire;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("spinPowerUp");
    }

    IEnumerator spinPowerUp()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(2.0f);
            transform.Rotate(new Vector3(2.0f, 3.5f, 5.0f) * 5.0f * Time.deltaTime);
            yield return new WaitForSeconds(3.0f);
        }

    }



    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            GAMEMANAGERSP.numScore += 25;
            GAMEMANAGERSP.numArenaScore += 25;
            //You can only have one at a time since I only have one UI slot for gun upgrades... ;)
            if (isMulti)
            {
                HeroControllerSP.hasGun_MULTI = true;
                HeroControllerSP.hasGun_POISON = false;
                HeroControllerSP.hasGun_COLD = false;
                HeroControllerSP.hasGun_FIRE = false;

                HeroController.hasGun_MULTI = true;
                HeroController.hasGun_POISON = false;
                HeroController.hasGun_COLD = false;
                HeroController.hasGun_FIRE = false;

                Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
            else if (isPoison)
            {
                HeroControllerSP.hasGun_MULTI = false;
                HeroControllerSP.hasGun_POISON = true;
                HeroControllerSP.hasGun_COLD = false;
                HeroControllerSP.hasGun_FIRE = false;

                HeroController.hasGun_MULTI = false;
                HeroController.hasGun_POISON = true;
                HeroController.hasGun_COLD = false;
                HeroController.hasGun_FIRE = false;
                Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
            else if (isCold)
            {
                HeroControllerSP.hasGun_MULTI = false;
                HeroControllerSP.hasGun_POISON = false;
                HeroControllerSP.hasGun_COLD = true;
                HeroControllerSP.hasGun_FIRE = false;

                HeroController.hasGun_MULTI = false;
                HeroController.hasGun_POISON = false;
                HeroController.hasGun_COLD = true;
                HeroController.hasGun_FIRE = false;
                Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
            else if (isFire)
            {
                HeroControllerSP.hasGun_MULTI = false;
                HeroControllerSP.hasGun_POISON = false;
                HeroControllerSP.hasGun_COLD = false;
                HeroControllerSP.hasGun_FIRE = true;

                HeroController.hasGun_MULTI = false;
                HeroController.hasGun_POISON = false;
                HeroController.hasGun_COLD = false;
                HeroController.hasGun_FIRE = true;
                Instantiate(SE_Gear, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }

        }
    }
}
