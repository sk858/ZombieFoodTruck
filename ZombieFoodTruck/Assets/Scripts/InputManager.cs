using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    private bool m_grabbed;

    private bool m_movementMode = true;

    private GameObject m_currentEmptyObject;
	
    private GrabbableObject m_objectGrabbed;

    private bool m_hasMovedOnce;

    [SerializeField]
    private bool m_disableEating;
    
    [SerializeField] 
    private bool m_arrowMode;

    [SerializeField] 
    private GameObject m_movePrompt;

    [SerializeField]
    private string[] m_layerMask;

	[SerializeField]  
	private GameObject m_levelSelectPanel;

    [SerializeField]
    private CleaningManager[] m_cleanUpTasks;

    [SerializeField] private PauseButton m_pauseButton;

    [SerializeField] private MuteManager m_muteManager;

    public bool ArrowMode
    {
        get { return m_arrowMode; }
    }

    public bool DisableEating
    {
        get { return !CanEat(); }
    }

    public bool LevelDisabledEating;

    public bool ForceEat = false;

    // Use this for initialization
    void Awake ()
    {
       // m_layerMaskNum = LayerMask.GetMask(m_layerMask);
    }

    private bool CanEat()
    {
        if (LevelDisabledEating)
        {
            return false;
        }
        foreach (var task in m_cleanUpTasks)
        {
            if (task.gameObject.activeSelf && !task.IsThereATask)
            {
                return true;
            }
        }
        return false;
    }
	
    // Update is called once per frame
    void Update () 
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            m_muteManager.ToggleMute();
        }
        if (Input.GetKeyDown(KeyCode.R) && !EndingSequenceManager.Instance.Started)
        {
            GameManagerUser.Instance.RestartDay();
        }
		if (PauseManager.Instance.IsGamePaused) {
			return;
		}
		if (ArrowMode && GameManagerUser.Instance.LevelStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space) && WorkerManager.Instance.CanMove())
            {
				TruckLogger.Instance.LogPressSpacebar ();
				if (m_movementMode) {
					m_movementMode = !WorkerManager.Instance.StartWorking ();
				}

            }
            else if (!m_movementMode && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
            {
                m_movementMode = true;
                WorkerManager.Instance.StopWorking();
            }
            else if (Input.GetKeyDown(KeyCode.E) && !DisableEating)
            {
                WorkerManager.Instance.DockEating();
                ForceEat = false;
            }

            if (m_movementMode && WorkerManager.Instance.CanMove())
            {
                Vector2 velocity = Vector3.zero;

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    velocity += Vector2.up;
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    velocity += Vector2.left;
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    velocity += Vector2.right;
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    velocity += Vector2.down;
                }
                if (!m_hasMovedOnce && velocity != Vector2.zero)
                {
                    m_hasMovedOnce = true;
					TutorialManager.Instance.OnFirstMove ();
                    SetMovePrompt(false);
                }
                WorkerManager.Instance.MoveWorker(velocity);
            }

            if (m_hasMovedOnce && m_movePrompt.GetComponent<ButtonSpawn>().Active)
            {
                SetMovePrompt(false);
            }

			// open/close the level select screen if player presses escape
			if (Input.GetKeyDown (KeyCode.Escape) && DebugManager.Instance.DebugMode) {
				m_levelSelectPanel.SetActive (!m_levelSelectPanel.activeSelf);
			}
        }
              /*  if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, m_layerMaskNum);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Station"))
                {
                    TaskManager taskManager = hit.collider.GetComponent<TaskManager>();
                    WorkerManager.Instance.SetWorkerTask(taskManager);
                }
                else if (hit.collider.CompareTag("Customer"))
                {
                    Customer customer = hit.collider.GetComponent<Customer>();
                    CustomerManager.Instance.CustomerClicked(customer.CustomerId);
                }
            }
        }*/
    }

    public void SetMovePrompt(bool isEnabled)
    {
        m_movePrompt.transform.parent = null;
        m_movePrompt.GetComponent<ButtonSpawn>().AnimateOut();
    }
    

    private bool IsArrowKeyPressed()
    {
        return  Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || 
                Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || 
                Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow);
    }

    public void ForceStopWorking()
    {
        m_movementMode = true;
        WorkerManager.Instance.StopWorking();
    }
}

