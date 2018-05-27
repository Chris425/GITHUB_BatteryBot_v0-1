using UnityEngine;
using System.Collections;

public class DoorScriptArena : MonoBehaviour
{

    public AudioClip switchHit;
    public AudioClip errorSound;
    public AudioClip successSound;
    private AudioSource source;
    public string doorColour;

    public GameObject door;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void Awake()
    {
        source = GetComponent<AudioSource>();

    }


    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("PlayerShot") || other.gameObject.name.Contains("InteractShot"))
        {

            if (doorColour.ToUpper().Contains("RED"))
            {
                if (HeroController.hasSkull_RED || HeroController.hasSkull_BRONZE || HeroController.hasSkull_SILVER || HeroController.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }
            else if (doorColour.ToUpper().Contains("PURPLE"))
            {
                if (HeroController.hasSkull_PURPLE || HeroController.hasSkull_BRONZE || HeroController.hasSkull_SILVER || HeroController.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }
            else if (doorColour.ToUpper().Contains("BLUE"))
            {
                if (HeroController.hasSkull_BLUE || HeroController.hasSkull_BRONZE || HeroController.hasSkull_SILVER || HeroController.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }
            else if (doorColour.ToUpper().Contains("BRONZE"))
            {
                if (HeroController.hasSkull_BRONZE || HeroController.hasSkull_SILVER || HeroController.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }
            else if (doorColour.ToUpper().Contains("SILVER"))
            {
                if (HeroController.hasSkull_SILVER || HeroController.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }
            else if (doorColour.ToUpper().Contains("GOLD"))
            {
                if (HeroController.hasSkull_GOLD)
                {
                    source.PlayOneShot(successSound, 1.0f);
                    door.gameObject.SetActive(false);
                    var doorCollider = this.gameObject.GetComponent<Collider>();
                    doorCollider.enabled = false;
                }
                else
                {
                    source.PlayOneShot(errorSound, 1.0f);
                }
            }




        }

    }

}
