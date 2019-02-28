using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseConsumable : MonoBehaviour
{
	
	[SerializeField]
	private float m_duration;

	private float m_startTime;
	private float m_endTime;
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// If the time's out then destroy the consumable
		if (Time.time >= m_endTime)
		{
			Deactivate();
			Destroy(gameObject);
			return;
		}
		UpdateConsumable();
	}

	void Awake()
	{
		Activate();
		m_endTime = Time.time + m_duration;
	}

	public virtual void Activate() { }

	public virtual void UpdateConsumable() { }

	public virtual void Deactivate() { }

	public virtual string GetAlertMessage()
	{
		return "SET MY MESSAGE";
	}

}
