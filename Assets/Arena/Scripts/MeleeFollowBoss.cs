using UnityEngine;
using System.Collections;

public class MeleeFollowBoss : MonoBehaviour
{
    private Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    //get location of character!
    public GameObject target;
    public float distanceX;
    public float distanceZ;
    public float distanceY;
    private float cooldown = 6.0f;
    private float cooldownTimer;
    private int bossHealth = 5;

    public GameObject BiteSpecEffect1;
    public GameObject BiteSpecEffect2;
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
        //case when your player projectile hits the boss
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
                //kill the skeleton
                Destroy(this.gameObject);

                //possibly spawn some loot!


                int randomNum = Random.Range(1, 11);
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
        cooldownTimer -= 0.03f;

        distanceX = this.transform.position.x - target.transform.position.x;
        distanceZ = this.transform.position.z - target.transform.position.z;
        distanceY = this.transform.position.y - target.transform.position.y;
        //Debug.Log (distance);


        if ((distanceX > -2.7 && distanceX < 2.7) && (distanceZ > -2.7 && distanceZ < 2.7) && (distanceY > -3.7 && distanceY < 3.7) && cooldownTimer < 0.01f)
        {
            anim.SetTrigger("isAttacking");
            //we are in range. Start draining battery
            Debug.Log("Skeleton Attacks!");
            HeroController.battery -= 50;
            cooldownTimer = cooldown;

            int randomNum = Random.Range(1, 3);
            switch (randomNum)
            {
                case 1:
                    Instantiate(BiteSpecEffect1, this.transform.position, this.transform.rotation);
                    break;
                case 2:
                    Instantiate(BiteSpecEffect2, this.transform.position, this.transform.rotation);
                    break;
            }

        }
        else
        {
            if (agent.isActiveAndEnabled)
            {
                agent.SetDestination(target.transform.position);
            }
            
        }

    }

}