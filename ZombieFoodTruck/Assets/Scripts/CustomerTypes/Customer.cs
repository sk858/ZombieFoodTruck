using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
	public enum CustomerState
	{
		Undef,
		Walking,
		GettingOrderTaken,
		GettingFoodServed,
		InLineForOrder,
		InLineForFood,
		Fleeing,
		GivingTicket,
		Leaving,
		Dead
	};

	public enum CustomerType
	{
		Normal,
		Smelly,
		Rich,
		ZombieHunter,
		Buff,
		Police,
	}

	[SerializeField] 
	private float m_speed;

	[SerializeField] 
	private float m_runSpeed;
	
	[SerializeField]
	private float m_patience;
	
	[SerializeField]
	private bool m_freezePatience;

	[SerializeField]
	private CustomerState m_state = CustomerState.Undef;

//	[SerializeField]
//	private ProgressBar m_progressBar;
	[SerializeField] private GameObject m_patienceSlider;

	private int m_customerId;

	protected CustomerType m_customerType;

	[SerializeField]
	private GameObject m_selectionArrow;

	[SerializeField] 
	private PatienceBar m_patienceBar;

	private Vector3 m_destination;

	private float m_currentTaskProgress;

	private bool m_isBeingHelped;

	private CustomerState m_stateOnArrival = CustomerState.Undef;
	
	private bool m_isFront;

	private float m_maxPatience;

	private bool m_isAngry;

	[SerializeField]
	private float m_maxTip;

	[SerializeField] 
	private OrderBubble m_orderBubble;

	[SerializeField] 
	private OrderBubble m_longOrderBubble;

	[SerializeField] public Sprite DeadSprite;
	
	private int m_tip;

	private List<Food.FoodType> m_order = new List<Food.FoodType>();

	private Animator m_animator;
	
	public bool FreezePatience
	{
		get
		{
			return m_freezePatience;
		}
		set
		{
			m_freezePatience = value;
		}
	}

	public CustomerState State
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

	public float Patience
	{
		get
		{
			return m_patience;
		}
		set
		{
			m_patience = Mathf.Clamp(value, 0, m_maxPatience);
//			m_progressBar.Value = (m_patience / m_maxPatience);
			m_patienceSlider.GetComponent<Slider>().value = (m_patience / m_maxPatience);
			m_patienceBar.SetPatience(m_patience);
		}
	}
	
	public int CustomerId
	{
		get
		{
			return m_customerId;
		}
	}

	public CustomerType Type
	{
		get { return m_customerType; }
		set { m_customerType = value; }
	}

	public bool IsFront
	{
		get { return m_isFront; }
		set { m_isFront = value; }
	}

	public float MaxPatience
	{
		get { return m_maxPatience; }
		set
		{
			m_maxPatience = value;
			m_patienceBar.Init(m_maxPatience, m_patience);
		}
	}

	public float CurrentTaskProgress
	{
		get
		{
			return m_currentTaskProgress;
		}
	}

	public bool IsBeingHelped
	{
		get
		{
			return m_isBeingHelped;
		}
		set
		{
			m_isBeingHelped = value;
		}
	}

	public bool IsMoving
	{
		get
		{
			return 
				(m_state == CustomerState.Fleeing || 
			 	 m_state == CustomerState.Walking || 
				 m_state == CustomerState.Leaving);
		}
	}

	public float MaxTip
	{
		get { return m_maxTip; }
		set { m_maxTip = value; }
	}

	public CustomerState StateOnArrival
	{
		get { return m_stateOnArrival; }
		set { m_stateOnArrival = value; }
	}

	public List<Food.FoodType> Order
	{
		get
		{
			return m_order;
		}
	}

	public Animator Animator
	{
		get { return m_animator; }
	}

	public bool IsAngry
	{
		get { return m_isAngry; }
	}

	public void Init(int customerId, CustomerState state, Vector3 destination, CustomerState stateOnArrival, float patience, float maxTip, List<Food.FoodType> order)
	{
		m_animator = GetComponentInChildren<Animator>();
		m_customerId = customerId;
		m_state = state;
		m_destination = destination;
		StateOnArrival = stateOnArrival;
		m_patience = patience;
		m_maxPatience = patience;
		m_maxTip = maxTip;
		m_order = order;
		m_orderBubble.gameObject.SetActive(false);
		m_longOrderBubble.gameObject.SetActive(false);
		m_patienceBar.Init(m_maxPatience, m_maxPatience);
		SetAnim(m_destination);
		if (order.Count < 3)
		{
			m_orderBubble.Init(order);
		}
		else
		{
			m_longOrderBubble.Init(order);
		}
	}
	
	void Update () 
	{	
		if (m_state == CustomerState.Walking || 
		         m_state == CustomerState.Leaving ||
		         m_state == CustomerState.Fleeing)
		{
			float speed = m_state == CustomerState.Fleeing ? m_runSpeed : m_speed;
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, m_destination, step);
			if (transform.position == m_destination)
			{
				m_animator.SetBool("IsStill", true);
				m_animator.SetBool("IsWalkingUp", false);
				m_animator.SetBool("IsWalkingDown", false);
				m_animator.SetBool("IsWalkingRight", false);
				if (m_state == CustomerState.Leaving || m_state == CustomerState.Fleeing)
				{
					CustomerManager.Instance.RemoveCustomer(m_customerId);
				}
				else
				{
					m_freezePatience = false;
					m_state = StateOnArrival;
					if (m_state == CustomerState.GettingOrderTaken)
					{
						CustomerManager.Instance.SetRegisterTask(m_customerId);
					}
				}
			}
		}
	}

	private void SetAnim(Vector3 destination)
	{
		m_animator.SetBool("IsStill", false);
		Vector3 directionVector = destination - transform.position;
		bool isWalkingUp = directionVector.y > 0;
		bool isWalkingRight = directionVector.x > 0;
		bool isWalkingDown = directionVector.y < 0;
		if (isWalkingRight && isWalkingUp)
		{
			if (Mathf.Abs(directionVector.y) < Mathf.Abs(directionVector.x))
			{
				m_animator.SetBool("IsWalkingUp", false);
				m_animator.SetBool("IsWalkingDown", false);
				m_animator.SetBool("IsWalkingRight", true);
			}
			else
			{
				m_animator.SetBool("IsWalkingUp", true);
				m_animator.SetBool("IsWalkingDown", false);
				m_animator.SetBool("IsWalkingRight", false);
			}
		}
		else if (isWalkingRight && isWalkingDown)
		{
			if (Mathf.Abs(directionVector.y) < Mathf.Abs(directionVector.x))
			{
				m_animator.SetBool("IsWalkingUp", false);
				m_animator.SetBool("IsWalkingDown", false);
				m_animator.SetBool("IsWalkingRight", true);
			}
			else
			{
				m_animator.SetBool("IsWalkingUp", false);
				m_animator.SetBool("IsWalkingDown", true);
				m_animator.SetBool("IsWalkingRight", false);
			}
		}
		else
		{
			m_animator.SetBool("IsWalkingUp", isWalkingUp);
			m_animator.SetBool("IsWalkingDown", isWalkingDown);
			m_animator.SetBool("IsWalkingRight", isWalkingRight);
		}
	}
	
	public void SetDestination(Vector3 destination, CustomerState stateOnArrival = CustomerState.Undef)
	{
		m_destination = destination;
		StateOnArrival = stateOnArrival;
		SetAnim(destination);
	}
	
	//Gets called by CustomerManager when the smell state needs to be updated
