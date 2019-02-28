using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichCustomer : Customer
{

	[SerializeField] private int m_tipMultiplier;

	// Use this for initialization
	void Start () {
		m_customerType = CustomerType.Rich;
    }

	public override int GetTipAmount()
	{
		return base.GetTipAmount() * m_tipMultiplier;
	}
	
}
