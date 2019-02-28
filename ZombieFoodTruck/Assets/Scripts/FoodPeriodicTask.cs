using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FoodPeriodicTask : TaskManager
{

	[SerializeField]
	private GameObject m_foodPrefab;

	[SerializeField]
	private Vector3 m_workerFoodOffset;

	[SerializeField] 
	private GameObject m_pointingHand;

	[SerializeField] 
	private GameObject m_foodReadyUI;

	private bool m_isMiniGameEnabled;

	[SerializeField] 
	private float[] m_attentionPointBases;

	[SerializeField] 
	private float m_attentionPointVariance;

	private float[] m_currentAttentionPoints;

	private int m_attentionPointCounter;

	private bool m_needsAttention;

	private bool m_taskStarted;

	private bool m_foodReady;

	private List<Upgrade> m_appliedStationsUpgrades = new List<Upgrade>();

	protected override void Awake()
	{
		base.Awake();
		m_currentAttentionPoints = new float[m_attentionPointBases.Length];
		ResetAttentionPoints();
	}
	// Update is called once per frame
	protected override void Update () 
	{
		if (IsDoingTask)
		{
			if (!m_needsAttention 
			    && m_attentionPointCounter < m_currentAttentionPoints.Length
			    && m_taskProgress >= m_currentAttentionPoints[m_attentionPointCounter])
			{
				m_needsAttention = true;
				m_pointingHand.SetActive(true);
			}
			if (!m_needsAttention)
			{
				m_taskProgress += m_workSpeed * Time.deltaTime;
			}
			UpdateProgressBar();
			if (m_taskProgress >= m_progressForCompletion)
			{
				m_taskProgress = 0f;
				UpdateProgressBar();
				SetProgressBarActive(false);
				MakeFood();
				TutorialManager.Instance.OnTaskFinished (m_taskType);
			}
		}
	}
	
	private void MakeFood()
	{
		m_isDoingTask = false;
		m_foodReady = true;
		m_foodReadyUI.SetActive(true);
	}

	private void GiveFoodToWorker(Worker worker)
	{
		GameObject food = Instantiate(m_foodPrefab, m_workerDoingTask.transform);
		food.transform.localPosition = m_workerFoodOffset;
		m_workerDoingTask.AddFood(food);
		ResetAttentionPoints();
		m_foodReady = false;
		m_foodReadyUI.SetActive(false);
		TutorialManager.Instance.OnTaskStarted (TaskType.Tray);
	}

	public void SetTimeToMake(float time)
	{
		m_progressForCompletion = time;
	}

	public void StationTimeUpgrade(float timeUpgrade)
	{
		m_progressForCompletion -= timeUpgrade;
	}
	
	public void AddStationUpgrade(Upgrade upgrade)
	{
		m_appliedStationsUpgrades.Add(upgrade);
		
	}
	
	public bool StationsUpgradeApplied(Upgrade upgrade)
	{
		Debug.Log(m_appliedStationsUpgrades);
		return m_appliedStationsUpgrades.Contains(upgrade);
		
	}
	
	public override bool OnWorkingChanged(bool isWorking, Worker worker)
	{
		if (isWorking)
		{
			if (worker.HoldingSomething)
			{
				return false;
			}
			TutorialManager.Instance.OnTaskStarted (m_taskType);
		}
		if (m_foodReady)
		{
			GiveFoodToWorker(worker);
			return false;
		}
		if (IsDoingTask)
		{
			if (m_needsAttention)
			{
				m_needsAttention = false;
				m_attentionPointCounter++;
				m_pointingHand.SetActive(false);
			}
		}
		else
		{
			m_isDoingTask = true;
		}
		m_workerDoingTask = worker;
		m_workSpeed = worker.GetSpeed(m_taskType);
		SetProgressBarActive(true);
		return false;
	}

	public void ResetAttentionPoints()
	{
		m_attentionPointCounter = 0;
		for (int i = 0; i < m_currentAttentionPoints.Length; i++)
		{
			m_currentAttentionPoints[i] = m_attentionPointBases[i] +
			                              Random.Range(-m_attentionPointVariance, m_attentionPointVariance);
		}
	}

	public override bool CanDock(Worker worker)
	{
		if (m_isDoingTask && !m_needsAttention)
		{
			return false;
		}
		if (worker.HoldingSomething)
		{
			return false;
		}
		return true;
	}
}
