using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAMEMANAGERSP : MonoBehaviour {

    public static bool hasFinishedLevelOne;
    public static bool hasLevelOneSave1;
    public static bool hasLevelOneSave2;

    public static bool hasFinishedLevelTwo;
    public static bool hasLevelTwoSave1;
    public static bool hasLevelTwoSave2;

    public static bool hasFinishedLevelThree;
    public static bool hasLevelThreeSave1;
    public static bool hasLevelThreeSave2;

    public static bool hasFinishedLevelBonus;
    public static bool hasLevelBonusSave1;
    public static bool hasLevelBonusSave2;
    
    void Start () {
        //this is a persistent game object!
        Object.DontDestroyOnLoad(this); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
