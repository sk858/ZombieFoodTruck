	using System;
using System.Collections;
using System.Collections.Generic;
	using System.IO.IsolatedStorage;
	using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Singleton in charge of progressing the day.
/// Spawns customers.
/// Incremenets day timer.
/// </summary>
public class LevelManager : MonoBehaviourSingleton<LevelManager> {

	/// <summary>
	/// Specifies a customer spawn action
	/// </summary>
	private class SpawnInfo
	{
		public Customer.CustomerType CustomerType;
		public int Amount;
		public float SpawnTime;
		public Food.FoodType[] FoodOrder = null;
		public int MarkerSize = 0;
	}

	private class SpawnInfoComparer : IComparer<SpawnInfo>
	{
		public int Compare(SpawnInfo x, SpawnInfo y)
		{
			if (x.SpawnTime < y.SpawnTime) return -1;
			return 1;
		}
	}
		
	public enum DayState { ACTIVE, INACTIVE, END };
	
	private System.Random m_random = new System.Random();
	
	private LineManager m_registerLine;
    
	private LineManager m_stoveLine;

	[SerializeField] private Worker m_worker;

	[SerializeField] private GameObject m_stove;

	[SerializeField] private GameObject m_cuttingBoard;

	[SerializeField] private GameObject m_frier;

	[SerializeField] private GameObject m_iceMachine;

	[SerializeField] private GameObject m_drinkMachine;

	[SerializeField] private GameObject m_iceCreamMachine;

	[SerializeField] private float m_timeBetweenCustomersInBatches;

	private Dictionary<Food.FoodType,GameObject> m_foodStationMap = new Dictionary<Food.FoodType,GameObject>();

	private DayState m_state = DayState.END;

	private float m_timeOfDay = 0f;
	
	private BaseLevel m_levelInfo;

	[SerializeField] private GameObject m_timer;

	[SerializeField] private GameObject m_startMessageText;

	[SerializeField] private Text m_dayNumberText;

	[SerializeField] private Text m_dayNumberSign;

	[SerializeField] private Text m_goalValueText;

	[SerializeField] private GameObject m_numberCustomersBar;

	// For day bar markers
	[SerializeField] private GameObject m_dayBarFillArea;
	[SerializeField] private int m_dayBarFillAreaWidth;
	[SerializeField] private GameObject m_dayBarMarker;
	[SerializeField] private CleaningManager[] m_cleaningManagers;
	private List<GameObject> m_dayBarMarkers = new List<GameObject>();

	
	public float TimeOfDay
	{
		get { return m_timeOfDay; }
	}

	// Holds customer spawn actions and executes them
	private List<SpawnInfo> m_spawnTimeline;
	private int m_spawnCounter;
	private SpawnInfoComparer m_spawnInfoComparer = new SpawnInfoComparer();

	public DayState State
	{
		get { return m_state; }
	}

	private void Awake()
	{
		// start in the end state so that nothing happens until the day starts
		m_state = DayState.END;
		// obtain reference to line managers
		m_registerLine = GameObject.FindWithTag("RegisterLine").GetComponent<LineManager>();
		m_stoveLine = GameObject.FindWithTag("StoveLine").GetComponent<LineManager>();
		if (m_registerLine == null || m_stoveLine == null)
		{
			Debug.Log("Cannot find reference to registerline or stoveline via tag.");
		}

		// generate foodStationMap
		m_foodStationMap = new Dictionary<Food.FoodType,GameObject> () {
			{ Food.FoodType.Burger, m_stove },
			{ Food.FoodType.Drink, m_drinkMachine },
			{ Food.FoodType.Fries, m_frier },
			{ Food.FoodType.IceCream, m_iceCreamMachine },
		};

		// add listeners for GameManagerUser
		GameManagerUser.Instance.OnGameStart.AddListener(StartNewDay);
		GameManagerUser.Instance.OnGamePrep.AddListener (PrepNewDay);
	}

	private void Start()
	{
		
	}

	private SpawnInfo GetSpawnInfo(BaseLevel.CustomerBatch batch)
	{
		SpawnInfo info = new SpawnInfo();
		info.CustomerType = batch.CustomerType;
		info.Amount = batch.Amount;
		info.SpawnTime = batch.StartTime + (float)m_random.NextDouble() * (batch.EndTime - batch.StartTime);
		info.FoodOrder = batch.FoodOrder;
		info.MarkerSize = batch.MarkerSize;
		return info;
	}

