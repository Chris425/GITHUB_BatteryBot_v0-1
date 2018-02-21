using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PureHorizontalRotation : MonoBehaviour {
    public float rotSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0.0f, 0.0f, rotSpeed) * 5 * Time.deltaTime);
    }


}
