using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JSONSaveState {

    public  int numLivesRemaining;

    public  bool hasFinishedLevelOne;
    public  bool hasLevelOneSave1;
    public  bool hasLevelOneSave2;

    public  bool hasFinishedLevelTwo;
    public  bool hasLevelTwoSave1;
    public  bool hasLevelTwoSave2;

    public  bool hasFinishedLevelThree;
    public  bool hasLevelThreeSave1;
    public  bool hasLevelThreeSave2;

    public  bool hasFinishedLevelFour;
    public  bool hasLevelFourSave1;
    public  bool hasLevelFourSave2;

    public  bool hasFinishedLevelBonus;
    public  bool hasLevelBonusSave1;
    public  bool hasLevelBonusSave2;

    //Keep track of what items the hero has, so that they may be retained
    //1-5 weapons
    public  bool hasAxe;
    public  bool hasGun;
    public bool hasBazooka;
    public bool hasSilver;
    public  bool hasGreatsword;
    public  bool hasShield;
    public  bool hasBooster;
    //powerups for 1-5 weapons
    public  bool hasAxeLightning;
    public  bool hasGunMulti;
    public  bool hasGunFire;
    public  bool hasGunPoison;
    public  bool hasGunIce;
    public  bool hasGreatswordFire;
    public  bool hasShieldIce;
    public  bool hasBoosterArcane;
    //ammo, gears and battery levels 
    public  int numAmmo;
    public  int numGears;
    public  int numBattery;
    public  int numScore;
    public int numQualitySetting;
    //armour
    public  bool hasHelm;
    public  bool hasBoots;
    public  bool hasArmour;
    public  bool hasLegs;
    //bro-bot
    public  bool hasBroBot;
    //metal skulls
    public bool hasSkull_BRONZE;
    public bool hasSkull_SILVER;
    public bool hasSkull_GOLD;






}
