using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerBoost : MonoBehaviour
{
	
	[SerializeField]
	private float m_timeLasting;

	[SerializeField] 
	private float m_buffValue;
	
	[SerializeField] 
	private TaskManager.TaskType m_buffType;

	[SerializeField] 
	private float m_timeToRecharge;

	[SerializeField] 
	private ProgressBar m_progressBar;

	private Color m_originalButtonColor;

	private Button m_button;

	private float m_oldBuffValue;

	private float m_timeSpentRecharging;

	private int m_idOfWorkerBuffed = -1;

	private bool m_isRecharging;

	private bool m_inUse;

	private bool m_isSelecting;

	private void Awake()
	{
		m_button = GetComponent<Button>();
		m_originalButtonColor = m_button.colors.normalColor;
	}
	
	private void Update()
	{
		
		//If charging, keep track of time spent charging
		//and leave charging state when finished
		if (m_isRecharging)
		{
			m_timeSpentRecharging += Time.deltaTime;
			m_progressBar.Value = m_timeSpentRecharging / m_timeToRecharge;
			if (m_timeSpentRecharging >= m_timeToRecharge)
			{
				m_isRecharging = false;
				m_timeToRecharge = 0f;
				m_button.interactable = true;
			}
		}
	}
	
	public void UseBuff(int workerId)
	{
		m_inUse = true;
		WorkerManager workerManager = WorkerManager.Instance;
		
		//Keep track of the workers old speed
		m_oldBuffValue = workerManager.GetWorker(workerId).GetSpeed(m_buffType);
		
		//Buff worker
		WorkerManager.Instance.BuffWorker(m_buffType, workerId, m_buffValue);
		
		//Exit worker selection mode
		workerManager.DisableWorkerSelection();
		m_idOfWorkerBuffed = workerId;
		m_button.interactable = false;
		StartCoroutine("WaitForDebuff");
	}

	public void OnButtonClick()
	{
		if (m_inUse || m_isRecharging)
		{
			return;
		}
		//If selecting a worker and the button is clicked,
		//exit selection mode
		if (m_isSelecting)
		{
			WorkerManager.Instance.DisableWorkerSelection();
			m_isSelecting = false;
			ColorBlock colorBlock = m_button.colors;
			colorBlock.normalColor = m_originalButtonColor;
			m_button.colors = colorBlock;
			return;
		}
		//If WorkerManager is not already trying to select a worker,
		//enable worker selection for this buff
		if (WorkerManager.Instance.EnableWorkerSelection(UseBuff))
		{

			m_isSelecting = true;
			ColorBlock colorBlock = m_button.colors;
			colorBlock.normalColor = m_button.colors.pressedColor;
			m_button.colors = colorBlock;
		}
	}

	public IEnumerator WaitForDebuff()
	{
		//Wait for the usage period of the buff
		float timeWaiting = 0f;
		while (timeWaiting < m_timeLasting)
		{
			timeWaiting += Time.deltaTime;
			m_progressBar.Value = (m_timeLasting - timeWaiting)/m_timeLasting;
			yield return null;
		}
		
		//Set the worker's speed back to normal
		WorkerManager.Instance.BuffWorker(m_buffType, m_idOfWorkerBuffed, m_oldBuffValue);
		m_isRecharging = true;
		m_inUse = false;
		m_idOfWorkerBuffed = -1;
		m_oldBuffValue = 0;
	}
}
