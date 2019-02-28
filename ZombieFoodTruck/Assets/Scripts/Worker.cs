using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
	public enum WorkerState
	{
		Idle,
		Walking,
		Cleaning,
		Cooking,
		TakingOrders,
		Serving
	};
	
	//Unique ID of worker
	[SerializeField]
	private int m_workerId;

	//Color when worker is highlighted for selection
	[SerializeField] 
	private Color m_highlightedColor;

	//Original color of the worker on game start
	private Color m_originalColor;

	private Vector3 m_destination;
	
	protected bool m_isWorking;

	private bool m_holdingFood;

	private bool m_holdingBody;

	private bool m_holdingPotatoes;

	private bool m_holdingMeat;

	private bool m_holdingIce;

	[SerializeField] private RuntimeAnimatorController m_chefAnimator;

	[SerializeField]
	private ButtonSpawn m_spaceBarUI;

	[SerializeField] 
	private Vector3 m_customerEatOffset;
	
	[SerializeField]
	protected float m_cookingSpeed;

	[SerializeField]
	protected float m_registerSpeed;

	[SerializeField]
	protected float m_cleaningSpeed;

	[SerializeField]
	protected float m_baseMovementSpeed;

	protected float m_movementSpeed;
	
	[SerializeField] 
	protected float m_carryingBodySpeed;

	[SerializeField]
	protected WorkerState m_state;

	[SerializeField] 
	private GameObject m_bodyToHold;

	[SerializeField] 
	private float m_heldObjectOffset;

	[SerializeField] private GameObject m_emptyPrefab;

	private TaskCollider  m_taskCollider;

	private bool m_isSpaceBarDisplayed;

	private bool m_dockedToEat;
	
	private Animator m_animator;

	private string m_oldLayer;
	
	private int m_oldPlaceInLayer;

	private int m_customerIdToEat;

	private bool m_doingQuickTask;

	private bool m_isEating;

	private bool m_isInHungerAnim;

	private bool m_isHungry;

	private SpriteRenderer m_spriteRenderer;

	[SerializeField]
	private Transform m_baseTrans;

	[SerializeField] 
	private Color m_hungryColor;

	[SerializeField] 
	private Color m_hungryBarColor;

	[SerializeField] 
	private float m_hungerFlashTime;

	private float m_hungerTimer;

	private bool m_isRed;

	protected List<Upgrade> m_appliedUpgrades = new List<Upgrade>();

	//The station the worker is currently at. Null if worker is not working
	protected TaskManager m_workerStation;

	private GameObject m_objectHeld;

	public TaskCollider CurrentTaskCollider
	{
		get { return m_taskCollider; }
	}
	
	public float CookingSpeed
	{
		get
		{
			return m_cookingSpeed;
		}
		set
		{
			m_cookingSpeed = value;
		}
	}

	public float RegisterSpeed
	{
		get
		{
			return m_registerSpeed;
		}
		set
		{
			m_registerSpeed = value;
		}
	}

	public float CleaningSpeed
	{
		get
		{
			return m_cleaningSpeed;
		}
		set
		{
			m_cleaningSpeed = value;
		}
	}

	public WorkerState State
	{
		get
		{
			return m_state;
		}
		set
		{
			m_state = value;
		}
	}

	public bool HoldingFood
	{
		get
		{
			return m_holdingFood;
		}
	}

	public bool HoldingBody
	{
		get
		{
			return m_holdingBody;
		}
	}

	public bool HoldingSomething
	{
		get
		{
			return m_holdingBody || m_holdingPotatoes || m_holdingFood || m_holdingMeat || m_holdingIce;
		}
	}

	public bool HoldingPotatoes
	{
		get { return m_holdingPotatoes; }
	}

	public GameObject ObjectHeld
	{
		get { return m_objectHeld; }
	}
	
	public bool IsSpaceBarDisplayed
	{
		get
		{
			return m_isSpaceBarDisplayed;
		}
		set
		{
			if (m_isSpaceBarDisplayed != value)
			{
				m_isSpaceBarDisplayed = value;
				if (value)
				{
					m_spaceBarUI.AnimateIn();
				}
				else
				{
					m_spaceBarUI.AnimateOut();
				}
			}
		}
	}

	public bool IsWorking
	{
		get { return m_isWorking; }
	}

	public bool DockedToEat
	{
		get { return m_dockedToEat; }
		set { m_dockedToEat = value; }
	}

	public bool HoldingMeat
	{
		get { return m_holdingMeat; }
	}

	public bool HoldingIce
	{
		get { return m_holdingIce; }
	}

	public Transform BaseTrans
	{
		get { return m_baseTrans; }
	}

	public bool DoingQuickTask
	{
		get { return m_doingQuickTask; }
	}

	public bool CanMove
	{
		get { return !m_isInHungerAnim && !m_isEating && !m_doingQuickTask && !CustomerManager.Instance.IsBeingFined; }
	}

	public void Init(int id)
	{
		m_workerId = id;
	}
	
	protected void Awake()
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
		m_originalColor = m_spriteRenderer.color;
		m_taskCollider = GetComponentInChildren<TaskCollider>();
		m_movementSpeed = m_baseMovementSpeed;
		m_animator = GetComponent<Animator>();
	}

	void Update()
	{
		if (EatingMechanicManager.Instance.IsHungerEnabled)
		{
			m_movementSpeed = m_baseMovementSpeed * EatingMechanicManager.Instance.MovementModifier;
		}
		if (State == WorkerState.Walking)
		{
			float step = m_movementSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, m_destination, step);
			if (transform.position == m_destination)
			{
				if (!EatingMechanicManager.Instance.IsHungerEnabled || !EatingMechanicManager.Instance.IsEnergyEmpty) 
				{
					m_workerStation.OnWorkingChanged (true, this);
					m_state = m_workerStation.WorkerStateForTask;
					m_isWorking = true;
				}
				
			}
		}
		if (m_holdingBody)
		{
			AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
			Vector3 objectPos = m_bodyToHold.transform.localPosition;

			SpriteRenderer renderer = m_bodyToHold.GetComponentInChildren<SpriteRenderer>();

			if (stateInfo.IsName("worker_body_anim_back") || stateInfo.IsName("worker_body_still_back"))
			{
				renderer.sortingLayerName = "Workers";
				renderer.sortingOrder = 1;
				objectPos.x = 0;
				objectPos.y = -0.2f;

			}
			if (stateInfo.IsName("worker_body_anim_front") || stateInfo.IsName("worker_body_still_front"))
			{
				renderer.sortingLayerName = "HeldItem";
				objectPos.x = 0;
				objectPos.y = -0.1f;
		
			}
			if (stateInfo.IsName("worker_body_anim_left") || stateInfo.IsName("worker_body_still_left"))
			{
				renderer.sortingLayerName = "HeldItem";
				objectPos.x = m_heldObjectOffset;
				objectPos.y = -0.1f;

			}
			if (stateInfo.IsName("worker_body_anim_right") || stateInfo.IsName("worker_body_still_right"))
			{
				renderer.sortingLayerName = "HeldItem";
				objectPos.x = -m_heldObjectOffset;
				objectPos.y = -0.1f;
			}
			m_bodyToHold.transform.localPosition = objectPos;
		}
		else if (HoldingSomething)
		{
			AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
			Vector3 objectPos = m_objectHeld.transform.localPosition;

			SpriteRenderer renderer = m_objectHeld.GetComponentInChildren<SpriteRenderer>();

			if (stateInfo.IsName("worker_back_anim") || stateInfo.IsName("worker_still_back")  || stateInfo.IsName("worker_quicktask"))
			{
				renderer.sortingLayerName = "HeldItem";
				objectPos.x = 0;
			}
			if (stateInfo.IsName("worker_front_anim") || stateInfo.IsName("worker_still_front"))
			{
				renderer.sortingLayerName = "Workers";
				renderer.sortingOrder = 1;
				objectPos.x = 0;

			}
			if (stateInfo.IsName("worker_left_anim") || stateInfo.IsName("worker_still_left"))
			{
				renderer.sortingLayerName = "HeldItem";
				objectPos.x = -m_heldObjectOffset;
			}
			if (stateInfo.IsName("worker_right_anim") || stateInfo.IsName("worker_still_right"))
			{
				renderer.sortingLayerName = "HeldItem";
				objectPos.x = m_heldObjectOffset;
			}
			m_objectHeld.transform.localPosition = objectPos;
		}

		if (m_isHungry)
		{
			m_hungerTimer += Time.deltaTime;
			if (m_hungerTimer >= m_hungerFlashTime)
			{
				if (m_isRed)
				{
					m_spriteRenderer.color = m_originalColor;
					EnergyBarManager.Instance.SetColor(m_originalColor);
				}
				else
				{
					m_spriteRenderer.color = m_hungryColor;
					EnergyBarManager.Instance.SetColor(m_hungryBarColor);
				}
				m_isRed = !m_isRed;
				m_hungerTimer -= m_hungerFlashTime;
			}
		}
	}

	public void DiscardFood()
	{
		m_state = WorkerState.Idle;
		m_holdingFood = false;
		Destroy(ObjectHeld.gameObject);
		m_objectHeld = null;
	}
	
	protected virtual void OnMouseDown()
	{
		WorkerManager.Instance.WorkerClicked(m_workerId);
	}
	
	/*
	 * Returns this workers speed for the given task type
	 */
	public float GetSpeed(TaskManager.TaskType taskType)
	{
		switch (taskType)
		{
			case TaskManager.TaskType.Cleaning:
				return m_cleaningSpeed;
			case TaskManager.TaskType.Register:
				return m_registerSpeed;
			case TaskManager.TaskType.Stove:
				return m_cookingSpeed;
		}
		return 1;
	}
	
	
	/*
	 * Sets the workers speed for the given task type
	 */
	public void SetSpeed(TaskManager.TaskType taskType, float newValue)
	{
		switch (taskType)
		{
			case TaskManager.TaskType.Cleaning:
				m_cleaningSpeed = newValue;
				break;
			case TaskManager.TaskType.Register:
				m_registerSpeed = newValue;
				break;
			case TaskManager.TaskType.Stove:
				m_cookingSpeed = newValue;
				break;
		}
		//If the worker is currently working, update the
		//speed at which the task is progressing on the
		//station side
		if (IsWorking && m_workerStation.Type == taskType)
		{
			m_workerStation.SetWorkSpeed(newValue);
		}
	}

	public void SetSelectedUI(bool isBeingSelected)
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		spriteRenderer.color = isBeingSelected ? m_highlightedColor : m_originalColor;
	}

	public void AddUpgrade(Upgrade upgrade)
	{
		m_appliedUpgrades.Add(upgrade);
	}

	public bool IsUpgradeApplied(Upgrade upgrade)
	{
		return m_appliedUpgrades.Contains(upgrade);
	}

	public void AddFood(GameObject foodObject)
	{
		m_objectHeld = foodObject;
		m_oldLayer = m_objectHeld.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
		m_oldPlaceInLayer = m_objectHeld.GetComponentInChildren<SpriteRenderer>().sortingOrder;
		m_holdingFood = true;
		WorkerManager.Instance.OnWorkerGetFood(foodObject.GetComponent<Food>());
	}

	public void RemoveFood()
	{
		m_objectHeld.GetComponentInChildren<SpriteRenderer>().sortingLayerName = m_oldLayer;
		m_objectHeld.GetComponentInChildren<SpriteRenderer>().sortingOrder = m_oldPlaceInLayer;
		m_objectHeld = null;
		m_holdingFood = false;
	}

	public void AddPotatoes(GameObject potatoObject)
	{
		m_animator.SetBool("IsCuttingPotatoes", false);
		m_objectHeld = potatoObject;
		m_oldLayer = m_objectHeld.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
		m_oldPlaceInLayer = m_objectHeld.GetComponentInChildren<SpriteRenderer>().sortingOrder;
		m_holdingPotatoes = true;
		WorkerManager.Instance.OnGetPotatoes ();
	}
	
	public void AddMeat(GameObject meatObject)
	{
		m_objectHeld = meatObject;
		m_oldLayer = m_objectHeld.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
		m_oldPlaceInLayer = m_objectHeld.GetComponentInChildren<SpriteRenderer>().sortingOrder;
		m_holdingMeat = true;
	}

	public void RemoveMeat()
	{
		Destroy(m_objectHeld);
		m_objectHeld = null;
		m_holdingMeat = false;
	}

	public void AddIce(GameObject iceObject)
	{
		m_objectHeld = iceObject;
		m_oldLayer = m_objectHeld.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
		m_oldPlaceInLayer = m_objectHeld.GetComponentInChildren<SpriteRenderer>().sortingOrder;
	
		m_holdingIce = true;
	}
	
	public void RemoveIce()
	{
		Destroy(m_objectHeld);
		m_objectHeld = null;
		m_holdingIce = false;
	}

	public void RemovePotatoes()
	{
		Destroy(m_objectHeld);
		m_objectHeld = null;
		m_holdingPotatoes = false;
	}

	public void SetHungry(bool isHungry)
	{
		m_isHungry = isHungry;
		if (!m_isHungry && m_isRed)
		{
			m_spriteRenderer.color = m_originalColor;
			EnergyBarManager.Instance.SetColor(m_originalColor);
			m_isRed = false;
		}
	}

	public bool StartWorking()
	{
		TaskManager currentCollidingTask = m_taskCollider.CurrentCollidingTask;
		
		if (currentCollidingTask == null)
		{
			return false;
		}
		
		
		// don't start working if energy is completely depleted, unless you are eating someone
		if (EatingMechanicManager.Instance.IsHungerEnabled && EatingMechanicManager.Instance.IsEnergyEmpty) {
			if (currentCollidingTask.Type != TaskManager.TaskType.Cleaning && !DockedToEat && !HoldingBody)
			{
				HungerAnim();
				return false;
			}
		}

		if (!currentCollidingTask.OnWorkingChanged(true, this))
		{
			m_spaceBarUI.AnimateOut();
			return false;
		}
		if (currentCollidingTask.DoNotAnchor)
		{
			currentCollidingTask.OnWorkingChanged(true, this);
			m_spaceBarUI.AnimateOut();
			m_workerStation = null;
			return false;
		}
		m_workerStation = currentCollidingTask;
		m_state = m_workerStation.WorkerStateForTask;
		transform.position = m_workerStation.WorkerDestination;
		m_isWorking = true;
		m_spaceBarUI.AnimateOut();
		return true;
	}

	public void DockEating()
	{
		TaskManager currentCollidingTask = m_taskCollider.CurrentCollidingTask;
		if (currentCollidingTask == null || HoldingSomething)
		{
			return;
		}
		if (currentCollidingTask is RegisterManager)
		{
			m_spaceBarUI.AnimateOut();
			m_dockedToEat = true;
			StartWorking();
		}
		else if (currentCollidingTask is ServeFoodManager)
		{
			((ServeFoodManager)currentCollidingTask).Eat(this);
		}
	}

	public bool StopWorking()
	{
		if (m_doingQuickTask)
		{
			return false;
		}
		if (m_workerStation == null)
		{
			return false;
		}
		if (m_workerStation is CuttingBoardManager)
		{
			SoundManager.Instance.StopChop();
		}
		else if (m_workerStation is CleaningManager)
		{
			SoundManager.Instance.OnCleanStop();
		}
		m_workerStation.OnWorkingChanged(false, this);
		m_isWorking = false;
		m_state = WorkerState.Idle;
		m_workerStation = null;
		m_spaceBarUI.AnimateIn();
		return true;
	}

	public void SetVelocity(Vector2 movementVector)
	{
		float movementSpeed = m_holdingBody ? m_carryingBodySpeed : m_movementSpeed;
		m_animator.SetBool("IsStill", movementVector.Equals(Vector2.zero));
		m_animator.SetBool("IsWalkingForward", movementVector.y < 0);
		m_animator.SetBool("IsWalkingBack", movementVector.y > 0);
		m_animator.SetBool("IsWalkingLeft", movementVector.x < 0);
		m_animator.SetBool("IsWalkingRight", movementVector.x > 0);
	/*	if (GetComponent<Rigidbody2D>().velocity.Equals( movementVector * movementSpeed))
		{
			return;
		}*/
		GetComponent<Rigidbody2D>().velocity = movementVector * movementSpeed;


	}

	public GameObject ChangeBodyHeld(bool isBodyHeld, Customer.CustomerType type = Customer.CustomerType.Normal)
	{
		StopWorking();
		m_holdingBody = isBodyHeld;
		m_bodyToHold.SetActive(isBodyHeld);
		m_animator.SetBool("HoldingBody", isBodyHeld);
		if (isBodyHeld)
		{
			m_dockedToEat = false;
			m_oldLayer = m_bodyToHold.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
			m_oldPlaceInLayer = m_bodyToHold.GetComponentInChildren<SpriteRenderer>().sortingOrder;
			m_bodyToHold.GetComponent<DeadCustomer>().SetType(type);
			return null;
		}
		else
		{
			m_bodyToHold.GetComponentInChildren<SpriteRenderer>().sortingLayerName = m_oldLayer;
			m_bodyToHold.GetComponentInChildren<SpriteRenderer>().sortingOrder = m_oldPlaceInLayer;
			return m_bodyToHold;
		}
	}

	public void OnEatingFinished()
	{
		m_animator.SetBool("IsEating", false);
		m_animator.SetBool("HoldingBody", true);
		CustomerManager.Instance.EatCustomer(m_customerIdToEat);
		m_customerIdToEat = -1;
		m_isEating = false;
	}

	public void Eat(Customer customer)
	{
		TutorialManager.Instance.OnCustomerEaten ();
		m_isEating = true;
		SetVelocity(Vector2.zero);
		m_animator.SetBool("IsEating", true);
		GameObject empty = Instantiate(m_emptyPrefab, transform.position + m_customerEatOffset, Quaternion.identity);
		customer.transform.parent = empty.transform;
		customer.transform.localPosition = Vector3.zero;
		customer.Animator.SetTrigger("Eaten");
		customer.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Workers";
		customer.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
		m_customerIdToEat = customer.CustomerId;
		SoundManager.Instance.OnEat();
	}

	public void OnChop()
	{
		SoundManager.Instance.OnChop();
	}

	public void OnBite()
	{
		SoundManager.Instance.OnBite();
	}

	public void QuickTaskStart()
	{
		SetVelocity(Vector2.zero);
		m_animator.SetTrigger("DoingQuickTask");
		m_doingQuickTask = true;
	}

	public void QuickTaskFinish()
	{
		m_doingQuickTask = false;
	}

	public void OnMop()
	{
		SoundManager.Instance.OnCleanStart();
	}

	public void HungerAnim()
	{
		SetVelocity(Vector2.zero);
		m_isInHungerAnim = true;
		m_animator.SetTrigger("HungerAnim");
		SoundManager.Instance.PlayHungerSound();
	}

	public void OnHungerAnimFinished()
	{
		m_isInHungerAnim = false;
	}

	public void SetMovementSpeed(float NewSpeed)
	{
		m_baseMovementSpeed = NewSpeed;
	}

	public void MakeChef()
	{
		m_animator.runtimeAnimatorController = m_chefAnimator;
	}
}
