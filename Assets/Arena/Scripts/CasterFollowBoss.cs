using UnityEngine;
using System.Collections;

public class CasterFollowBoss : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    //get location of character!
    public GameObject target;
    public float distanceX;
    public float distanceZ;
    public float distanceY;
    private float cooldown = 5.0f;
    private float cooldownTimer;
    private int bossHealth = 2;

    public GameObject CasterSpecEffect;
    public GameObject CasterSpawnLoc;
    public GameObject objToSpawn;
    public GameObject DeathSpecEffect;

    public GameObject RedBattery;
    public GameObject GreenBattery;
    public GameObject SuperBattery;

    void OnEnable()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.Find("BatteryBot");

        anim = this.GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
    }

    public void OnCollisionEnter(Collision other)
    {
        //case when your player projectile hits the caster
        if (other.gameObject.name.Contains("PlayerShot"))
        {
            if (HeroController.isSuperCharged == true)
            {
                bossHealth -= 2;
            }
            else
            {
                bossHealth -= 1;
            }

            Instantiate(DeathSpecEffect, other.transform.position, this.transform.rotation);
            //consume playershot
            Destroy(other.gameObject);
            if (bossHealth <= 0)
            {
                Destroy(this.gameObject);
                //possibly spawn some loot!
                //CDC add new special loot for boss monsters?


                int randomNum = Random.Range(1, 10);
                if (randomNum <= 2)
                {
                    int posOffset = Random.Range(-4, 4);
                    int rotOffset1 = Random.Range(1, 180);
                    int rotOffset2 = Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(RedBattery, spawnPos, spawnRot);

                }
                else if (randomNum > 2 && randomNum <= 4)
                {
                    int posOffset = Random.Range(-4, 4);
                    int rotOffset1 = Random.Range(1, 180);
                    int rotOffset2 = Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(GreenBattery, spawnPos, spawnRot);

                }
                else if (randomNum == 5)
                {
                    int posOffset = Random.Range(-4, 4);
                    int rotOffset1 = Random.Range(1, 180);
                    int rotOffset2 = Random.Range(1, 180);
                    Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y + posOffset, this.transform.position.z);
                    Quaternion spawnRot = new Quaternion(this.transform.rotation.x + rotOffset1, this.transform.rotation.y + rotOffset2, this.transform.rotation.z + rotOffset1, this.transform.rotation.w + rotOffset2);
                    Instantiate(SuperBattery, spawnPos, spawnRot);

                }

                Destroy(other.gameObject);
                
        }

        }
    }

    void Update()
    {
        moveToPlayer();
    }

    void moveToPlayer()
    {
        //make the spawnloc aim at the player - so you won't be safe up high either!
        Vector3 targetPostition = new Vector3(target.transform.position.x,
                                       target.transform.position.y,
                                       target.transform.position.z);
        CasterSpawnLoc.transform.LookAt(targetPostition);

        //let the caster face the player at all times
        this.transform.LookAt(targetPostition);


        cooldownTimer -= 0.03f;

        distanceX = this.transform.position.x - target.transform.position.x;
        distanceZ = this.transform.position.z - target.transform.position.z;
        distanceY = this.transform.position.y - target.transform.position.y;
        //Debug.Log (distance);


        if ((distanceX > -8.0 && distanceX < 8.0) && (distanceZ > -8.0 && distanceZ < 8.0) && cooldownTimer < 0.01f)
        {
            agent.SetDestination(target.transform.position);
            anim.SetTrigger("isAttacking");
            //we are in range. Start shooting
            Debug.Log("Caster is readying a fireball!");
            Instantiate(objToSpawn, CasterSpawnLoc.transform.position, this.transform.rotation);
            cooldownTimer = cooldown;
            Instantiate(CasterSpecEffect, this.transform.position, this.transform.rotation);
                  
        }
        else
        {
            if (agent.isActiveAndEnabled)
            {
                agent.SetDestination(target.transform.position);
                anim.SetTrigger("isWalking");
            }
            
        }

    }

}