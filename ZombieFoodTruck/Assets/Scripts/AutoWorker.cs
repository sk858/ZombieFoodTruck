using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWorker : Worker
{
	[SerializeField]
	private float m_chargeTime;

	[SerializeField]
	private float m_activeTime;
	
	[SerializeField]
	private ProgressBar m_progressBar;

	private Vector3 m_chargingPos;

	private bool m_isCharging;

	private float m_timeSpentWorking;

	private float m_timeSpentCharging;

	private GrabbableObject m_grabbableObject;

	public bool IsGrabbable
	{
		get
		{
			return (!IsWorking) && (!m_isCharging); 
		}
	}

	new void Awake()
	{
		base.Awake();
		m_chargingPos = transform.position;
		m_progressBar.Value = 1;
		m_grabbableObject = GetComponent<GrabbableObject>();
	}

	new void OnDestroy()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If charging, keep track of time spent charging
		//and leave charging state when finished
		if (m_isCharging)
		{
			m_timeSpentCharging += Time.deltaTime;
			m_progressBar.Value = m_timeSpentCharging / m_chargeTime;
			if (m_timeSpentCharging >= m_chargeTime)
			{
				m_isCharging = false;
				m_grabbableObject.IsGrabbingDisabled = false;
				m_timeSpentCharging = 0f;
			}
		}
		
		//If working, keep track of time spent working
		//and go into charging state when finished, moving
		//back to its original spot
		if (IsWorking)
		{
			m_timeSpentWorking += Time.deltaTime;
			m_progressBar.Value = (m_activeTime - m_timeSpentWorking) / m_activeTime;
			if (m_timeSpentWorking >= m_activeTime)
			{
				m_isWorking = false;
				m_isCharging = true;
				m_timeSpentWorking = 0f;
				m_grabbableObject.Undock();
				transform.position = m_chargingPos;
			}
		}
	}
	
	protected void OnDockChanged(bool isDocked, GameObject dock)
	{
		//Notify the station that the worker is docked
		dock.GetComponent<TaskManager>().OnWorkingChanged(isDocked, this);
		if (isDocked)
		{
			m_workerStation = dock.GetComponent<TaskManager>();
			m_isWorking = true;
			m_grabbableObject.IsGrabbingDisabled = true;

		}
		else
		{
			m_workerStation = null;
			m_isWorking = false;
		}
	}
}
