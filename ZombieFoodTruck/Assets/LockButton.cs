using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnUnlock()
	{
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
		rigidbody.bodyType = RigidbodyType2D.Dynamic;
		rigidbody.AddForce(new Vector2(4, 1) * 50f);
	//	rigidbody.AddTorque(20f);
	}
}
