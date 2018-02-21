using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitTerrain : MonoBehaviour
{
    //when a projectile hits anything but the player, destroy it on impact

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("PLAYERBASE"))
        {
            //do nothing, hero controller will take care of it!
        }
        else
        {
            //case when enemy projectile hits something in the terrain
            //note that we don't do this script for player projectile because we want it to hit switches and stuff.
            
            Destroy(this.gameObject);
        }
    }
}