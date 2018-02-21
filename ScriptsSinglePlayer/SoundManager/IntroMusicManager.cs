using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMusicManager : MonoBehaviour {

    void Start()
    {
        UnityEngine.Object.DontDestroyOnLoad(this);
    }
	// Update is called once per frame
	void Update () {
        int currScene = SceneManager.GetActiveScene().buildIndex;
        if (currScene > 1) //NOT INTRO OR INTRO_LOAD
        {
            Destroy(this.gameObject);
        }

    }
}
