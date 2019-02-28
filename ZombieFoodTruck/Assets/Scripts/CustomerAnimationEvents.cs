using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimationEvents : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void OnAngryAnimFinished()
	{
		GetComponentInParent<Customer>().OnAngryAnimFinished();
	}

	public void OnBadgeAnimFinished()
	{
		GetComponentInParent<PoliceCustomer>();
	}

	public void OnFineAnimFinished()
	{
		CustomerManager.Instance.OnFineFinished(GetComponentInParent<Customer>());
	}
}
