using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GAMEMANAGER_ARENA : MonoBehaviour {


    public GameObject battery;
    public GameObject vampire;
    public GameObject caster;
    public GameObject vampirePoison;
    public GameObject casterPoison;
    public GameObject skeleZombie;
    public GameObject paladin;
    public GameObject minotaurArena;
    public GameObject summonerArena;
    public GameObject demon;
    public GameObject specEffect;


    public GameObject enemyEffect;
    public GameObject SE_Warhorn;
    public GameObject SE_Warhorn2;

    public List<GameObject> batterySpawnLocList;
    public GameObject enemySpawnLoc;
    private float time;
    private float firstEra = 6.0f;
    private float secondEra = 75.0f;
    private float thirdEra = 245.0f;
    private float finalEra = 525.0f;
    //private float time;
    //private float firstEra = 1.0f;
    //private float secondEra = 2.0f;
    //private float thirdEra = 3.0f;
    //private float finalEra = 4.0f;
    private float minorBossCooldown;
    private float majorBossCooldown;

    private int bat_highSpawnChance = 18; //ex. random.range(1, 10), more likely to occur than (1,1000)
    private int bat_mediumSpawnChance = 50;
    private int bat_lowSpawnChance = 125;
    private int bat_abysmalSpawnChance = 250;
    private int bat_endgameSpawnChance = 425;

   //probability is inverse compared to battery
    private int enemy_abysmalSpawnChance = 825;
    private int enemy_lowSpawnChance = 710;
    private int enemy_mediumSpawnChance = 560;
    private int enemy_highSpawnChance = 488;
    private int enemy_endgameSpawnChance = 295;

    public List<GameObject> minorBossSliders;

    private bool isBossSlot1Open = true;
    private bool isBossSlot2Open = true;
    private bool isBossSlot3Open = true;
    private bool isBossSlot4Open = true;

    private bool isMajorBossSlotOpen = true;

    public Slider minorBossSlider1;
    public Slider minorBossSlider2;
    public Slider minorBossSlider3;
    public Slider minorBossSlider4;

    public Slider majorBossSlider;

    MinotauroArena minosScript1;
    MinotauroArena minosScript2;
    MinotauroArena minosScript3;
    MinotauroArena minosScript4;

    DemonArena demonScript;
    

    Color orange;
    Color deepOrange;

    public Text era;
    public Text timeText;


    private void Start()
    {
        
        orange = new Color(0.9f,0.5f,0.1f,1.0f);
        deepOrange = new Color(0.7f, 0.2f, 0.1f, 1.0f);


        minorBossSlider1.gameObject.SetActive(false);
        minorBossSlider2.gameObject.SetActive(false);
        minorBossSlider3.gameObject.SetActive(false);
        minorBossSlider4.gameObject.SetActive(false);

        majorBossSlider.gameObject.SetActive(false);


        batterySpawnLocList = new List<GameObject>();
        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.name.Contains("BatterySpawner"))
            {
                batterySpawnLocList.Add(obj);
            }

        }

 
    }

    private void spawnBattery()
    {
        int posOffset = Random.Range(-4, 4);
        int rotOffset1 = Random.Range(1, 180);
        int rotOffset2 = Random.Range(1, 180);

        int spawnerNum = Random.Range(0, batterySpawnLocList.Count);
        Transform chosenSpawner = batterySpawnLocList[spawnerNum].gameObject.transform;

        Vector3 spawnPos = new Vector3(chosenSpawner.position.x + posOffset, chosenSpawner.position.y, chosenSpawner.position.z + posOffset);
        Quaternion spawnRot = new Quaternion(chosenSpawner.rotation.x + rotOffset1, chosenSpawner.rotation.y + rotOffset2, chosenSpawner.rotation.z + rotOffset1, chosenSpawner.rotation.w + rotOffset2);

        Instantiate(specEffect, spawnPos, spawnRot);
        Instantiate(battery, spawnPos, spawnRot);
    }

    //default is normal vampire - they are more common!
    private void spawnEnemy(int era)
    {
        int determineWhichEnemy = Random.Range(0, 8);
        if (era == 1)
        {            
            switch (determineWhichEnemy)
            {
                case 0:
                    Instantiate(vampire, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 1:
                    Instantiate(caster, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                default:
                    Instantiate(vampire, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
            }
            Instantiate(enemyEffect, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);

        }
        else if (era == 2)
        {
            
            switch (determineWhichEnemy)
            {
                case 0:
                    Instantiate(vampire, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 1:
                    Instantiate(vampirePoison, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 2:
                    Instantiate(caster, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 3:
                    Instantiate(casterPoison, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                default:
                    Instantiate(vampire, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
            }
            Instantiate(enemyEffect, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
        }
        else if (era == 3)
        {
            
            switch (determineWhichEnemy)
            {
                case 0:
                    Instantiate(vampire, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 1:
                    Instantiate(vampirePoison, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 2:
                    Instantiate(caster, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 3:
                    Instantiate(casterPoison, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 4:
                    Instantiate(skeleZombie, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                default:
                    Instantiate(vampire, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
            }
            Instantiate(enemyEffect, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
        }
        else if (era == 4) //bosses can spawn starting here. handled in spawnMinorBoss() method.
        {

            if (minorBossCooldown < 0.0f)
            {
                spawnMinorBoss();
                minorBossCooldown = 80.0f;
            }

            switch (determineWhichEnemy)
            {
                
                case 0:
                    Instantiate(vampire, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 1:
                    Instantiate(vampirePoison, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 2:
                    Instantiate(caster, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 3:
                    Instantiate(casterPoison, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 4:
                    Instantiate(skeleZombie, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 5:
                    Instantiate(paladin, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 6:
                    Instantiate(summonerArena, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                default:
                    Instantiate(vampire, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
            }

            Instantiate(enemyEffect, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
        }
        else
        {
            //final era / endgame
            if (minorBossCooldown < 0.0f)
            {
                spawnMinorBoss();
                minorBossCooldown = 60.0f;
            }
            if (majorBossCooldown < 0.0f)
            {
                spawnMajorBoss();
                majorBossCooldown = 120.0f;
            }

            switch (determineWhichEnemy)
            {
                case 0:
                    Instantiate(vampire, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 1:
                    Instantiate(vampirePoison, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 2:
                    Instantiate(caster, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 3:
                    Instantiate(casterPoison, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 4:
                    Instantiate(skeleZombie, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 5:
                    Instantiate(paladin, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                case 6:
                    Instantiate(summonerArena, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
                default:
                    Instantiate(summonerArena, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
                    break;
            }

            Instantiate(enemyEffect, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
        }

    }

    private void spawnMajorBoss()
    {
        if (isMajorBossSlotOpen)
        {
            GameObject myDemon = Instantiate(demon, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
            demonScript = demon.GetComponent(typeof(DemonArena)) as DemonArena;
            demonScript.bossHealthSlider = majorBossSlider;
            isMajorBossSlotOpen = false;
            
            demonScript.bossHealthSlider.gameObject.SetActive(true);
            
            Instantiate(SE_Warhorn2, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
        }
    }

    private void spawnMinorBoss()
    {
        //for spawning minotaurs, up to 4. Once the slot is filled, do not spawn more into that slot.
        if (isBossSlot1Open )
        {
            GameObject mino = Instantiate(minotaurArena, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
            minosScript1 = mino.GetComponent(typeof(MinotauroArena)) as MinotauroArena;

            minosScript1.bossHealthSlider = minorBossSlider1;
            minosScript1.slotNum = 1;
            isBossSlot1Open = false;            
            minosScript1.isAggroed = true;

            minosScript1.bossHealthSlider.gameObject.SetActive(true);
            
            Instantiate(SE_Warhorn, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
        }
        else if (isBossSlot2Open)
        {
            GameObject mino = Instantiate(minotaurArena, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
            minosScript2 = mino.GetComponent(typeof(MinotauroArena)) as MinotauroArena;

            minosScript2.bossHealthSlider = minorBossSlider2;
            minosScript2.slotNum = 2;
            isBossSlot2Open = false;
            minosScript2.isAggroed = true;
            minosScript2.bossHealthSlider.gameObject.SetActive(true);
            
            Instantiate(SE_Warhorn, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
        }
        else if (isBossSlot3Open)
        {
            GameObject mino = Instantiate(minotaurArena, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
            minosScript3 = mino.GetComponent(typeof(MinotauroArena)) as MinotauroArena;

            minosScript3.bossHealthSlider = minorBossSlider3;
            minosScript3.slotNum = 3;
            isBossSlot3Open = false;
            minosScript3.isAggroed = true;
            minosScript3.bossHealthSlider.gameObject.SetActive(true);
            
            Instantiate(SE_Warhorn, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
        }
        else if (isBossSlot4Open)
        {

            GameObject mino = Instantiate(minotaurArena, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
            minosScript4 = mino.GetComponent(typeof(MinotauroArena)) as MinotauroArena;

            minosScript4.bossHealthSlider = minorBossSlider4;
            minosScript4.slotNum = 4;
            isBossSlot4Open = false;
            minosScript4.isAggroed = true;
            minosScript4.bossHealthSlider.gameObject.SetActive(true);
            
            Instantiate(SE_Warhorn, enemySpawnLoc.transform.position, enemySpawnLoc.transform.rotation);
        }

        else
        {
            System.Console.WriteLine("Cannot spawn boss, all slots are full.");
        }
        //play special sound effect
        
    }
    public void majorBossDeath()
    {
        isMajorBossSlotOpen = true;
        majorBossSlider.gameObject.SetActive(false);
    }

    public void minorBossDeath(int minorBossNum)
    {
        if (minorBossNum == 1)
        {
            isBossSlot1Open = true;
            minorBossSlider1.gameObject.SetActive(false);
        }
        else if (minorBossNum == 2)
        {
            isBossSlot2Open = true;
            minorBossSlider2.gameObject.SetActive(false);
        }
        else if (minorBossNum == 3)
        {
            isBossSlot3Open = true;
            minorBossSlider3.gameObject.SetActive(false);
        }
        else if (minorBossNum == 4)
        {
            isBossSlot4Open = true;
            minorBossSlider4.gameObject.SetActive(false);
        }
    }



    // Update is called once per frame
    void Update () {
        if (!HeroController.isPaused)
        {
            StartCoroutine("handleBatteryEnemySpawns");
        }
        
    }

    private IEnumerator handleBatteryEnemySpawns()
    {
        time += 1.3f * Time.deltaTime;
        minorBossCooldown -= 1.3f * Time.deltaTime;
        majorBossCooldown -= 1.3f * Time.deltaTime;
        timeText.text = time.ToString("n2");
        //first few units of time - batteries will be plentiful
        yield return null;
        //***FIRST ERA
        if (time < firstEra)
        {
            era.text = "FIRST ERA";
            yield return null;
            if (Random.Range(1, bat_highSpawnChance) == 2)
            {
                yield return null;
                spawnBattery();
            }
            yield return null;
            if (Random.Range(1, enemy_abysmalSpawnChance) == 2)
            {
                yield return null;
                spawnEnemy(1); //pass in era - determines which enemies are eligible to spawn.
            }
            //Chance to spawn enemy here. later eras will have chance to spawn bosses. 
        }
        yield return null;
        //larger range of random numbers generated... less likely to occur
        //***SECOND ERA
        if (time > firstEra && time < secondEra)
        {
            era.text = "SECOND ERA";
            era.color = Color.yellow;
            yield return null;
            if (Random.Range(1, bat_mediumSpawnChance) == 2)
            {
                yield return null;
                spawnBattery();
            }
            yield return null;
            if (Random.Range(1, enemy_lowSpawnChance) == 2)
            {
                yield return null;
                spawnEnemy(2); //pass in era - determines which enemies are eligible to spawn.
            }
        }
        yield return null;
        //***THIRD ERA
        if (time > secondEra && time < thirdEra)
        {
            era.text = "THIRD ERA";
            era.color = orange;
            yield return null;
            if (Random.Range(1, bat_lowSpawnChance) == 2)
            {
                yield return null;
                spawnBattery();
            }
            yield return null;
            if (Random.Range(1, enemy_mediumSpawnChance) == 2)
            {
                yield return null;
                spawnEnemy(3); //pass in era - determines which enemies are eligible to spawn.
            }
        }
        yield return null;
        //***FINAL ERA
        if (time > thirdEra && time < finalEra)
        {
            era.text = "FOURTH ERA";
            era.color = deepOrange;
            yield return null;
            if (Random.Range(1, bat_abysmalSpawnChance) == 2)
            {
                yield return null;
                spawnBattery();
            }
            yield return null;
            if (Random.Range(1, enemy_highSpawnChance) == 2)
            {
                yield return null;
                spawnEnemy(4); //pass in era - determines which enemies are eligible to spawn.
            }
        }
        yield return null;
        //***ENDGAME
        if (time > finalEra)
        {
            era.text = "FINAL ERA";
            era.color = Color.red;
            yield return null;
            if (Random.Range(1, bat_endgameSpawnChance) == 2)
            {
                yield return null;
                spawnBattery();
            }
            yield return null;
            if (Random.Range(1, enemy_endgameSpawnChance) == 2)
            {
                yield return null;
                spawnEnemy(5); //pass in era - determines which enemies are eligible to spawn.
            }

        }
        yield return null;
    }

}
