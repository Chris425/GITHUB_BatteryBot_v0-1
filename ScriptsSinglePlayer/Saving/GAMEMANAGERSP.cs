using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GAMEMANAGERSP : MonoBehaviour {
    
    public AudioClip introSong;
    public AudioSource audSrc;
    private bool isPlayingIntro;
    public static bool shouldMakeMusicBot = true;

    //local in editor
    //private static string path = "Assets/SaveFile/SaveFile001.txt";
    private static string path = Application.persistentDataPath + "/SaveFile001.txt";
    public static string arenaPath = Application.persistentDataPath + "/SaveFileArena001.txt";

    public static int numLivesRemaining = 5;

    public static bool hasFinishedLevelOne = false;
    public static bool hasLevelOneSave1 = false;
    public static bool hasLevelOneSave2 = false;

    public static bool hasFinishedLevelTwo = false;
    public static bool hasLevelTwoSave1 = false;
    public static bool hasLevelTwoSave2 = false;

    public static bool hasFinishedLevelThree = false;
    public static bool hasLevelThreeSave1 = false;
    public static bool hasLevelThreeSave2 = false;

    public static bool hasFinishedLevelFour = false;
    public static bool hasLevelFourSave1 = false;
    public static bool hasLevelFourSave2 = false;

    public static bool hasFinishedLevelBonus = false;
    public static bool hasLevelBonusSave1 = false;
    public static bool hasLevelBonusSave2 = false;

    //Keep track of what items the hero has, so that they may be retained
    //1-5 weapons
    public static bool hasAxe = false;
    public static bool hasGun = false;
    public static bool hasGreatsword = false;
    public static bool hasShield = false;
    public static bool hasBooster = false;
    //powerups for 1-5 weapons
    public static bool hasAxeLightning = false;
    public static bool hasGunMulti = false;
    public static bool hasGunFire = false;
    public static bool hasGunPoison = false;
    public static bool hasGunIce = false;
    public static bool hasGreatswordFire = false;
    public static bool hasShieldIce = false;
    public static bool hasBoosterArcane = false;
    //ammo, gears and battery levels 
    public static int numAmmo = 0;
    public static int numGears = 0;
    public static int numBattery = 100;
    //armour
    public static bool hasHelm = false;
    public static bool hasBoots = false;
    public static bool hasArmour = false;
    public static bool hasLegs = false;
    //bro-bot
    public static bool hasBroBot = false;
    //Game Manager Score
    public static int numScore = 0;
        //saved in separate method to separate file
        public static int numArenaScore; 
        public static int numArenaHighScore;
    //Quality Settings
    public static int numQualitySetting;
    //Bazooka and metal skulls
    public static bool hasBazooka;
    public static bool hasSilver;
    public static bool hasSkull_BRONZE;
    public static bool hasSkull_SILVER;
    public static bool hasSkull_GOLD;


    public string Path
    {
        get
        {
            return path;
        }

        set
        {
            path = value;
        }
    }


    public string ArenaPath
    {
        get
        {
            return arenaPath;
        }

        set
        {
            arenaPath = value;
        }
    }


    //skulls not used since then they would persist
    //handle that in individual savepoints


    void Start() {
        shouldMakeMusicBot = true;

        //this is a persistent game object!
        UnityEngine.Object.DontDestroyOnLoad(this);

        audSrc = GetComponent<AudioSource>();

        string currScene = SceneManager.GetActiveScene().name;
        Debug.Log(currScene);
    }

    // Update is called once per frame
    void Update() {

    }



    //method to set player gear and stuff during a save
    //do if/else for all attributes, ensure that it goes back to false if it is no longer active. especially true for gun multi/fire/psn etc.
    public static void saveCurrentState()
    {
        //1-5 weapons
        if (HeroControllerSP.hasAxe) { hasAxe = true; }
        if (HeroControllerSP.hasGun) { hasGun = true; }
        if (HeroControllerSP.hasGS) { hasGreatsword = true; }
        if (HeroControllerSP.hasShield) { hasShield = true; }
        if (HeroControllerSP.hasJetBooster) { hasBooster = true; }
        //powerups for 1-5 weapons
        if (HeroControllerSP.hasAxe_LIGHTNING) { hasAxeLightning = true; }
        if (HeroControllerSP.hasGun_MULTI) { hasGunMulti = true; }
        else if (HeroControllerSP.hasGun_COLD) { hasGunIce = true; }
        else if (HeroControllerSP.hasGun_FIRE) { hasGunFire = true; }
        else if (HeroControllerSP.hasGun_POISON) { hasGunPoison = true; }
        if (HeroControllerSP.hasGS_FIRE) {hasGreatswordFire  = true; }
        if (HeroControllerSP.hasShield_ICE) {hasShieldIce  = true; }
        if (HeroControllerSP.hasJetBooster_ARCANE) {hasBoosterArcane  = true; }
        //ammo, gears and battery levels 
        numAmmo = HeroControllerSP.Ammo;
        numGears = HeroControllerSP.Gears;
        numBattery = HeroControllerSP.battery;
        numLivesRemaining = HeroControllerSP.Lives;
        //numScore is in this class; don't write it here.
        //numQualitySetting is in this class; don't write it here.
        //armour
        if (HeroControllerSP.hasHelm) {hasHelm  = true; }
        if (HeroControllerSP.hasArmour) {hasArmour  = true; }
        if (HeroControllerSP.hasLegs) {hasLegs = true; }
        if (HeroControllerSP.hasBoots) {hasBoots  = true; }
        //brobot
        if (HeroControllerSP.hasNPCBot) { hasBroBot = true; }
        //level progress
        //already in class so don't do anything unless we are saving to file.
        //metal skulls and bazooka
        if (HeroControllerSP.hasBazooka) { hasBazooka = true; }
        if (HeroControllerSP.hasSilver) { hasSilver = true; }
        if (HeroControllerSP.hasSkull_BRONZE) { hasSkull_BRONZE = true; }
        if (HeroControllerSP.hasSkull_SILVER) { hasSkull_SILVER = true; }
        if (HeroControllerSP.hasSkull_GOLD) { hasSkull_GOLD = true; }

    }

    //method to wipe current state, to be used on game over.
    public static void wipeGameState()
    {
        //1-5 weapons
        hasAxe = false;
        hasGun = false;
        hasGreatsword = false;
        hasShield = false;
        hasBooster = false;
        //powerups for 1-5 weapons
        hasAxeLightning = false;
        hasGunMulti = false;
        hasGunIce = false;
        hasGunFire = false;
        hasGunPoison = false;
        hasGreatswordFire = false;
        hasShieldIce = false;
        hasBoosterArcane = false;
        //ammo, gears and battery levels 
        numAmmo = 0;
        numGears = 0;
        numBattery = 100;
        numLivesRemaining = 5;
        //No need to wipe numQualitySetting on game over.
        //do not wipe numScore on gameOver. Only on new game.
        //armour
        hasHelm = false;
        hasArmour = false;
        hasLegs = false;
        hasBoots = false;
        //brobot
        hasBroBot = false;
        //level progress - ONLY remove save points, not level progress. That's too much to lose on game over.
        hasLevelOneSave1 = false;
        hasLevelOneSave2 = false;
        hasLevelTwoSave1 = false;
        hasLevelTwoSave2 = false;
        hasLevelThreeSave1 = false;
        hasLevelThreeSave2 = false;
        hasLevelFourSave1 = false;
        hasLevelFourSave2 = false;
        hasLevelBonusSave1 = false;
        hasLevelBonusSave2 = false;
        //Never wipe metal skulls, silver or bazooka, even on gameover

    }

    //method to load current inventory state
    //called in HeroControllerSP on awake.
    public static void loadCurrentState()
    {
        //1-5 weapons
        if (hasAxe) { HeroControllerSP.hasAxe = true; }
        if (hasGun) { HeroControllerSP.hasGun = true; }
        if (hasGreatsword) { HeroControllerSP.hasGS= true; }
        if (hasShield) { HeroControllerSP.hasShield = true; }
        if (hasBooster) { HeroControllerSP.hasJetBooster = true; }
        //powerups for 1-5 weapons
        if (hasAxeLightning) { HeroControllerSP.hasAxe_LIGHTNING = true; }
        if (hasGunMulti) { HeroControllerSP.hasGun_MULTI = true; }
        else if (hasGunIce) { HeroControllerSP.hasGun_COLD = true; }
        else if (hasGunFire) { HeroControllerSP.hasGun_FIRE = true; }
        else if (hasGunPoison) { HeroControllerSP.hasGun_POISON = true; }
        if (hasGreatswordFire) { HeroControllerSP.hasGS_FIRE = true; }
        if (hasShieldIce) { HeroControllerSP.hasShield_ICE = true; }
        if (hasBoosterArcane) { HeroControllerSP.hasJetBooster_ARCANE = true; }
        //ammo, gears and battery levels 
        HeroControllerSP.Ammo = numAmmo;
        HeroControllerSP.Gears = numGears;
        HeroControllerSP.battery = numBattery;
        HeroControllerSP.Lives = numLivesRemaining;
        //numScore exists during current playsession already.
        //numQualitySetting exists during current playsession already.
        //armour
        if (hasHelm) { HeroControllerSP.hasHelm = true; }
        if (hasArmour) { HeroControllerSP.hasArmour = true; }
        if (hasLegs) { HeroControllerSP.hasLegs = true; }
        if (hasBoots) { HeroControllerSP.hasBoots = true; }
        //brobot
        if (hasBroBot) { HeroControllerSP.hasNPCBot = true; }
        //level progress
        //stored in this class already so no need to do it unless we are saving to file.
        //bazooka and metal skulls
        if (hasBazooka) { HeroControllerSP.hasBazooka = true; }
        if (hasSilver) { HeroControllerSP.hasSilver = true; }
        if (hasSkull_BRONZE) { HeroControllerSP.hasSkull_BRONZE = true; }
        if (hasSkull_SILVER) { HeroControllerSP.hasSkull_SILVER = true; }
        if (hasSkull_GOLD) { HeroControllerSP.hasSkull_GOLD = true; }
    }


    //method to save current data to a file to be accessed later...
    //Call SaveCurrentState() first to pull latest info from HeroController
    public static void saveGameDataToFILE()
    {

        //problem - simply writing hasAxe doesn't work because it shows as False, not false. So make it a string, bring it to lower case.

        string jsonData = "{\"hasAxe\": " + hasAxe.ToString().ToLower() + ","
                        + "\"hasGun\" : " + hasGun.ToString().ToLower() + ","
                        + "\"hasGreatsword\" :" + hasGreatsword.ToString().ToLower() + ","
                        + "\"hasShield\" : " + hasShield.ToString().ToLower() + ","
                        + "\"hasBooster\" : " + hasBooster.ToString().ToLower() + ","
                        + "\"hasAxeLightning\" : " + hasAxeLightning.ToString().ToLower() + ","
                        + "\"hasGunMulti\" : " + hasGunMulti.ToString().ToLower() + ","
                        + "\"hasGunIce\" : " + hasGunIce.ToString().ToLower() + ","
                        + "\"hasGunFire\" : " + hasGunFire.ToString().ToLower() + ","
                        + "\"hasGunPoison\" : " + hasGunPoison.ToString().ToLower() + ","
                        + "\"hasGreatswordFire\" : " + hasGreatswordFire.ToString().ToLower() + ","
                        + "\"hasShieldIce\" : " + hasShieldIce.ToString().ToLower() + ","
                        + "\"hasBoosterArcane\" : " + hasBoosterArcane.ToString().ToLower() + ","
                        + "\"numAmmo\" : " + numAmmo + ","
                        + "\"numGears\" : " + numGears + ","
                        + "\"numBattery\" : " + numBattery + ","
                        + "\"numLivesRemaining\" : " + numLivesRemaining + ","
                        + "\"numQualitySetting\" : " + numQualitySetting + ","
                        + "\"numScore\" : " + numScore + ","
                        + "\"hasHelm\" : " + hasHelm.ToString().ToLower() + ","
                        + "\"hasArmour\" : " + hasArmour.ToString().ToLower() + ","
                        + "\"hasLegs\" : " + hasLegs.ToString().ToLower() + ","
                        + "\"hasBoots\" : " + hasBoots.ToString().ToLower() + ","
                        + "\"hasBroBot\" : " + hasBroBot.ToString().ToLower() + ","
                        + "\"hasFinishedLevelOne\" : " + hasFinishedLevelOne.ToString().ToLower() + ","
                        + "\"hasFinishedLevelTwo\" : " + hasFinishedLevelTwo.ToString().ToLower() + ","
                        + "\"hasFinishedLevelThree\" : " + hasFinishedLevelThree.ToString().ToLower() + ","
                        + "\"hasFinishedLevelFour\" : " + hasFinishedLevelFour.ToString().ToLower() + ","
                        + "\"hasFinishedLevelBonus\" : " + hasFinishedLevelBonus.ToString().ToLower() + ","
                        + "\"hasBazooka\" : " + hasBazooka.ToString().ToLower() + ","
                        + "\"hasSilver\" : " + hasSilver.ToString().ToLower() + ","
                        + "\"hasSkull_BRONZE\" : " + hasSkull_BRONZE.ToString().ToLower() + ","
                        + "\"hasSkull_SILVER\" : " + hasSkull_SILVER.ToString().ToLower() + ","
                        + "\"hasSkull_GOLD\" : " + hasSkull_GOLD.ToString().ToLower() + ""
                        + "}";

        StreamWriter writer = new StreamWriter(path, false);

        try
        {       
            writer.WriteLine(jsonData);
            writer.Close();
            Debug.Log("[INFO] Saved to file successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.ToString());
            Debug.Log("[ERROR] Failed to save to file.");
            writer.Close();
        }
        


    }


    //simply store an int for current arena max score!
    public static void saveArenaScoreToFILE()
    {
        //see if score in file is larger or smaller than you current score.
        StreamReader reader = new StreamReader(arenaPath);
        try
        {
            int currHighScore = Int32.Parse(reader.ReadLine());
            reader.Close();
            if (currHighScore < numArenaScore)
            {
                //your score is higher, overwrite!
                StreamWriter writer = new StreamWriter(arenaPath, false);
                try
                {
                    writer.WriteLine(numArenaScore);
                    writer.Close();
                    Debug.Log("[INFO] Saved arena score to separate file successfully.");
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.ToString());
                    Debug.Log("[ERROR] Failed to save arena score to separate file!");
                    writer.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            Debug.Log("Failed to find current arena high score when saving...");
        }

        



    }


    //to be loaded from a future screen
    public static void loadGameDataFromFILE()
    {
        StreamReader reader = new StreamReader(path);
        try
        {            
            string jsonStr = reader.ReadLine();
            
            var jsonObj = JsonUtility.FromJson<JSONSaveState>(jsonStr);

            //1-5 weapons
            if (jsonObj.hasAxe) { HeroControllerSP.hasAxe = true; }
            if (jsonObj.hasGun) { HeroControllerSP.hasGun = true; }
            if (jsonObj.hasGreatsword) { HeroControllerSP.hasGS = true; }
            if (jsonObj.hasShield) { HeroControllerSP.hasShield = true; }
            if (jsonObj.hasBooster) { HeroControllerSP.hasJetBooster = true; }
            //powerups for 1-5 weapons
            if (jsonObj.hasAxeLightning) { HeroControllerSP.hasAxe_LIGHTNING = true; }
            if (jsonObj.hasGunMulti) { HeroControllerSP.hasGun_MULTI = true; }
            else if (jsonObj.hasGunIce) { HeroControllerSP.hasGun_COLD = true; }
            else if (jsonObj.hasGunFire) { HeroControllerSP.hasGun_FIRE = true; }
            else if (jsonObj.hasGunPoison) { HeroControllerSP.hasGun_POISON = true; }
            if (jsonObj.hasGreatswordFire) { HeroControllerSP.hasGS_FIRE = true; }
            if (jsonObj.hasShieldIce) { HeroControllerSP.hasShield_ICE = true; }
            if (jsonObj.hasBoosterArcane) { HeroControllerSP.hasJetBooster_ARCANE = true; }
            //ammo, gears, battery levels , lives, score
            HeroControllerSP.Ammo = jsonObj.numAmmo;
            HeroControllerSP.Gears = jsonObj.numGears;
            HeroControllerSP.battery = jsonObj.numBattery;
            HeroControllerSP.Lives = jsonObj.numLivesRemaining;
            numScore = jsonObj.numScore;
            numQualitySetting = jsonObj.numQualitySetting;
            //armour
            if (jsonObj.hasHelm) { HeroControllerSP.hasHelm = true; }
            if (jsonObj.hasArmour) { HeroControllerSP.hasArmour = true; }
            if (jsonObj.hasLegs) { HeroControllerSP.hasLegs = true; }
            if (jsonObj.hasBoots) { HeroControllerSP.hasBoots = true; }
            //brobot
            if (jsonObj.hasBroBot) { HeroControllerSP.hasNPCBot = true; }
            //level progress
            if (jsonObj.hasFinishedLevelOne) { hasFinishedLevelOne = true; }
            if (jsonObj.hasFinishedLevelTwo) { hasFinishedLevelTwo = true; }
            if (jsonObj.hasFinishedLevelThree) { hasFinishedLevelThree = true; }
            if (jsonObj.hasFinishedLevelFour) { hasFinishedLevelFour = true; }
            if (jsonObj.hasFinishedLevelBonus) { hasFinishedLevelBonus = true; }
            //bazooka and metal skulls
            if (jsonObj.hasBazooka) { hasBazooka = true; }
            if (jsonObj.hasSilver) { hasSilver = true; }
            if (jsonObj.hasSkull_BRONZE) { hasSkull_BRONZE= true; }
            if (jsonObj.hasSkull_SILVER) { hasSkull_SILVER= true; }
            if (jsonObj.hasSkull_GOLD) { hasSkull_GOLD= true; }


            reader.Close();
            Debug.Log("[INFO] Loaded data from file successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.ToString());
            Debug.Log("[ERROR] Failed to load save from file.");
            reader.Close();
            throw;
        }

    }


}
