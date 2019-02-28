using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCustomer : MonoBehaviour
{

	private Customer.CustomerType m_type;

	public Customer.CustomerType Type
	{
		get { return m_type; }
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetType(Customer.CustomerType type)
	{
		m_type = type;
	}
}
