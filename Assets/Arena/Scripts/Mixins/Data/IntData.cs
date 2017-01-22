using UnityEngine;
using System.Collections;

public class IntData : Data {

	public int data;	// this is the field we operate on (p.o.d.)

	public void SetData(int d)
	{
		data = d;
	}

	public void IncData(IntData delta)
	{
		if (delta != null)
			data += delta.GetData();
		else
			Debug.Log("IntData::IncData(): error, delta was null");
	}

	public void IncData(int delta)
	{
		data += delta;
	}

	public int GetData()
	{
		return data;
	}
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
