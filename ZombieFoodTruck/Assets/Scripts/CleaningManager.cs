using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CleaningManager : TaskManager
{
	public class BodyCleanedEvent : UnityEvent{ };

	public BodyCleanedEvent OnBodyCleaned = new BodyCleanedEvent ();

    [SerializeField]
    private GameObject m_miniGame;

    [SerializeField] 
    private GameObject m_deadBody;

    private Customer.CustomerType m_bodyType;

    [SerializeField] 
    private GameObject m_pointingHandUI;

    [SerializeField] 
    private GameObject m_meatPrefab;

    [SerializeField] 
    private bool m_holdToTask;

    [SerializeField] 
    private GameObject[] m_progressBars;
    
    private Animator m_animator;

    private bool m_isHalfway;

    private int m_tier = -1;


    private void Start()
    {
        m_animator = GetComponent<Animator>();
        if (m_tier == -1)
        {
            m_progressBarParent = m_progressBars[0];
            m_progressBar = m_progressBarParent.GetComponentInChildren<ProgressBar>();
         //   SetProgressBarActive(false);
            m_tier = 0;
        }
    }

    protected override void Update () 
    {
        if (IsDoingTask && m_isThereATask)
        {
            if (!Input.GetKey(KeyCode.Space) && m_holdToTask)
            {
                InputManager.Instance.ForceStopWorking();
            }
            m_taskProgress += m_workSpeed * Time.deltaTime;
            UpdateProgressBar();
            if (!m_isHalfway && m_taskProgress >= (m_progressForCompletion/2f))
            {
                m_isHalfway = true;
                m_animator.SetTrigger("Halfway");
            }
            if (m_taskProgress >= m_progressForCompletion)
            {
                m_taskProgress = 0f;
                UpdateProgressBar();
                BodyCleaned();
				TruckLogger.Instance.LogFinishCleaning ();
				TutorialManager.Instance.OnTaskFinished (TaskType.Cleaning);
				OnTaskFinishedEvent.Invoke(TaskType.Cleaning);
            }
        }
    }
    
    public void BodyCleaned()
    {
        ConsumableSpawner.Instance.OnBodyCleaned(m_bodyType);
        m_isThereATask = false;
        m_deadBody.SetActive(false);
        m_miniGame.gameObject.SetActive(false);
        if (EatingMechanicManager.Instance.IsMeatEnabled)
        {
            GameObject meat = Instantiate(m_meatPrefab, m_workerDoingTask.transform);
            meat.transform.localPosition = Vector3.zero;
            m_workerDoingTask.AddMeat(meat);
        }
		OnBodyCleaned.Invoke ();
        m_animator.SetTrigger("Done");
        m_workerDoingTask.GetComponent<Animator>().SetBool("IsCleaningBody", false);
        SetProgressBarActive(false);
        OnWorkingChanged(false, m_workerDoingTask);
    }
    
    public override bool OnWorkingChanged(bool isWorking, Worker worker)
    {
        if (!worker.HoldingBody && (worker.HoldingSomething || !m_isThereATask))
        {
            return false;
        }
        if (worker.HoldingBody)
        {
            GameObject deadBody = worker.ChangeBodyHeld(false);
            m_deadBody.GetComponent<SpriteRenderer>().sprite = deadBody.GetComponent<SpriteRenderer>().sprite;
            m_deadBody.SetActive(true);
            m_bodyType = deadBody.GetComponent<DeadCustomer>().Type;
            m_isThereATask = true;
			TutorialManager.Instance.OnTaskStarted (TaskType.Cleaning);
			TruckLogger.Instance.LogStartCleaning ();
            m_animator.SetTrigger("Started");
            m_isHalfway = false;
            SetProgressBarActive(true);
        }
        else if (!m_isThereATask)
        {
            return false;
        }
        if (isWorking)
        {
            worker.transform.position = transform.position + m_workerStationOffset;
        }
        worker.GetComponent<Animator>().SetBool("IsCleaningBody", isWorking);
        base.OnWorkingChanged(isWorking, worker);
        return true;
    }

    public override bool CanDock(Worker worker)
    {
        if (m_isDoingTask)
        {
            return false;
        }
        
        if (worker.HoldingBody || (!worker.HoldingSomething && m_isThereATask))
        {
            return true;
        }
        return false;
    }

    public void SetProgressBarTier(int tier)
    {
        m_tier = tier;
        m_progressBarParent = m_progressBars[tier];
        m_progressBar = m_progressBarParent.GetComponentInChildren<ProgressBar>();
       // SetProgressBarActive(false);
    }
		
}
