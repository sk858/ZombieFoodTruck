using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ServeFoodManager : TaskManager
{
    [SerializeField] 
    private Vector3 m_eatOffset;

    private AudioSource m_audioSource;
    
	public class FoodServedEvent : UnityEvent{ };

	// called whenever food item is served
	public FoodServedEvent OnFoodServed = new FoodServedEvent();

    private int m_customerIdForTask = -1;
    
    public int CustomerIdForTask
    {
        get
        {
            return m_customerIdForTask;
        }
    }

    private List<Food> m_foodOnTray = new List<Food>();

    private List<Food.FoodType> m_foodLeftInOrder = new List<Food.FoodType>();

    [SerializeField]
    private float m_horizontalOffset;

    [SerializeField]
    private Vector3 m_foodPlacement;

    private int m_orderSize;

    private Customer m_customerForTask;

	public List<Food.FoodType> FoodLeftInOrder {
		get { return m_foodLeftInOrder; }
	}

    protected override void Awake()
    {
        base.Awake();
        m_audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        if (m_isThereATask && m_foodLeftInOrder.Count == 0)
        {
            if (m_customerForTask.State == Customer.CustomerState.GettingFoodServed)
            {
                ServeFood();
            }
        }
    }
    
    public override bool OnWorkingChanged(bool isWorking, Worker worker)
    {
        if (worker.HoldingBody)
        {
            return false;
        }
        if (isWorking && worker.HoldingFood && m_isThereATask)
        {
            Food food = worker.ObjectHeld.GetComponent<Food>();
            if (m_foodLeftInOrder.Contains(food.Type))
            {
                m_foodLeftInOrder.Remove(food.Type);
                worker.RemoveFood();
                PutFoodOnTray(food);
				OnFoodServed.Invoke ();
                if (m_foodLeftInOrder.Count == 0 && m_customerForTask.State == Customer.CustomerState.GettingFoodServed)
                {
                    ServeFood();
                }
                else
                {
                    CustomerManager.Instance.IncreasePatienceOnFood();
                }
            }
            return true;
        }
        return false;
    }
    
    // Returns old customer or null if there wasn't one
    public Customer ChangeOrder(int customerId, List<Food.FoodType> order)
    {
        m_customerForTask = CustomerManager.Instance.GetCustomer(customerId);
        m_customerIdForTask = customerId;
        m_isThereATask = true;
        m_foodLeftInOrder = new List<Food.FoodType>(order);
        m_orderSize = order.Count;
        WorkerManager.Instance.OnNewOrder();
        return null;
    }

    public void CustomerLeft()
    {
        m_isThereATask = false;
        m_foodLeftInOrder = null;
        DestroyFoodOnTray();
        m_customerIdForTask = -1;
    }

    public void CustomerEaten()
    {
        m_isThereATask = false;
        m_foodLeftInOrder = null;
        DestroyFoodOnTray();
        m_customerIdForTask = -1;
    }

    private void DestroyFoodOnTray()
    {
        for (int i = 0; i < m_foodOnTray.Count; i++)
        {
            Destroy(m_foodOnTray[i].gameObject);
        }
        m_foodOnTray = new List<Food>();
    }

    public void PutFoodOnTray(Food food)
    {
        food.transform.parent = transform;
        float m_startingX = 0f;
		
        if (m_orderSize % 2 == 0)
        {
            m_startingX = (-m_horizontalOffset / 2f) - (m_horizontalOffset * ((m_orderSize/ 2) - 1));
        }
        else
        {
            m_startingX = -(m_horizontalOffset * (m_orderSize / 2));
        }
        food.transform.localPosition = new Vector3(m_startingX + m_horizontalOffset * m_foodOnTray.Count, 0, -0.1f);
        m_foodOnTray.Add(food);
        food.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Customers";
        food.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
        if (food is IceCream)
        {
            ((IceCream)food).OnServe(this);
        }
        m_audioSource.Play();
    }

    public void RemoveFood(Food food)
    {
        m_foodLeftInOrder.Add(food.Type);
        m_foodOnTray.Remove(food);
        for (int i = 0; i < m_foodOnTray.Count; i++)
        {
            m_foodOnTray[i].transform.localPosition = m_foodPlacement + (i * m_horizontalOffset * Vector3.right);
        }
    }

    public override bool CanDock(Worker worker)
    {
        if (worker.HoldingFood && m_isThereATask)
        {
            Food food = worker.ObjectHeld.GetComponent<Food>();
            if (m_foodLeftInOrder.Contains(food.Type))
            {
                return true;
            }
        }
        return false;
    }

    public void ServeFood()
    {
        int oldCustomerId = m_customerIdForTask;
        m_customerIdForTask = -1;
        CustomerLeft();
        CustomerManager.Instance.OnCustomerOrderServed(oldCustomerId);
        TutorialManager.Instance.OnTaskFinished (TaskType.Tray);
        TruckLogger.Instance.LogFoodServed ();
    }

    public void Eat(Worker worker)
    {
        if (m_customerIdForTask != -1 && m_customerForTask.State == Customer.CustomerState.GettingFoodServed)
        {
            Customer toEat = CustomerManager.Instance.GetCustomer(m_customerIdForTask);
            if (toEat.Type == Customer.CustomerType.Police)
            {
                worker.DockedToEat = false;
                // TODO Show no eat animation
                return;
            }
            worker.transform.position = transform.position + m_eatOffset;
            worker.Eat(toEat);
        }
    }

    public bool NeedsFood(Food.FoodType foodType)
    {
        if (m_customerIdForTask == -1)
        {
            return true;
        }
        return m_foodLeftInOrder.Contains(foodType);
    }
}
