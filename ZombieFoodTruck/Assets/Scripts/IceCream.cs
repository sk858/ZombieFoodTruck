using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCream : Food
{

	private float m_timeToMelt;
	
	private TaskManager m_taskManager;

	private Worker m_worker;

	private ServeFoodManager m_tray;

	private bool m_isInMachine;

	private bool m_isHeldByWorker;

	private bool m_isOnTray;

	private bool m_isMelting;

	private float m_timer;

	public void Init(Worker worker, float m_timeLeft)
	{
		m_isInMachine = false;
		m_isHeldByWorker = true;
		m_worker = worker;
		m_timeToMelt = m_timeLeft;
		m_isMelting = true;
	}

	public void OnServe(ServeFoodManager tray)
	{
		m_tray = tray;
		m_isOnTray = true;
		m_isHeldByWorker = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_isMelting)
		{
			m_timer += Time.deltaTime;
			if (m_timer >= m_timeToMelt)
			{
				Melt();
			}
			
		}
	}

	private void Melt()
	{
		TruckLogger.Instance.LogIceCreamMelted ();
		if(m_isHeldByWorker)
		{
			m_worker.RemoveFood();
		}
		else if (m_isOnTray)
		{
			m_tray.RemoveFood(this);
		}
		Destroy (gameObject);
	}

	public void OnFinished()
	{
		m_isMelting = false;
	}
	
}