	private void ClearDayBarMarkers()
	{
		foreach (var dayBar in m_dayBarMarkers)
		{
			Destroy(dayBar);
		}
		m_dayBarMarkers.Clear();
	}

	private void PutMarkersOnDayBar(float dayLength)
	{
		foreach (var info in m_spawnTimeline)
		{
			if (info.MarkerSize > 0)
			{
				Vector3 pos = new Vector3((int) (info.SpawnTime/dayLength * m_dayBarFillAreaWidth), 0);
				Debug.Log(pos.x);
				Debug.Log(info.SpawnTime / dayLength);
				GameObject marker = Instantiate(m_dayBarMarker, pos, Quaternion.identity);
				marker.transform.parent = m_dayBarFillArea.transform;
				marker.GetComponent<RectTransform>().anchoredPosition = pos;

				marker.GetComponentInChildren<Text>().text = "" + info.MarkerSize;
			}
		}
	}
    
	// Prepares a new day. Gets all relevant info
	private void PrepNewDay(BaseLevel level, bool restart) 
	{
		Debug.Log ("Prepping new level: " + level.LevelID);


		// A/BTesting
		// Decide whether player is using time bar or number of customers left
//		if (PlayerStatistics.Instance.ABTestingValue == 0) {
//			// activate the time bar
//			m_timer.gameObject.SetActive (true);
//
//			// deactivate the number customers bar
//			// TODO: uncomment the line below once you've extablished a reference to the number customers bar
//			m_numberCustomersBar.gameObject.SetActive (false);
//		} else if (PlayerStatistics.Instance.ABTestingValue == 1) {
//			// activate the number customers bar
//			// TODO: uncomment the line below once you've extablished a reference to the number customers bar
//			m_numberCustomersBar.gameObject.SetActive(true);
//
//			// deactivate the time bar
//			m_timer.gameObject.SetActive(false);
//		}

		// just use the customer UI instead of clock
		m_timer.gameObject.SetActive (false);
	//	m_numberCustomersBar.gameObject.SetActive (true);
			

		m_levelInfo = level;

		// deactivate the time meter if necessary
		if (!level.TimeMeter) {
			Debug.Log ("deactivating time meter");
			m_timer.gameObject.SetActive (false);
		}

		// change MoneyManagerQuota
		MoneyManager.Instance.Quota = m_levelInfo.Quota;

		// set active the relevant food stations
		foreach (Food.FoodType f in m_levelInfo.FoodStations) 
		{
			m_foodStationMap [f].SetActive (true);
			if (f == Food.FoodType.Fries)
				m_cuttingBoard.SetActive (true);
			else if (f == Food.FoodType.Drink)
				m_iceMachine.SetActive (true);
		}

		// enable or disable eating depending on the level
		if (!EatingMechanicManager.Instance.IsMeatEnabled)
		{
			InputManager.Instance.LevelDisabledEating = m_levelInfo.DisableEating;
		}

		// generate spawn timeline
		m_spawnTimeline = new List<SpawnInfo>();

		// Add a random batch from each knockout group
		foreach (var knockoutGroup in level.KnockoutGroups)
		{
			BaseLevel.CustomerBatch batch = knockoutGroup[m_random.Next(knockoutGroup.Length)];
			m_spawnTimeline.Add(GetSpawnInfo(batch));
		}

		foreach (var batch in level.CustomerBatches)
		{
			m_spawnTimeline.Add(GetSpawnInfo(batch));
		}

		m_spawnTimeline.Sort(m_spawnInfoComparer);

		m_startMessageText.GetComponent<Text>().text = level.StartMessage;

		m_dayNumberText.text = "" + level.LevelID;

		m_dayNumberSign.text = "Day " + level.LevelID;

		m_goalValueText.text = "" + level.Quota;


	/*	if (PlayerStatistics.Instance.ABTestingValue == 0) {
			// if using time bar, clear day markers and put new ones on
			ClearDayBarMarkers ();
			PutMarkersOnDayBar (level.DayLength);
		}*/
		
		// Force eat on level 2
		if (level.LevelID == 2)
		{
			InputManager.Instance.ForceEat = true;
		}
		m_numberCustomersBar.GetComponent<NumberCustomers>().Init(GetTotalCustomersInDay());
	}

