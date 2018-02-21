using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoisonShot : MonoBehaviour {


    //public Transform Target;
    //public float firingAngle = 45.0f;
    //public float gravity = 9.8f;

    //public Transform Projectile;
    //private Transform myTransform;


    public GameObject poisonWell;


    // Update is called once per frame
    void Update () {

        //StartCoroutine("SimulateProjectile");
	}




    public void OnCollisionEnter(Collision other)
    {
        //when colliding, spawn poison wells
        float posX = this.gameObject.transform.position.x;
        float posY = this.gameObject.transform.position.y;
        float posZ = this.gameObject.transform.position.z;
        Vector3 currPosOfProjectile = new Vector3(posX, posY, posZ);
        Quaternion noRot = new Quaternion();
        Instantiate(poisonWell, currPosOfProjectile, noRot);
        Destroy(this.gameObject);
    }


        //void Awake()
        //{
        //    myTransform = transform;
        //}

        //void Start()
        //{
        //    StartCoroutine(SimulateProjectile());
        //}


        //not currently used.... need to raycast target, or scan to get enemies find closest and choose it as target->performance hit
    //IEnumerator SimulateProjectile()
    //{
    //    // Short delay added before Projectile is thrown
    //    yield return new WaitForSeconds(1.5f);

    //    // Move projectile to the position of throwing object + add some offset if needed.
    //    Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

    //    // Calculate distance to target
    //    float target_Distance = Vector3.Distance(Projectile.position, Target.position);

    //    // Calculate the velocity needed to throw the object to the target at specified angle.
    //    float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

    //    // Extract the X  Y componenent of the velocity
    //    float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
    //    float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

    //    // Calculate flight time.
    //    float flightDuration = target_Distance / Vx;

    //    // Rotate projectile to face the target.
    //    Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);

    //    float elapse_time = 0;

    //    while (elapse_time < flightDuration)
    //    {
    //        Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

    //        elapse_time += Time.deltaTime;

    //        yield return null;
    //    }
    //}
}
