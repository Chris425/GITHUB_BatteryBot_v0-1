using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizFireProjExplosion : MonoBehaviour
{
    public GameObject FireBlastMulti;
    //when a projectile hits anything but the player, destroy it on impact

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("PLAYERBASE"))
        {
            //do nothing, hero controller will take care of it!
        }
        else if (other.gameObject.name.ToUpper().Contains("WIZARD") || other.gameObject.name.Contains("Minotauro") || other.gameObject.name.Contains("Wizard"))
        {
            //do nothing, I don't want it to explode immediately when fired from the caster if it happens to collide.
        }
        else
        {
            //case when enemy projectile hits something in the terrain
            //note that we don't do this script for player projectile because we want it to hit switches and stuff.
            
            Instantiate(FireBlastMulti, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}