using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAMEMANAGERSP : MonoBehaviour {

    public static bool hasFinishedLevelOne = false;
    public static bool hasLevelOneSave1 = false;
    public static bool hasLevelOneSave2 = false;

    public static bool hasFinishedLevelTwo = false;
    public static bool hasLevelTwoSave1 = false;
    public static bool hasLevelTwoSave2 = false;

    public static bool hasFinishedLevelThree = false;
    public static bool hasLevelThreeSave1 = false;
    public static bool hasLevelThreeSave2 = false;

    public static bool hasFinishedLevelBonus = false;
    public static bool hasLevelBonusSave1 = false;
    public static bool hasLevelBonusSave2 = false;
    
    void Start () {
        //this is a persistent game object!

    Object.DontDestroyOnLoad(this); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
