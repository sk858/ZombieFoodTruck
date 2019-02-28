using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IceCreamManager : TaskManager
{

	[SerializeField]
	private GameObject m_foodPrefab;

	[SerializeField]
	private Vector3 m_workerFoodOffset;

	[SerializeField] 
	private GameObject m_foodReadyUI;

	[SerializeField] 
	private float m_timeToMelt;

	private float m_timer;

	private bool m_taskStarted;

	private bool m_foodReady;

	private List<Upgrade> m_appliedStationsUpgrades = new List<Upgrade>();
	// Update is called once per frame
	protected override void Update () 
	{
		if (IsDoingTask)
		{
			m_taskProgress += m_workSpeed * Time.deltaTime;
			UpdateProgressBar();
			if (m_taskProgress >= m_progressForCompletion)
			{
				m_taskProgress = 0f;
				UpdateProgressBar();
				SetProgressBarActive(false);
				MakeFood();
				TutorialManager.Instance.OnTaskFinished (TaskType.IceCream);
			}
		}
		if (m_foodReady)
		{
			m_timer += Time.deltaTime;
			if (m_timer >= m_timeToMelt)
			{
				TruckLogger.Instance.LogIceCreamMelted ();
				Melt();
			}
			
		}
	}

	private void Melt()
	{
		m_foodReady = false;
		m_foodReadyUI.SetActive(false);
		m_timer = 0f;
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
		food.GetComponent<IceCream>().Init(worker, m_timeToMelt - m_timer);
		m_workerDoingTask.AddFood(food);
		m_foodReady = false;
		m_foodReadyUI.SetActive(false);
		TutorialManager.Instance.OnTaskStarted (TaskType.Tray);
		TruckLogger.Instance.LogFoodGrabbed ();
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
			if (m_foodReady)
			{
				GiveFoodToWorker(worker);
				return false;
			}
			else if (!m_isDoingTask)
			{
				TutorialManager.Instance.OnTaskStarted(TaskType.IceCream);
				TruckLogger.Instance.LogStartIceCream ();
				m_isDoingTask = true;
			}
			else
			{
				return false;
			}
		}
		m_workerDoingTask = worker;
		m_workSpeed = worker.GetSpeed(m_taskType);
		SetProgressBarActive(true);
		return false;
	}

	public override bool CanDock(Worker worker)
	{
		if (EatingMechanicManager.Instance.IsHungerEnabled && EatingMechanicManager.Instance.IsEnergyEmpty)
		{
			return false;
		}
		if (m_isDoingTask)
		{
			return false;
		}
		if (!worker.HoldingSomething)
		{
			return true;
		}
		return false;
	}
}
