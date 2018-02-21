using UnityEngine;
using System.Collections;

public class BazookaPickUp : MonoBehaviour
{
    

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
            transform.Rotate(new Vector3(0.0f, 0.0f, 3.3f) * 5.0f * Time.deltaTime);
            yield return new WaitForSeconds(3.0f);
        }

    }

    public void OnCollisionEnter(Collision other)
    {
        //case when the player interacts with it
        if (other.gameObject.name.Contains("InteractShot"))
        {
            GAMEMANAGERSP.numScore += 100;
            GAMEMANAGERSP.hasBazooka = true;
            HeroControllerSP.hasBazooka = true;
            HeroControllerSP.isSlot2 = true;
            HeroControllerSP.isSlot1 = false; HeroControllerSP.isSlot3 = false; 
            Destroy(this.gameObject);
        }
    }
}
