using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FoodMinigameTask : TaskManager
{

	[SerializeField]
	private GameObject m_foodPrefab;

	[SerializeField]
	private Vector3 m_workerFoodOffset;

	[SerializeField] 
	private GameObject m_miniGame;

	private bool m_isMiniGameEnabled;
	
	public bool IsMiniGameEnabled
	{
		get
		{
			return m_isMiniGameEnabled;
		}
		set
		{
			m_isMiniGameEnabled = value;
			if (IsDoingTask)
			{
				m_miniGame.gameObject.SetActive(value);
			}
		}
	}
	
		
	// Update is called once per frame
	protected override void Update () 
	{
		if (IsDoingTask)
		{
			if (!m_isMiniGameEnabled)
			{
				m_taskProgress += m_workSpeed * Time.deltaTime;
				UpdateProgressBar();
			}
			if (m_taskProgress >= m_progressForCompletion)
			{
				m_taskProgress = 0f;
				UpdateProgressBar();
				SetProgressBarActive(false);
				MakeFood();
			}
		}
	}
	
	private void MakeFood()
	{
		GameObject food = Instantiate(m_foodPrefab, m_workerDoingTask.transform);
		food.transform.localPosition = m_workerFoodOffset;
		m_workerDoingTask.AddFood(food);
		OnWorkingChanged(false, m_workerDoingTask);
	}

	public void SetTimeToMake(float time)
	{
		m_progressForCompletion = time;
	}
	
	public override bool OnWorkingChanged(bool isWorking, Worker worker)
	{
		if (isWorking)
		{
			if (worker.HoldingSomething)
			{
				return false;
			}
		}
		base.OnWorkingChanged(isWorking, worker);
		if (m_isMiniGameEnabled)
		{
			m_miniGame.gameObject.SetActive(isWorking);
		}
		SetProgressBarActive(true);
		return true;
	}
 }
