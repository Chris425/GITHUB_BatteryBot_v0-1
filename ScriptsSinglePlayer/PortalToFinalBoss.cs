using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToFinalBoss : MonoBehaviour
{

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Interact"))
        {
            GAMEMANAGERSP.saveCurrentState();
            //Don't save to file because the load scene will also attempt to read from that same file before you finish...
            //GAMEMANAGERSP.saveGameDataToFILE();
            
            SceneManager.LoadScene("FinalBossBattle");
        }
    }
}
