using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CuttingBoardManager : TaskManager
{

	[SerializeField] 
	private GameObject m_potatoPrefab;

	[SerializeField]
	private Vector3 m_workerFoodOffset;

	[SerializeField] 
	private bool m_holdToTask;

	private List<Upgrade> m_appliedStationsUpgrades = new List<Upgrade>();
	
	private bool m_isHalfway;

	private bool m_foodReady;

	[SerializeField]
	private GameObject m_foodReadyUI;

	[SerializeField] 
	private GameObject[] m_progressBars;

	[SerializeField] private Animator m_animator;

	private bool m_isInstant;

	protected override void Awake()
	{
		base.Awake();
		//m_animator = gameObject.GetComponent<Animator>();
		SetFoodProgressTier(0);
	}
		
	// Update is called once per frame
	protected override void Update () 
	{
		if (IsDoingTask)
		{
			if (!Input.GetKey(KeyCode.Space) && m_holdToTask)
			{
				InputManager.Instance.ForceStopWorking();
			}
			m_taskProgress += Time.deltaTime;
			if (!m_isHalfway && m_taskProgress >= m_progressForCompletion / 2f)
			{
				m_isHalfway = true;
				m_animator.SetTrigger("Halfway");
			}
			if (m_taskProgress >= m_progressForCompletion)
			{
				m_foodReady = true;
				m_taskProgress = 0f;
				UpdateProgressBar();
				TutorialManager.Instance.OnTaskFinished (TaskType.Cutting);
				TruckLogger.Instance.LogFinishCutting ();
				m_foodReadyUI.SetActive(true);
				OnTaskFinishedEvent.Invoke (TaskType.Cutting);
				m_animator.SetTrigger("Done");
				MakeFood(m_workerDoingTask);
				InputManager.Instance.ForceStopWorking();
			}
			UpdateProgressBar();
		}
	}
	
	private void MakeFood(Worker worker)
	{
		GameObject food = Instantiate(m_potatoPrefab, worker.transform);
		food.transform.localPosition = m_workerFoodOffset;
		worker.AddPotatoes(food);
		m_isHalfway = false;
		m_foodReady = false;
		m_foodReadyUI.SetActive(false);
		SetProgressBarActive(false);
	}

	public void SetTimeToMake(float time)
	{
		m_progressForCompletion = time;
	}
	
	public void StationTimeUpgrade(float timeUpgrade)
	{
		m_progressForCompletion = timeUpgrade;
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

	private void PutPotatoesBack(Worker worker)
	{
		worker.RemovePotatoes();
		m_foodReady = true;
		m_foodReadyUI.SetActive(true);
		worker.QuickTaskStart();
	}
	
	public override bool OnWorkingChanged(bool isWorking, Worker worker)
	{
		if (worker.HoldingSomething && !worker.HoldingPotatoes)
		{
			return false;
		}
		if (isWorking)
		{
			if (worker.HoldingPotatoes)
			{
				PutPotatoesBack(worker);
				return false;
			}
			if (m_foodReady)
			{
				MakeFood(worker);
				worker.QuickTaskStart();
				return false;
			}
			if (m_isInstant)
			{
				return false;
			}
			if (m_taskProgress == 0f)
			{
				SetProgressBarActive(true);
			}
			worker.GetComponent<Animator>().SetBool("IsCuttingPotatoes", true);
			worker.transform.position = transform.position + m_workerStationOffset;
			TutorialManager.Instance.OnTaskStarted(TaskType.Cutting);
			TruckLogger.Instance.LogResumeCutting();
			if (m_taskProgress == 0f)
			{
				m_animator.SetTrigger("Started");
			}
		}
		else
		{
			m_workerDoingTask.GetComponent<Animator>().SetBool("IsCuttingPotatoes", false);
		}
		base.OnWorkingChanged(isWorking, worker);
		return true;
	}

	public override bool CanDock(Worker worker)
	{
		if (EatingMechanicManager.Instance.IsHungerEnabled && EatingMechanicManager.Instance.IsEnergyEmpty)
		{
			return false;
		}
		if (worker.HoldingPotatoes)
		{
			return true;
		}
		if (m_foodReady && !worker.HoldingSomething)
		{
			return true;
		}
		if (m_isDoingTask)
		{
			return false;
		}
		return !worker.HoldingSomething;
	}

	public void OnUpgradeApply(int tier)
	{
		ChangeProgressBar(m_progressBars[tier]);
	}

	public void SetFoodProgressTier(int tier)
	{
		m_progressBarParent = m_progressBars[tier];
		m_progressBar = m_progressBarParent.GetComponentInChildren<ProgressBar>();
	//	SetProgressBarActive(false);
	}

	public void OnPotatoAnimationDone()
	{
		m_foodReady = true;
		m_taskProgress = 0f;
		UpdateProgressBar();
		m_foodReadyUI.SetActive(true);
		OnTaskFinishedEvent.Invoke (TaskType.Cutting);
		m_animator.SetTrigger("Done");
	}

	public void OnPotatoesUsed()
	{
		if (m_isInstant)
		{
			m_animator.SetTrigger("Started");
		}
	}

	public void MakeInstant()
	{
		m_isInstant = true;
		if (!gameObject.activeSelf) {
			Debug.Log ("cutting board was not active");
			gameObject.SetActive (true);
		}
		m_animator.SetBool("IsInstant", true);
		OnPotatoesUsed();
	}
	
}
