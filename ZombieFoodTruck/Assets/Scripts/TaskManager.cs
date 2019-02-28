using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TaskManager : MonoBehaviour {

	public enum TaskType
	{
		Stove,
		Register,
		Tray,
		Cleaning,
		Cutting,
		Fryer,
		Soda,
		Ice,
		IceCream,
		Trash,
	}
	
	public class TaskFinishedEvent : UnityEvent<TaskType> {};

	[SerializeField] 
	protected GameObject m_progressBarParent;

	protected ProgressBar m_progressBar;

	[SerializeField] 
	protected TaskType m_taskType;

	[SerializeField] 
	private Worker.WorkerState m_workerStateForTask;

	[SerializeField] 
	protected Vector3 m_workerStationOffset;

	[SerializeField]
	private bool m_doNotAnchor;
		
	[SerializeField]
	public float m_progressForCompletion;
	
	
	
	protected float m_taskProgress;

	protected bool m_isDoingTask;

	protected bool m_isThereATask;

	protected float m_workSpeed = 1;

	protected Worker m_workerDoingTask;
	
	public TaskFinishedEvent OnTaskFinishedEvent = new TaskFinishedEvent();
	

	public bool IsThereATask
	{
		get { return m_isThereATask; }
	}

	public TaskType Type
	{
		get
		{
			return m_taskType;
		}
	}

	public Vector3 WorkerDestination
	{
		get
		{
			return transform.position + m_workerStationOffset;
		}
	}

	public Worker.WorkerState WorkerStateForTask
	{
		get
		{
			return m_workerStateForTask;
		}
	}

	public bool IsDoingTask
	{
		get { return m_isDoingTask; }
	}

	public bool DoNotAnchor
	{
		get { return m_doNotAnchor; }
	}

	protected virtual void Awake()
	{
		if (m_progressBarParent != null)
		{
			m_progressBar = m_progressBarParent.GetComponentInChildren<ProgressBar>();
		//	SetProgressBarActive(false);
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		if (IsDoingTask && m_isThereATask)
		{
			m_taskProgress += (Time.deltaTime*m_workSpeed);
			UpdateProgressBar();
			
			if (m_taskProgress >= m_progressForCompletion)
			{
				m_taskProgress = 0f;
				UpdateProgressBar();
				OnTaskFinishedEvent.Invoke(m_taskType);
			}
		}
	}

	public virtual bool OnWorkingChanged(bool isWorking, Worker worker)
	{
		m_isDoingTask = isWorking;
		if (isWorking)
		{
			m_workerDoingTask = worker;
			m_workSpeed = worker.GetSpeed(m_taskType);
		}
		else
		{
			m_workerDoingTask = null;
		}
		return true;
	}
	
	public void SetWorkSpeed(float newValue)
	{
		m_workSpeed = newValue;
	}

	public void UpdateProgressBar()
	{
		if (m_progressBar != null)
		{
			m_progressBar.Value = (m_taskProgress / m_progressForCompletion);
		}
	}

	protected void SetProgressBarActive(bool isActive)
	{
		ButtonSpawn buttonSpawn = m_progressBarParent.GetComponentInChildren<ButtonSpawn>();
		if (buttonSpawn != null)
		{
			if (isActive)
			{
				m_progressBarParent.SetActive(true);
				buttonSpawn.AnimateIn();
			}
			else
			{
				buttonSpawn.AnimateOut();
			}
		}
		else
		{
			m_progressBarParent.SetActive(isActive);
		}
	}

	public void GiveTask(float timeToDoTask)
	{
		m_isThereATask = true;
		m_progressForCompletion = timeToDoTask;
	}

	public void AddProgress(float progress)
	{
		m_taskProgress += progress;
		UpdateProgressBar();
	}
	

	public virtual bool CanDock(Worker worker)
	{
		if (worker.HoldingSomething || worker.IsWorking || EatingMechanicManager.Instance.IsEnergyEmpty)
		{
			return false;
		}
		return true;
	}
	
	public void ChangeProgressBar(GameObject bar)
	{
		bar.SetActive(m_progressBarParent.activeSelf);
		m_progressBarParent.SetActive(false);
		m_progressBarParent = bar;
		m_progressBar = bar.GetComponentInChildren<ProgressBar>();
	}
}
