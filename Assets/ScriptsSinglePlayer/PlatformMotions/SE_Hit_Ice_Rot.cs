using UnityEngine;
using System.Collections;

public class SE_Hit_Ice_Rot : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        spinIce();
    }

    void spinIce()
    {
        transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
    }
}
