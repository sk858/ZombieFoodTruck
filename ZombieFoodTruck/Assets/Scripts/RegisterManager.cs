using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RegisterManager : TaskManager {
			
	[SerializeField] 
	private Customer.CustomerState m_customerStateOnWait;

	[SerializeField] 
	private Vector3 m_eatOffset;

	private int m_customerIdForTask = -1;
	
	public int CustomerIdForTask
	{
		get
		{
			return m_customerIdForTask;
		}
	}

	// Update is called once per frame
	protected override void Update () 
	{
		if (IsDoingTask && m_isThereATask)
		{
				m_taskProgress += m_workSpeed * Time.deltaTime;
				if (m_taskProgress >= m_progressForCompletion)
				{
	
				}
		}
	}

	private void ServeCustomer()
	{
		CustomerManager.Instance.OnCustomerOrderTaken(m_customerIdForTask);
		CustomerLeft();

		TutorialManager.Instance.OnTaskFinished(TaskType.Register);
		TruckLogger.Instance.LogOrderTaken ();
	}
	
	public override bool OnWorkingChanged(bool isWorking, Worker worker)
	{
		if (worker.HoldingBody)
		{
			return false;
		}
		if (worker.DockedToEat && m_customerIdForTask != -1 && isWorking)
		{
			Customer toEat = CustomerManager.Instance.GetCustomer(m_customerIdForTask);
			if (toEat.Type == Customer.CustomerType.Police)
			{
				worker.DockedToEat = false;
				toEat.GetComponentInChildren<Animator>().SetTrigger("Eaten");
				// TODO Show no eat animation
				return false;
			}
			worker.transform.position = transform.position + m_eatOffset;
			worker.Eat(toEat);
			return false;
		}
		if (EatingMechanicManager.Instance.IsEnergyEmpty)
		{
			worker.HungerAnim();
			return false;
		}
		if (!m_isThereATask || worker.HoldingSomething)
		{
			return false;
		}
		if (isWorking)
		{
			if (m_customerIdForTask != -1)
			{
				Customer customer = CustomerManager.Instance.GetCustomer(m_customerIdForTask);
				customer.IsBeingHelped = true;
			}
			TutorialManager.Instance.OnTaskStarted (TaskType.Register);
			Debug.Log ("starting register task");
			ServeCustomer();
		}
		
		return false;
	}
	
	public void CustomerLeft()
	{
		m_isThereATask = false;
		m_customerIdForTask = -1;
	}

	// Returns old customer or null if there wasn't one
	public Customer ChangeOrder(int customerId)
	{
		if (m_customerIdForTask != -1)
		{
			Customer customer = CustomerManager.Instance.GetCustomer(m_customerIdForTask);
			customer.IsBeingHelped = false;
			customer.State = Customer.CustomerState.InLineForOrder;
		}
		Customer newCustomer = CustomerManager.Instance.GetCustomer(customerId);
		m_taskProgress = newCustomer.CurrentTaskProgress;
		newCustomer.IsBeingHelped = IsDoingTask;

		m_customerIdForTask = customerId;
		if (m_workerDoingTask != null && m_workerDoingTask.DockedToEat)
		{
			m_workerDoingTask.transform.position = transform.position + m_eatOffset;
			m_workerDoingTask.Eat(newCustomer);
			return null;
		}
		m_isThereATask = true;
		return null;
	}

	public override bool CanDock(Worker worker)
	{
		if (EatingMechanicManager.Instance.IsHungerEnabled && EatingMechanicManager.Instance.IsEnergyEmpty)
		{
			return false;
		}
		if (!worker.HoldingSomething && m_isThereATask)
		{
			return true;
		}
		return false;
	}

}
