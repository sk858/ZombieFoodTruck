using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryerManager : TaskManager {

	
	[SerializeField] 
	private GameObject m_fryObject;
	
	[SerializeField]
	private Vector3 m_workerFoodOffset;

	[SerializeField]
	private GameObject m_foodReadyUI;

	private bool m_hasFries;

	private bool m_halfway;

	private bool m_waitingForAnim;
	
	private List<Upgrade> m_appliedStationsUpgrades = new List<Upgrade>();

	private Animator m_animator;

	private AudioSource m_audioSource;

	[SerializeField] private CuttingBoardManager m_cuttingBoardManager;

	public bool AbleToReceivePotatoes { 
		get { return !m_hasFries && !m_waitingForAnim && !m_isThereATask; }
	}

	protected override void Awake()
	{
		base.Awake();
		m_animator = GetComponent<Animator>();
		m_audioSource = GetComponent<AudioSource>();
		//m_cuttingBoardManager = Resources.FindObjectsOfTypeAll<CuttingBoardManager>()[0];
	}
		
	// Update is called once per frame
	protected override void Update () 
	{
		if (m_isThereATask)
		{
			if (!m_halfway && m_taskProgress >= m_progressForCompletion / 2f)
			{
				m_animator.SetTrigger("FriesHalfway");
			}
			
			m_taskProgress += Time.deltaTime;
			if (m_taskProgress >= m_progressForCompletion)
			{
				m_animator.SetTrigger("FriesDone");
				m_isThereATask = false;
				m_audioSource.Stop();
				m_waitingForAnim = true;
			}
			UpdateProgressBar();
		}
	}
	
	private void GiveFood(Worker worker)
	{
		GameObject food = Instantiate(m_fryObject, worker.transform);
		food.transform.localPosition = m_workerFoodOffset;
		worker.AddFood(food);
		m_hasFries = false;
		m_taskProgress = 0f;
		UpdateProgressBar();
		TutorialManager.Instance.OnTaskStarted (TaskType.Tray);
		TruckLogger.Instance.LogFoodGrabbed ();
		m_foodReadyUI.SetActive(false);
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

	private void PutFoodBack(Worker worker)
	{
		GameObject objectHeld = worker.ObjectHeld;
		worker.RemoveFood();
		Destroy(objectHeld);
		m_hasFries = true;
		m_foodReadyUI.SetActive(true);
		worker.QuickTaskStart();
	}
	
	public override bool OnWorkingChanged(bool isWorking, Worker worker)
	{
		if (m_isThereATask || m_waitingForAnim)
		{
			return false;
		}
		if (isWorking && worker.HoldingFood && worker.ObjectHeld.GetComponent<Food>().Type == Food.FoodType.Fries)
		{
			PutFoodBack(worker);
			return false;
		}
		if (!m_hasFries && !worker.HoldingPotatoes)
		{
			return false;
		}
		if (m_hasFries && worker.HoldingSomething)
		{
			return false;
		}
		if (isWorking)
		{
			if (m_hasFries)
			{
				GiveFood(worker);
				worker.QuickTaskStart();
			}
			else if (worker.HoldingPotatoes)
			{
				m_audioSource.Play();
				m_isThereATask = true;
				worker.RemovePotatoes();
				TutorialManager.Instance.OnTaskStarted (TaskType.Fryer);
				TruckLogger.Instance.LogStartFryer ();
				m_animator.SetTrigger("FriesStarted");
				worker.QuickTaskStart();
				m_cuttingBoardManager.OnPotatoesUsed();
			}
		}
		return false;
	}

	public override bool CanDock(Worker worker)
	{
		if (EatingMechanicManager.Instance.IsHungerEnabled && EatingMechanicManager.Instance.IsEnergyEmpty)
		{
			return false;
		}
		if (m_isThereATask || m_waitingForAnim)
		{
			return false;
		}
		if (worker.HoldingFood && worker.ObjectHeld.GetComponent<Food>().Type == Food.FoodType.Fries)
		{
			return true;
		}
		if (m_hasFries && worker.HoldingSomething)
		{
			return false;
		}
		if (!m_hasFries && !worker.HoldingPotatoes)
		{
			return false;
		}		
		return true;
	}

	public void OnAnimFinished()
	{
		m_hasFries = true;
		m_foodReadyUI.SetActive(true);
		TutorialManager.Instance.OnTaskFinished (TaskType.Fryer);
		m_waitingForAnim = false;
	}
}
