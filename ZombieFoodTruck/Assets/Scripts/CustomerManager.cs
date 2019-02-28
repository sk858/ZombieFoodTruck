using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviourSingleton<CustomerManager>
{

	public class EatingEvent : UnityEvent {};

	public EatingEvent OnCustomerEaten = new EatingEvent ();

	//Customer prefabs
	[SerializeField] private GameObject m_normalCustomer;

	[SerializeField] private GameObject m_smellyCustomer;

	[SerializeField] private GameObject m_richCustomer;

	[SerializeField] private GameObject m_buffCustomer;

	[SerializeField] private GameObject m_policeCustomer;

	[SerializeField] private GameObject m_zombieHunter;

	[SerializeField] private GameObject m_deadBody;

	[SerializeField] private GameObject m_deadBodyWorker;

	[SerializeField] private float m_foodIncreaseFactor;

	private bool m_pauseRegisterLine = false;

	private bool m_pauseStoveLine = false;

	private Dictionary<Customer.CustomerType, GameObject> m_customerMap;

	//Maps customer id's to Customers
	private Dictionary<int, Customer> m_allCustomers = new Dictionary<int, Customer>();

	[SerializeField]
	private Vector3 m_customerSpawnLocation;

	//Location customers go to after leaving or getting served
	[SerializeField]
	private Vector3 m_exitDestination;

	//TaskManagers for stove, register, cleaning station

	[SerializeField]
	private RegisterManager m_registerTask;

	[SerializeField]
	private CleaningManager[] m_cleanUpTasks;

	[SerializeField]
	private FoodMinigameTask m_stoveTask;

	[SerializeField]
	private ServeFoodManager m_serveFoodManager;

	//Time a customer will wait for their order
	[SerializeField]
	private float m_orderPatience;

	//Time a customer will wait for their food
	[SerializeField]
	private float m_foodPatience;

	[SerializeField]
	private LineManager m_registerLine;

	[SerializeField]
	private LineManager m_stoveLine;

	[SerializeField]
	private int m_moneyPerBurger;

	[SerializeField]
	private int m_moneyPerFries;

	[SerializeField]
	private int m_moneyPerDrink;

	[SerializeField]
	private int m_moneyPerIceCream;

	[SerializeField]
	private int m_moneyPerTicket;

	[SerializeField]
	private float m_maxTip;

	[SerializeField]
	private float m_chanceTip;

	[SerializeField]
	private int m_extraTipAmount;

	[SerializeField]
	private GameObject m_customerSelectionUI;

	private List<Upgrade> m_appliedUpgrades = new List<Upgrade>();

	private int m_idCounter = 1;

	private int m_indexForRegister;

	private int m_customerIdForRegister;

	private bool m_isInited;

	private bool m_isBeingFined;

	//Added to the patience of spawned customers
	private float m_patienceModifier;

	//Determines how quickly customer patience depletes.
	private float m_patienceDepletionMod = 1;

	private bool m_customerSelectMode;

	//NOT FINAL
	//Color of customers' patience bars
	private Color m_patienceBarColor = Color.green;

	private float m_spawnTimer;

	[SerializeField]
	private bool m_isIncognito;

	public bool CustomerSelectMode
	{
		get { return m_customerSelectMode; }
	}

	public bool CanLeaveRegisterWithAllArrows
	{
		get { return !m_registerLine.IsCustomersWaiting() && m_registerLine.Length <= 1; }

	}

	public CleaningManager[] CleanUpTasks
	{
		get { return m_cleanUpTasks; }
	}

	public bool IsBeingFined
	{
		get { return m_isBeingFined; }
	}

	public bool IsIncognito
	{
		get { return m_isIncognito; }
		set { m_isIncognito = value; }
	}

	// Initialize map of customer type to prefab
	void Awake()
	{
		m_customerMap = new Dictionary<Customer.CustomerType, GameObject>()
		{
			{Customer.CustomerType.Normal, m_normalCustomer},
			{Customer.CustomerType.Smelly, m_smellyCustomer},
			{Customer.CustomerType.Rich, m_richCustomer},
			{Customer.CustomerType.Buff, m_buffCustomer},
			{Customer.CustomerType.Police, m_policeCustomer},
			{Customer.CustomerType.ZombieHunter, m_zombieHunter}
		};
	}

	private void Start()
	{
		Init();

	}

	public void Init()
	{
		m_isInited = true;
		//SpawnCustomer();
		m_spawnTimer = 0f;
	}

	[SerializeField]
	private GameObject m_customerPrefab;

	void Update ()
	{
		if (m_isInited)
		{
			List<Customer> customersLeaving = new List<Customer>();
            //Loop for updating patience
            foreach (int id in m_allCustomers.Keys)
			{
				Customer customer = m_allCustomers[id];

				//If the customer is walking or has patience frozen, do not update patience
				if (customer.State == Customer.CustomerState.Walking ||
				    customer.State == Customer.CustomerState.Fleeing ||
					customer.State == Customer.CustomerState.Leaving ||
				    customer.State == Customer.CustomerState.Undef   ||
				    customer.State == Customer.CustomerState.Dead    ||
				    customer.IsBeingHelped)
				{
					continue;
				}

				if ((customer.State == Customer.CustomerState.GettingFoodServed ||
				    customer.State == Customer.CustomerState.InLineForFood) && m_pauseStoveLine)
				{
					continue;
				}
				
				if ((customer.State == Customer.CustomerState.GettingOrderTaken ||
				     customer.State == Customer.CustomerState.InLineForOrder) && m_pauseRegisterLine)
				{
					continue;
				}
				//If the police customer is giving a ticket, make them leave
				if (customer.State == Customer.CustomerState.GivingTicket)
				{
					customer.GetComponentInChildren<Animator>().SetTrigger("Fine");
					m_isBeingFined = true;
				}

				//Decrease customer's patience at rate dependent upon smell state
				customer.Patience -= m_patienceDepletionMod * Time.deltaTime;

				//If customer has not run out of patience, do nothing
				if (customer.Patience > 0 || customer.IsAngry)
				{
					continue;
				}
				customersLeaving.Add(customer);
				if (m_stoveLine.HasCustomer(customer.CustomerId))
				{
					m_pauseStoveLine = true;
				}
				if (m_registerLine.HasCustomer(customer.CustomerId))
				{
					m_pauseRegisterLine = true;
				}
			}
			for (int i = 0; i < customersLeaving.Count; i++)
			{
				Customer customer = customersLeaving[i];
				//If customer is waiting for order, remove from line and
				//indicate work station the customer has left
				if (customer.State == Customer.CustomerState.GettingOrderTaken)
				{
					m_registerTask.CustomerLeft();
				}
				else if (customer.State == Customer.CustomerState.GettingFoodServed)
				{
					m_serveFoodManager.CustomerLeft();
				}
				customer.OnPatienceDepleted();
			}
		}
	}

	public void OnFineFinished(Customer customer)
	{
		MoneyManager.Instance.RemoveMoney(m_moneyPerTicket);
		MoneyManager.Instance.ShowMoneyChange(-m_moneyPerTicket, customer.transform, Color.clear);
		customer.State = Customer.CustomerState.Leaving;
		customer.SetDestination(m_exitDestination, Customer.CustomerState.Undef);
		m_isBeingFined = false;
	}

	public void ChangeSelectMode(TaskManager.TaskType taskType)
	{
		switch (taskType)
		{
				case TaskManager.TaskType.Stove:
					break;
				case TaskManager.TaskType.Register:

					break;
				default:
					return;
		}
	}

	public void SpawnCustomer(Customer.CustomerType ctype, List<Food.FoodType> order)
	{
		Debug.Log ("spawning customer");
		GameObject go = Instantiate(m_customerMap[ctype], m_customerSpawnLocation, Quaternion.identity);
		Customer customer = go.GetComponent<Customer>();

		//Get spot in line for customer to walk to
		Vector3 destination = m_registerLine.AddCustomer(m_idCounter);

		//Do not decrease patience while customer is walking to line
		customer.FreezePatience = true;

		Customer.CustomerState stateOnArrival = Customer.CustomerState.InLineForOrder;
		if (m_registerLine.NextInLine() == m_idCounter)
		{
			stateOnArrival = Customer.CustomerState.GettingOrderTaken;
		}

		//Init customer and add to dictionary
		customer.Init(m_idCounter, Customer.CustomerState.Walking, destination, stateOnArrival, m_orderPatience + m_patienceModifier, m_maxTip, order);
		m_allCustomers.Add(m_idCounter, customer);
		m_idCounter++;

		//Set customer's patience bar color
		customer.SetPatienceBarColor(m_patienceBarColor);
	}

	public void RemoveCustomer(int customerId)
	{
		Customer c = m_allCustomers[customerId];
		m_allCustomers.Remove(customerId);
		Destroy(c.gameObject);
	}

	/*
	* Called when customer is clicked in eat mode. Eats the customer. Makes truck smelly. All onscreen customers flee.
	* Adds to tally of total people eaten today.
	*/
	public void EatCustomer(int customerId, Customer.CustomerState stateOfEatenCustomer)
	{
		// invoke the customer eaten event
		OnCustomerEaten.Invoke ();

		int numCustomersFleeing = 0;
		foreach (int id in m_allCustomers.Keys)
		{
			Customer c = m_allCustomers[id];
			//If customerId matches that of eaten customer
			//set state to dead, otherwise set customer to flee

			//Remove all customers from their respective lines
			if (c.CustomerId != customerId)
			{
				if (stateOfEatenCustomer == Customer.CustomerState.GettingFoodServed && !m_isIncognito)
				{
					if (m_stoveLine.HasCustomer(c.CustomerId))
					{
						m_stoveLine.RemoveCustomer(c.CustomerId);
						if (c.Type == Customer.CustomerType.Police)
						{
							c.State = Customer.CustomerState.Walking;
							c.SetDestination(m_stoveLine.GetFrontPos(), Customer.CustomerState.GivingTicket);
						}
						else
						{
							numCustomersFleeing++;
							c.State = Customer.CustomerState.Fleeing;
							c.SetDestination(m_exitDestination, Customer.CustomerState.Undef);
						}
					}
				}
				else if (stateOfEatenCustomer == Customer.CustomerState.GettingOrderTaken && !m_isIncognito)
				{
					if (m_registerLine.HasCustomer(c.CustomerId))
					{
						m_registerLine.RemoveCustomer(c.CustomerId);
						if (c.Type == Customer.CustomerType.Police)
						{
							c.State = Customer.CustomerState.Walking;
							c.SetDestination(m_registerLine.GetFrontPos(), Customer.CustomerState.GivingTicket);
						}
						else
						{
							numCustomersFleeing++;
							c.State = Customer.CustomerState.Fleeing;
							c.SetDestination(m_exitDestination, Customer.CustomerState.Undef);
						}
					}
				}
			}
			/*
			*/
			/*		if (c.State == Customer.CustomerState.GettingOrderTaken)
					{
						m_registerLine.RemoveCustomer(id);
						m_registerTask.CustomerLeft();
					}
					else if (c.State == Customer.CustomerState.GettingFoodServed)
					{
						m_stoveLine.RemoveCustomer(id);
						m_serveFoodManager.CustomerLeft();
					}
					else if (c.State == Customer.CustomerState.InLineForFood)
					{
						m_stoveLine.RemoveCustomer(id);
					}
					else if (c.State == Customer.CustomerState.InLineForOrder)
					{
						m_registerLine.RemoveCustomer(id);

					}
					else if (c.State == Customer.CustomerState.Walking)
					{
						if (m_registerLine.HasCustomer(id))
						{
							m_registerLine.RemoveCustomer(id);
						}
						else if(m_stoveLine.HasCustomer(id))
						{
							m_stoveLine.RemoveCustomer(id);
						}
					}*/

			if (id == customerId)
			{
				c.State = Customer.CustomerState.Dead;
				m_deadBody.GetComponent<SpriteRenderer>().sprite = c.DeadSprite;
				m_deadBodyWorker.GetComponent<SpriteRenderer>().sprite = c.DeadSprite;
				if (stateOfEatenCustomer == Customer.CustomerState.GettingFoodServed)
				{
					m_stoveLine.RemoveCustomer(id);
					m_serveFoodManager.CustomerEaten();
				}
				else if (stateOfEatenCustomer == Customer.CustomerState.GettingOrderTaken)
				{
					m_registerLine.RemoveCustomer(id);
					m_registerTask.CustomerLeft();
				}
			}

			for (int i = 0; i < numCustomersFleeing; i++) {
				TutorialManager.Instance.OnCustomerLeft ();
			}

		}
		if (m_isIncognito)
		{
			if (m_stoveLine.Length >= 1)
			{
				int nextCustomer = m_stoveLine.NextInLine();
				m_serveFoodManager.ChangeOrder(nextCustomer, m_allCustomers[nextCustomer].Order);
			}
		}

		//Remove dead customer
		RemoveCustomer(customerId);

	}

	public void MoveCustomer(int customerId, Vector3 destination, Customer.CustomerState stateOnArrival, bool freezePatience = true, UnityAction callOnArrival = null)
	{
		//SetVelocity customer to new location
		Customer customer = m_allCustomers[customerId];
		customer.State = Customer.CustomerState.Walking;
		customer.FreezePatience = freezePatience;
		customer.SetDestination(destination, stateOnArrival);
		if (stateOnArrival == Customer.CustomerState.GettingFoodServed)
		{
			m_serveFoodManager.ChangeOrder(customerId, customer.Order);
		}
	}

	/*
	 * Called when customer with id = customerId has their order taken
	 */
	public void OnCustomerOrderTaken(int customerId)
	{
		Customer customer = m_allCustomers[customerId];
		//Reset customer's patience and freeze patience now that customer is walking
		customer.State = Customer.CustomerState.Walking;
		customer.FreezePatience = true;
		customer.AnimateNewMaxPatience(m_foodPatience + m_patienceModifier);
		customer.SetTaskProgress(0f);
		customer.IsBeingHelped = false;
		//Remove customer from register line and add to stove line
		m_registerLine.RemoveCustomer(customerId);
		Vector3 destination = m_stoveLine.AddCustomer(customerId);

		Customer.CustomerState stateOnArrival = Customer.CustomerState.InLineForFood;
		if (m_stoveLine.NextInLine() == customer.CustomerId && InputManager.Instance.ArrowMode)
		{
			stateOnArrival = Customer.CustomerState.GettingFoodServed;
			m_serveFoodManager.ChangeOrder(customerId, customer.Order);
		}
		customer.SetDestination(destination, stateOnArrival);
		customer.SetOrderBubble(true);
		Debug.Log("customer order taken");
	}

	/*
 	 * Called when customer with id = customerId has their order served
 	 */
	public void OnCustomerOrderServed(int customerId)
	{
		//Send customer away, as they have been fed
		Customer customer = m_allCustomers[customerId];
		customer.State = Customer.CustomerState.Leaving;
		customer.FreezePatience = true;
		customer.SetTaskProgress(0f);
		customer.IsBeingHelped = false;
		m_stoveLine.RemoveCustomer(customerId);
		m_serveFoodManager.CustomerLeft();
		if (m_stoveLine.Length >= 1)
		{
			int nextCustomer = m_stoveLine.NextInLine();
			m_serveFoodManager.ChangeOrder(nextCustomer, m_allCustomers[nextCustomer].Order);
		}
		customer.SetDestination(m_exitDestination);
		Debug.Log("order served");
		AddMoneyForOrder(customer);
		customer.OnOrderServed();
		SoundManager.Instance.OnOrderServed();
	}

	public void AddMoneyForOrder(Customer customer)
	{
		int sum = 0;
		int totalMoney = 0;
		for (int i = 0; i < customer.Order.Count; i++)
		{
			switch (customer.Order[i])
			{
					case Food.FoodType.Burger:
						sum += m_moneyPerBurger;
						break;
					case Food.FoodType.Drink:
						sum += m_moneyPerDrink;
						break;
					case Food.FoodType.Fries:
						sum += m_moneyPerFries;
						break;
					case Food.FoodType.IceCream:
						sum += m_moneyPerIceCream;
						break;
			}
		}
		int tip = customer.GetTipAmount();
		Color tipColor = customer.GetPatienceColor ();
		if (PlayerStatistics.Instance.PurchasedOneTime.Contains(OneTimePurchaseUpgrade.Instance.NewTipChanceId))
		{
			int extra = ChanceToEarnExtraCash();
			tip +=  extra;
			totalMoney = sum + tip;
			Debug.Log("Extra cash earned");
			Debug.Log(extra);
		}
		else
		{
			totalMoney = sum + tip;
		}
		MoneyManager.Instance.EarnMoney(totalMoney);
		MoneyManager.Instance.ShowMoneyChange(sum, customer.transform, tipColor, tip);
	}

	/*
	 * Sets task for register to customerId's order
	 */
	public void SetRegisterTask(int customerId)
	{
		Debug.Log("register task set");
		m_registerTask.ChangeOrder(customerId);
	}

	public void SetFoodTask(int customerId, List<Food.FoodType> order)
	{
		m_serveFoodManager.ChangeOrder(customerId, order);
	}

	/*
	* Sets task for cleanup station to customerId's dead body
	*/
	public void SetCleanUpTask()
	{
		Debug.Log("cleanup task set");
	}

	public void BoostPatience(float patienceBoost)
	{
		m_patienceModifier = patienceBoost;
		if (patienceBoost == 0)
		{
			return;
		}
		foreach (int key in m_allCustomers.Keys)
		{
			Customer c = m_allCustomers[key];
			c.Patience += patienceBoost;
			c.MaxPatience += patienceBoost;
		}
	}

	public void AddUpgrade(Upgrade upgrade)
	{
		m_appliedUpgrades.Add(upgrade);
	}

	public bool IsUpgradeApplied(Upgrade upgrade)
	{
		return m_appliedUpgrades.Contains(upgrade);
	}

	public void IncreaseMoneyPerOrder(int val)
	{
		m_moneyPerBurger += val;
	}

	// Called by SmellyManager whenever the patience depletion multiplier is changed.
	public void SetPatienceDepletionMod(float value)
	{
		m_patienceDepletionMod = value;
	}

	// Sets the patience bars of all customers to given color.
	public void SetCustomerPatienceColor(Color color)
	{
		m_patienceBarColor = color;
		foreach (var customer in m_allCustomers.Values)
		{
			customer.SetPatienceBarColor(color);
		}
	}

	public Customer GetCustomer(int customerId)
	{
		return m_allCustomers[customerId];
	}

	public Customer GetCustomerForRegister()
	{
		int id = m_registerLine.NextInLine();
		if (id == -1)
		{
			return null;
		}
		return m_allCustomers[id];
	}


	public void EatCustomer(int customerId)
	{
		Customer c = m_allCustomers[customerId];
//		if (c.Type == Customer.CustomerType.Police)
//		{
//			// TODO Show no eat animation
//			return;
//		}

		EatCustomer(customerId, c.State);
		WorkerManager.Instance.SelectedWorker.ChangeBodyHeld(true, c.Type);
	}

	public void IncreasePatienceOnFood()
	{
		Customer c = m_allCustomers[m_serveFoodManager.CustomerIdForTask];
		c.Patience += c.MaxPatience * 0.25f;
	}

	public void OnAngryAnimationDone(Customer customer)
	{
		int id = customer.CustomerId;
		//If customer is waiting for order, remove from line and
		//indicate work station the customer has left
			TutorialManager.Instance.OnCustomerLeft ();
			if (m_registerLine.HasCustomer(customer.CustomerId))
			{
				m_registerLine.RemoveCustomer(id);
				m_pauseRegisterLine = false;
			}
			else if (m_stoveLine.HasCustomer(customer.CustomerId))
			{
				m_stoveLine.RemoveCustomer(id);
				m_pauseStoveLine = false;
			}

		//Send customer to exit destination and change state to leaving
		customer.SetDestination(m_exitDestination);
		customer.State = Customer.CustomerState.Leaving;
	}

	public void SetCleaningProgressBar(int tier)
	{
		foreach (var task in m_cleanUpTasks)
		{
			task.SetProgressBarTier(tier);
		}
	}
	public int ChanceToEarnExtraCash()
	{
		float random = Random.Range(0.0f, 1.0f);
		if (random > m_chanceTip)
		{
			return m_extraTipAmount;
		}
		else
		{
			return 0;
		}
	}

	public int GetExtraTipAmount()
	{
		return m_extraTipAmount;
	}

	public float GetChanceTip()
	{
		return m_chanceTip*100;
	}
}
