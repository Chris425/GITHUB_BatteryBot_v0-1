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
            //cdc trying this with thread.sleep to ensure it saves and closes writer on time.
            GAMEMANAGERSP.saveGameDataToFILE();
            System.Threading.Thread.Sleep(450);
            SceneManager.LoadScene("FinalBossBattle");
        }
    }
}
