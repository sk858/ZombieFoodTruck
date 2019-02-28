using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StoveTask : TaskManager
{

	[SerializeField]
	private GameObject m_foodPrefab;

	[SerializeField]
	private Vector3 m_workerFoodOffset;

	[SerializeField] 
	private GameObject m_pointingHand;

	[SerializeField] 
	private GameObject m_foodReadyUI;

	[SerializeField] 
	private Text m_meatDisplayText;

	[SerializeField] 
	private int m_meatLimit;

	[SerializeField] 
	private int m_meatIncCount;

	[SerializeField]
	private int m_meatCount;

	private bool m_isMiniGameEnabled;

	private bool m_taskStarted;

	private bool m_foodReady;

	private bool m_halfTrigger;

	private bool m_waitingForAnim;

	private AudioSource m_audioSource;
	
	private Animator m_animator;

	private List<Upgrade> m_appliedStationsUpgrades = new List<Upgrade>();

	protected override void Awake()
	{
		base.Awake();
		m_meatDisplayText.gameObject.SetActive(EatingMechanicManager.Instance.IsMeatEnabled);
		UpdateMeatText();
		m_animator = GetComponent<Animator>();
		m_audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		if (IsDoingTask)
		{
			m_taskProgress += m_workSpeed * Time.deltaTime;
			UpdateProgressBar();
			if (!m_halfTrigger && m_taskProgress >= m_progressForCompletion/2f)
			{
				m_halfTrigger = true;
				m_animator.SetTrigger("HalfDone");
			}
			if (m_taskProgress >= m_progressForCompletion)
			{
				m_taskProgress = 0f;
				UpdateProgressBar();
				m_animator.SetTrigger("FoodDone");
				m_isDoingTask = false;
				m_waitingForAnim = true;
				TutorialManager.Instance.OnTaskFinished (TaskType.Stove);
				m_audioSource.Stop();
			}
		}
	}
	
	public void OnBurgerFinishAnim()
	{
		m_foodReady = true;
		m_foodReadyUI.SetActive(true);
		m_waitingForAnim = false;
	}

	private void GiveFoodToWorker(Worker worker)
	{
		GameObject food = Instantiate(m_foodPrefab, m_workerDoingTask.transform);
		food.transform.localPosition = m_workerFoodOffset;
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

	public void RefillMeat()
	{
		m_meatCount = Mathf.Clamp(m_meatCount + m_meatIncCount, 0, m_meatLimit);
		UpdateMeatText();
	}

	private void UpdateMeatText()
	{
		m_meatDisplayText.text = m_meatCount + "/" + m_meatLimit;
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
	
	public override bool OnWorkingChanged(bool isWorking, Worker worker)
	{	
		if (isWorking)
		{
			if (worker.HoldingFood && worker.ObjectHeld.GetComponent<Food>().Type == Food.FoodType.Burger)
			{
				PutFoodBack(worker);
				return false;
			}
			if (worker.HoldingSomething || m_waitingForAnim)
			{
				return false;
			}	
			if (!EatingMechanicManager.Instance.IsMeatEnabled)
			{
				if (m_foodReady)
				{
					GiveFoodToWorker(worker);
					worker.QuickTaskStart();
					return false;
				}
				else if (!m_isDoingTask)
				{
					TutorialManager.Instance.OnTaskStarted(TaskType.Stove);
					TruckLogger.Instance.LogStoveStarted();
					m_isDoingTask = true;
					m_animator.SetTrigger("CookingStarted");
					m_halfTrigger = false;
					m_audioSource.Play();
					worker.QuickTaskStart();
				}
				else
				{
					return false;
				}
			}
		}
		m_workerDoingTask = worker;
		m_workSpeed = worker.GetSpeed(m_taskType);
		return false;
	}

	public override bool CanDock(Worker worker)
	{
		if (EatingMechanicManager.Instance.IsHungerEnabled && EatingMechanicManager.Instance.IsEnergyEmpty)
		{
			return false;
		}
		if (m_isDoingTask || m_waitingForAnim)
		{
			return false;
		}
		if (worker.HoldingFood && worker.ObjectHeld.GetComponent<Food>().Type == Food.FoodType.Burger)
		{
			return true;
		}
		if (!worker.HoldingSomething)
		{
			return true;
		}
		return false;
	}
}
