using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatienceBuff : BaseConsumable
{

	[SerializeField] private float m_buffAmount;

	private string m_patienceModId;
	
	
	public override void Activate()
	{
		Debug.Log("PATIENCE BOOST STARTING");
		m_patienceModId = "PatienceConsumable" + GetInstanceID();
		PatienceModManager.Instance.AddPatienceModifier(m_patienceModId, m_buffAmount);
	}

	public override void Deactivate()
	{
		Debug.Log("PATIENCE BOOST ENDED");
		PatienceModManager.Instance.RemovePatienceModifier(m_patienceModId);
	}

	public override string GetAlertMessage()
	{
		return "CUSTOMER PATIENCE FROZEN";
	}
	
}