	public int GetTotalCustomersInDay()
	{
		int sum = 0;
		foreach (var spawnInfo in m_spawnTimeline)
		{
			sum += spawnInfo.Amount;
		}
		return sum;
	}

	public int GetCustomersLeft()
	{
		int sum = 0;
		for (int i = m_spawnCounter; i < m_spawnTimeline.Count; i++)
		{
			sum += m_spawnTimeline[i].Amount;
		}
		return sum;
	}

    // Begins a new day. Parameter 'level' is the level to start.
    private void StartNewDay(BaseLevel level, bool restart)
    {
        Debug.Log("STARTING NEW LEVEL");
	    
	    m_timeOfDay = 0f;
	    m_state = DayState.ACTIVE;
	    m_spawnCounter = 0;
    }

	private float GetDayProgress()
	{
		return m_timeOfDay / m_levelInfo.DayLength;
	}
	
	private IEnumerator SpawnCustomers(SpawnInfo info)
	{
		for (int i = 0; i < info.Amount; i++)
		{
			var order = new List<Food.FoodType>();
			if (info.FoodOrder != null)
			{
				for (int j = 0; j < info.FoodOrder.Length; j++)
				{
					order.Add(info.FoodOrder[j]);
				}
			}
			else
			{
				order = RandomOrder ();
			}
			CustomerManager.Instance.SpawnCustomer(info.CustomerType, order);
			yield return new WaitForSeconds(m_timeBetweenCustomersInBatches);
		}
	}

	// gives a random order. Can contain number of items up to m_levelInfo.MaxOrder. Can contain any food type
	// that is allowed in this level.
	// Never returns a list of three fries.
	private List<Food.FoodType> RandomOrder() {
		var order = new List<Food.FoodType> ();
		int orderSize = m_random.Next(1, m_levelInfo.MaxOrder + 1);
		for (int k = 0; k < orderSize; k++)
		{
			var food = m_levelInfo.FoodStations[m_random.Next(0, m_levelInfo.FoodStations.Count)];

			order.Add(food);
		}
		// reroll if a list of three fries was chosen
		if (orderSize == 3) {
			if (order [0] == Food.FoodType.Fries && order [1] == Food.FoodType.Fries && order [2] == Food.FoodType.Fries) {
				return RandomOrder ();
			}
		}
		order.Sort ();
		return order;
	}
	
	// Update is called once per frame
	void Update () {
		if (State == DayState.ACTIVE && m_levelInfo.TimeMeter) {
            m_timeOfDay += Time.deltaTime;

	        while (m_spawnCounter < m_spawnTimeline.Count)
	        {
		        SpawnInfo spawn = m_spawnTimeline[m_spawnCounter];
		        
		        if (spawn.SpawnTime > m_timeOfDay)
		        {
			        break;
		        }
		        m_spawnCounter++;
		        StartCoroutine(SpawnCustomers(spawn));
		        m_numberCustomersBar.GetComponent<NumberCustomers>().UpdateText(GetCustomersLeft());
	        }

	        float prog = GetDayProgress();
            
            // if day is over then end the day
            if (prog >= 1) 
            {
                prog = 1;
	            m_state = DayState.INACTIVE;
            }
			m_timer.GetComponentInChildren<Timer>().SetTime(prog);
        } 
		if (State == DayState.INACTIVE)
        {
            // only activate the day's end when all customers have left and after player is no longer working or holding a body
			if (m_registerLine.Length == 0 && m_stoveLine.Length == 0 && !m_worker.HoldingBody)
            {
				if (!CleaningManagersHaveTasks()) {
					// alert the game manager that the day has ended
					GameManagerUser.Instance.OnDayEnd ();
					// set the day state to END since the day is over
					m_state = DayState.END;
				} else if (m_worker.CurrentTaskCollider.CurrentCollidingTask == null 
					|| m_worker.CurrentTaskCollider.CurrentCollidingTask.Type != TaskManager.TaskType.Cleaning) {
					TutorialManager.Instance.OnBodyLeftOver ();
				}
            }
        }
    }

	private bool CleaningManagersHaveTasks()
	{
		foreach (var cm in m_cleaningManagers)
		{
			if (cm.gameObject.activeSelf && cm.IsThereATask)
			{
				return true;
			}
		}
		return false;
	}

	public void FinishDay()
	{
		Debug.Log ("day finished");
		m_state = DayState.INACTIVE;
	}
}
