using UnityEngine;
using System.Collections;

//
// mixin approach for interactive objects: we will develop a series of small classes that communicate 'aspects' of the behavior
// with one another.
//
public class MixinSP : MonoBehaviour {

	public string name; // a unique identifier for this 'aspect' of the object behavior
	private GameObject recipient; // the gameobject who receives the message from this mxn

	public void SetRecipient(GameObject r)
	{
		recipient = r;
	}

	public GameObject GetRecipient()

	{
		return recipient;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