//	public void SetSmellState(BodySmellManager.Smell smellyState)
//	{
//		m_smellyState = smellyState;
//		m_progressBar.SmellyState = smellyState;
//	}
	
	public void SetPatienceBarColor(Color color)
	{
//		m_progressBar.SetFillColor(color);
	}

	public void SetTaskProgress(float progress)
	{
		m_currentTaskProgress = progress;
	}
	
	public virtual int GetTipAmount()
	{
		if ((m_patience / m_maxPatience) >= 0.8)
		{
			m_tip = (int)m_maxTip;
		}
		if ((m_patience / m_maxPatience) >= 0.6 && (m_patience / m_maxPatience) < 0.8)
		{
			float tipAmount = m_maxTip * (3 / 4f);
			m_tip = (int) Math.Round(tipAmount);
		}
		if ((m_patience / m_maxPatience) >= 0.4 && (m_patience / m_maxPatience) < 0.8)
		{
			float tipAmount = m_maxTip * (2 / 4f);
			m_tip = (int) Math.Round(tipAmount);
		}
		if ((m_patience / m_maxPatience) >= 0.2 && (m_patience / m_maxPatience) < 0.4)
		{
			float tipAmount = m_maxTip * (1 / 4f);
			m_tip = (int) Math.Round(tipAmount);
		}
		if ((m_patience / m_maxPatience) < 0.2)
		{
			m_tip = 0;
		}
		return m_tip;
	}

	public Color GetPatienceColor() {
		if ((m_patience / m_maxPatience) >= 0.66f) {
			return m_patienceBar.Green;
		} else if ((m_patience / m_maxPatience) >= 0.33f) {
			return m_patienceBar.Yellow;
		} else {
			return m_patienceBar.Red;
		}
	}

	public void SetOrderBubble(bool isEnabled)
	{
		GameObject orderBubble = m_order.Count < 3 ? m_orderBubble.gameObject : m_longOrderBubble.gameObject;
		if (isEnabled)
		{
			orderBubble.GetComponent<ButtonSpawn>().AnimateIn();
			orderBubble.SetActive(isEnabled);
		}
		else
		{
			orderBubble.GetComponent<ButtonSpawn>().AnimateOut();
		}
	}

	public void OnOrderServed()
	{
		SetOrderBubble(false);
		m_patienceBar.gameObject.SetActive(false);
	}
	
	public void OnPatienceDepleted()
	{
		m_animator.SetTrigger("Angry");
		m_isAngry = true;
	}

	public void OnAngryAnimFinished()
	{
		CustomerManager.Instance.OnAngryAnimationDone(this);
	}

	public void AnimateNewMaxPatience(float maxPatience)
	{
		float m_currentPercent = m_patience / m_maxPatience;
		m_patienceBar.Init(maxPatience, m_currentPercent*maxPatience);
		float time = 1.5f * (1 - m_currentPercent);
		m_maxPatience = maxPatience;
		m_patience = maxPatience;
		LeanTween.value(m_patienceBar.gameObject, (x) =>
		{
			m_patienceBar.SetPatience(x*m_maxPatience);
			m_patienceSlider.GetComponent<Slider>().value = x;
		}, m_currentPercent, 1f, time);
	}
	
	public void AnimateNewPatience(float patience)
	{
		float m_currentPercent = m_patience / m_maxPatience;
		float time = 1.5f * (1 - m_currentPercent);
		LeanTween.value(m_patienceBar.gameObject, (x) =>
		{
			m_patienceBar.SetPatience(x);
			m_patienceSlider.GetComponent<Slider>().value = x/m_maxPatience;
		}, m_patience, patience, time);
	}
	
	
    //Methods to change the game state for special customer types when certain events happen

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void OnOrderTaken() { }

    public virtual void OnServed() { }

    public virtual void OnEaten() { }

    public virtual void OnOtherCustomerEaten() { }
}
