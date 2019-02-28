 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorkerManager : MonoBehaviourSingleton<WorkerManager>
{
	public class WrongFoodEvent : UnityEvent<Food.FoodType> {};

	public class WrongPotatoesEvent : UnityEvent {};

	private bool m_workerSelectionModeEnabled = false;

	private int m_workerIdCounter;

	private Dictionary<int, Worker> m_workersByID = new Dictionary<int, Worker>();

	private List<Worker> m_workerList = new List<Worker>();

	private int m_selectedWorkerIndex = 0;

	private Worker m_selectedWorker;
	
	private UnityAction<int> m_callOnWorkerClicked;

	private WrongFoodEvent m_workerWrongFoodEvent = new WrongFoodEvent();

	private WrongPotatoesEvent m_workerWrongPotatoesEvent = new WrongPotatoesEvent ();
	
	[SerializeField]
	private ServeFoodManager m_serveFoodManager;
	[SerializeField]
	private FryerManager m_fryerManager;

	public int NumberWorkers
	{
		get { return m_workerList.Count; }
	}

	public bool WorkerSelectionModeEnabled
	{
		get
		{
			return m_workerSelectionModeEnabled;
		}
		set
		{
			m_workerSelectionModeEnabled = value;
		}
	}

	public Worker SelectedWorker
	{
		get
		{
			return m_selectedWorker;
		}
	}

	public WrongFoodEvent WorkerWrongFoodEvent
	{
		get { return m_workerWrongFoodEvent; }
	}

	public WrongPotatoesEvent WorkerWrongPotatoesEvent
	{
		get { return m_workerWrongPotatoesEvent; }
	}

	// Use this for initialization
	void Awake ()
	{
		//TODO: FIX THIS SO WORKERS ID'S ARE PERSISTENT
		var allWorkers = FindObjectsOfType<Worker>();
		for (int i = 0; i < allWorkers.Length; i++)
		{
			m_workersByID.Add(m_workerIdCounter, allWorkers[i]);
			m_workerList.Add(allWorkers[i]);
			allWorkers[i].Init(m_workerIdCounter);
			m_workerIdCounter++;
		}
		m_selectedWorker = m_workerList[m_selectedWorkerIndex];
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			m_selectedWorkerIndex = (m_selectedWorkerIndex + 1) % m_workerList.Count;
			m_selectedWorker = m_workerList[m_selectedWorkerIndex];
		}
	}
		
	public void WorkerClicked(int workerId)
	{
		if (!m_workerSelectionModeEnabled)
		{
			return;
		}
		m_callOnWorkerClicked.Invoke(workerId);
	}
	
	public void BuffWorker(TaskManager.TaskType buffType, int workerId, float buffValue)
	{
		Worker worker = m_workersByID[workerId];
		worker.SetSpeed(buffType, buffValue);
	}

	public Worker GetWorker(int workerId)
	{
		return m_workersByID[workerId];
	}
	
	public bool EnableWorkerSelection(UnityAction<int> action)
	{
		m_workerSelectionModeEnabled = true;
		m_callOnWorkerClicked = action;
		for (int i = 0; i < m_workerList.Count; i++)
		{
			m_workerList[i].SetSelectedUI(true);
		}
		return true;
	}
	
	public void DisableWorkerSelection()
	{
		m_workerSelectionModeEnabled = false;
		for (int i = 0; i < m_workerList.Count; i++)
		{
			m_workerList[i].SetSelectedUI(false);
		}
	}

	public Worker.WorkerState GetCurrentWorkerTask()
	{
		return SelectedWorker.State;
	}

	public bool StartWorking()
	{
		m_selectedWorker.SetVelocity(Vector2.zero);
		return m_selectedWorker.StartWorking();
	}

	public bool StopWorking()
	{
		return m_selectedWorker.StopWorking();
	}

	public void MoveWorker(Vector2 movementVector)
	{
		m_selectedWorker.SetVelocity(movementVector);
	}

	public void DockEating()
	{
		m_selectedWorker.DockEating();
	}

	public void OnWorkerGetFood(Food food)
	{
		if (!m_serveFoodManager.NeedsFood(food.Type) || m_serveFoodManager.CustomerIdForTask == -1)
		{
			WorkerWrongFoodEvent.Invoke(food.Type);
			Debug.Log("WRONGFOOD");
		}
	}

	public void OnNewOrder()
	{
		if (m_selectedWorker.HoldingFood)
		{
			Food.FoodType type = m_selectedWorker.ObjectHeld.GetComponent<Food>().Type;
			if (!m_serveFoodManager.NeedsFood(type) || m_serveFoodManager.CustomerIdForTask == -1)
			{
				WorkerWrongFoodEvent.Invoke(type);
				Debug.Log("WRONGFOOD");
			}
		}
	}

	public void OnGetPotatoes()
	{
		if (!m_fryerManager.AbleToReceivePotatoes) {
			Debug.Log ("WRONGPOTATOES");
			WorkerWrongPotatoesEvent.Invoke ();
		}
	}

	public void OnEnergyEmpty()
	{
		//StartCoroutine(HungerCoroutine());
	}

	public bool CanMove()
	{
		return m_selectedWorker.CanMove;
	}

	public void SetHungry(bool isHungry)
	{
		m_selectedWorker.SetHungry(isHungry);
	}

	public void WorkerMovementChange(float NewMovSpeed)
	{
		m_selectedWorker.SetMovementSpeed(NewMovSpeed);		
	}
}
