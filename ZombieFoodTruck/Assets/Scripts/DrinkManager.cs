using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DrinkManager : TaskManager
{

	[SerializeField]
	private GameObject m_foodPrefab;

	[SerializeField]
	private Vector3 m_workerFoodOffset;

	[SerializeField] 
	private GameObject m_foodReadyUI;

	[SerializeField] 
	private Text m_iceDisplayText;

	[SerializeField] 
	public int m_iceLimit;

	[SerializeField] 
	private int m_iceIncCount;

	[SerializeField]
	private int m_iceCount;

	private bool m_taskStarted;

	private bool m_foodReady;

	private AudioSource m_audioSource;

	private bool m_waitingForAnim;

	private List<Upgrade> m_appliedStationsUpgrades = new List<Upgrade>();

	private Animator m_animator;
	
	protected override void Awake()
	{
		base.Awake();
		UpdateIceText();
		m_audioSource = GetComponent<AudioSource>();
		m_animator = GetComponent<Animator>();
	}
	
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
				MakeFood();
				m_audioSource.Stop();
				m_waitingForAnim = true;
				m_animator.SetTrigger("Complete");
			}
		}
	}
	
	private void MakeFood()
	{
		m_isDoingTask = false;
	}

	private void GiveFoodToWorker(Worker worker)
	{
		GameObject food = Instantiate(m_foodPrefab, m_workerDoingTask.transform);
		food.transform.localPosition = m_workerFoodOffset;
		m_workerDoingTask.AddFood(food);
		TutorialManager.Instance.OnTaskStarted(TaskType.Tray);
		TruckLogger.Instance.LogFoodGrabbed();
		m_foodReady = false;
		m_foodReadyUI.SetActive(false);
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
	
	public void RefillIce()
	{
		m_iceCount = m_iceLimit;
		UpdateIceText();
	}

	public void IncreaseIceLimit(int NewIceLimit)
	{
		m_iceLimit = NewIceLimit;
		UpdateIceText();
	}
	
	public void UpdateIceText()
	{
		m_iceDisplayText.text = m_iceCount + "/" + m_iceLimit;
	}
	
	public override bool OnWorkingChanged(bool isWorking, Worker worker)
	{
		if (m_waitingForAnim)
		{
			return false;
		}
		
		if (isWorking)
		{
			if (!m_foodReady && worker.HoldingFood && worker.ObjectHeld.GetComponent<Food>().Type == Food.FoodType.Drink)
			{
				PutFoodBack(worker);
				return false;
			}
			if (worker.HoldingIce)
			{
				RefillIce();
				worker.RemoveIce();
				TutorialManager.Instance.OnTaskFinished (TaskType.Ice);
				TruckLogger.Instance.LogDepositIce ();
				SoundManager.Instance.IceRefilled();
				return false;
			}
			else if (worker.HoldingSomething)
			{
				return false;
			}
			else if (m_foodReady)
			{
				GiveFoodToWorker(worker);
				worker.QuickTaskStart();
				return false;
			}
			else if (m_iceCount > 0 && !m_isDoingTask)
			{
				m_iceCount--;
				if (m_iceCount <= 0) {
					TutorialManager.Instance.OnIceEmpty ();
				}
				m_isDoingTask = true;
				UpdateIceText();
				TutorialManager.Instance.OnTaskStarted(TaskType.Soda);
				TruckLogger.Instance.LogStartDrink ();
				m_audioSource.Play();
				worker.QuickTaskStart();
				m_animator.SetTrigger("Started");
			}
			else
			{
				return false;
			}
		}
		m_workerDoingTask = worker;
		m_workSpeed = worker.GetSpeed(m_taskType);
		return false;
	}

	public override bool CanDock(Worker worker)
	{
		if (m_waitingForAnim)
		{
			return false;
		}
		if (EatingMechanicManager.Instance.IsHungerEnabled && EatingMechanicManager.Instance.IsEnergyEmpty)
		{
			return false;
		}
		if (m_isDoingTask)
		{
			return false;
		}
		if (m_foodReady && !worker.HoldingSomething)
		{
			return true;
		}
		if (!m_foodReady && worker.HoldingFood && worker.ObjectHeld.GetComponent<Food>().Type == Food.FoodType.Drink)
		{
			return true;
		}
		if (worker.HoldingIce)
		{
			return true;
		}
		if (!worker.HoldingSomething && m_iceCount > 0)
		{
			return true;
		}
		return false;
	}
	
	private void PutFoodBack(Worker worker)
	{
		GameObject objectHeld = worker.ObjectHeld;
		worker.RemoveFood();
		Destroy(objectHeld);
		m_foodReady = true;
		m_foodReadyUI.SetActive(true);
		worker.QuickTaskStart();
	}

	public void OnAnimFinished()
	{
		m_waitingForAnim = false;
		m_foodReady = true;
		m_foodReadyUI.SetActive(true);
		TutorialManager.Instance.OnTaskFinished(TaskType.Soda);
	}
}
