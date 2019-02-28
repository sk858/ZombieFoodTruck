using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviourSingleton<TutorialManager> {

	[SerializeField] private int m_burgerTutorialLevel;
	[SerializeField] private int m_eatingTutorialLevel;
	[SerializeField] private int m_friesTutorialLevel;
	[SerializeField] private int m_eatRightTutorialLevel;
	[SerializeField] private int m_drinkTutorialLevel;

	[SerializeField] private GameObject m_tutorialTextBubble;
	[SerializeField] private Text m_tutorialText;

	[SerializeField] private GameObject m_stovePointingHand;
	[SerializeField] private GameObject m_registerPointingHand;
	[SerializeField] private GameObject m_trayPointingHand;
	[SerializeField] private GameObject m_cleaningPointingHand;
	[SerializeField] private GameObject m_cuttingBoardPointingHand;
	[SerializeField] private GameObject m_fryerPointingHand;
	[SerializeField] private GameObject m_icePointingHand;
	[SerializeField] private GameObject m_sodaPointingHand;
	[SerializeField] private GameObject m_iceCreamPointingHand;
	[SerializeField] private GameObject m_trashPointingHand;

	[SerializeField] private Text m_stoveText;
	[SerializeField] private Text m_registerText;
	[SerializeField] private Text m_trayText;
	[SerializeField] private Text m_cleaningText;
	[SerializeField] private Text m_cuttingBoardText;
	[SerializeField] private Text m_fryerText;
	[SerializeField] private Text m_iceText;
	[SerializeField] private Text m_sodaText;
	[SerializeField] private Text m_iceCreamText;
	[SerializeField] private Text m_trashText;

	[SerializeField] private Worker m_worker;
	[SerializeField] private GameObject m_eatingPrompt;
	[SerializeField] private GameObject m_rightEatingPrompt;
	[SerializeField] private GameObject m_pauseDim;
	[SerializeField] private GameObject m_proceedPrompt;

	[SerializeField] private GameObject m_registerMask;
	[SerializeField] private GameObject m_stoveMask;
	[SerializeField] private GameObject m_serveFoodMask;
	[SerializeField] private GameObject m_cleaningMask;
	[SerializeField] private GameObject m_fryerMask;
	[SerializeField] private GameObject m_cuttingMask;
	[SerializeField] private GameObject m_drinkMask;
	[SerializeField] private GameObject m_iceMask;
	[SerializeField] private GameObject m_energyBarMask;
	[SerializeField] private GameObject m_workerMask;

	private bool m_playerMoved = false;
	private bool m_customerEaten = false;
	private int m_customersPastRegister = 0;
	private bool m_energyDepleted = false;
	private bool m_friesCut = false;
	private bool m_learnedFries = false;
	private bool m_learnedFriesPause = false;
	private bool m_eatenAtRightWindow = false;
	private bool m_iceGrabbed = false;
	private bool m_iceLoaded = false;
	private bool m_sodaStarted = false;
	private bool m_sodaRestarted = false;
	private bool m_sodaGrabbed = false;
	private bool m_learnedDrinkPause = false;
	private bool m_textBubbleShowing = false;
	private bool m_wrongFood = false;
	private bool m_wrongPotatoes = false;
	private bool m_bodyLeftOverActive = false;


	private Food.FoodType m_wrongFoodGrabbed;

	public bool TutorialMode = true;

	private Dictionary<TaskManager.TaskType,GameObject> m_stationPointerMap;
	private Dictionary<TaskManager.TaskType,GameObject> m_stationBubbleMap;

	private int m_currentTutorial;

	private const float TextBubbleHideY = -380;
	private const float TextBubbleShowY = -220f;
	private const float TextBubbleShowTime = .25f;
	private const float TextBubbleHideTime = .2f;
	
	// To test the text bubble anims
	private float m_lastTest = 0;
	private float m_testStep = 1.5f;

	public bool AbleToResume = true;
	private float m_pauseCounter = 0f;
	[SerializeField] private float m_pauseDelay;
	private int m_pauseState = 0;

	private bool m_countDown = false;
	private float m_counter = 0f;
	[SerializeField] private float m_countDownTime;

	[SerializeField] private ServeFoodManager m_serveFoodManager;
	

	// Use this for initialization
	void Start () 
	{
		// generate station pointing hand map
		m_stationPointerMap = new Dictionary<TaskManager.TaskType,GameObject> () 
		{
			{ TaskManager.TaskType.Stove, m_stovePointingHand },					//start: on stove started	finish: on burger done
			{ TaskManager.TaskType.Register, m_registerPointingHand },				//start: on order taken		finish: on order taken
			{ TaskManager.TaskType.Tray, m_trayPointingHand },						//start: on food picked up	finish: food placed on tray
			{ TaskManager.TaskType.Cleaning, m_cleaningPointingHand },				//start: body placed		finish: body cleaned up
			{ TaskManager.TaskType.Cutting, m_cuttingBoardPointingHand },			//start: cutting started	finish: cutting complete
			{ TaskManager.TaskType.Fryer, m_fryerPointingHand },					//start: fries placed		finish: fries done
			{ TaskManager.TaskType.Ice, m_icePointingHand },						//start: ice picked up		finish: ice placed in drink machine
			{ TaskManager.TaskType.Soda, m_sodaPointingHand },						//start: started filling	finish: drink done
			{ TaskManager.TaskType.IceCream, m_iceCreamPointingHand },				//start: icecream started	finish: ice cream done
			{ TaskManager.TaskType.Trash, m_trashPointingHand },					//start: trash deposited	finish: trash deposited
		};



		// add listener to game manager for day prep
		GameManagerUser.Instance.OnGamePrep.AddListener (OnGamePrep);
		GameManagerUser.Instance.OnGameStart.AddListener (OnGameStart);
		GameManagerUser.Instance.OnVictory.AddListener (OnDayOver);
		GameManagerUser.Instance.OnGameOver.AddListener (OnDayOver);
		WorkerManager.Instance.WorkerWrongFoodEvent.AddListener (OnWrongFood);
		WorkerManager.Instance.WorkerWrongPotatoesEvent.AddListener (OnWrongPotatoes);

		if (PlayerStatistics.Instance.RandomMode) {
			TutorialMode = false;
		}
	}

	private IEnumerator ShowTextBubble(bool pause)
	{
		if (!m_textBubbleShowing) {
			if (pause) {
				m_pauseDim.gameObject.SetActive (true);
			}
			LeanTween.moveLocalY(m_tutorialTextBubble, TextBubbleShowY, TextBubbleShowTime).setEase (LeanTweenType.easeOutBack).setUseEstimatedTime(true);
			yield return new WaitForSecondsRealtime (TextBubbleShowTime);
			if (pause) {
				PauseManager.Instance.ForcePause ();
			}
			m_textBubbleShowing = true;
		} else {
			yield return null;
		}
	}

	private IEnumerator HideTextBubble(bool unpause)
	{
		if (m_textBubbleShowing) {
			if (unpause) {
				PauseManager.Instance.ForceResume ();
			}
			LeanTween.moveLocalY (m_tutorialTextBubble, TextBubbleHideY, TextBubbleHideTime).setEase (LeanTweenType.easeInBack).setUseEstimatedTime(true);
			yield return new WaitForSecondsRealtime (TextBubbleHideTime);
			if (unpause) {
				m_pauseDim.gameObject.SetActive (false);
			}
			m_textBubbleShowing = false;
		} else {
			yield return null;
		}
	}

	private void OnGamePrep(BaseLevel level, bool restart)
	{
		Debug.Log ("current tutorial: " + level.LevelID);
	}

	private void OnGameStart(BaseLevel level, bool restart)
	{
		if (TutorialMode) {
			if (PlayerStatistics.Instance.CurrentLevel == m_burgerTutorialLevel) {
			
				//m_tutorialText.text = "Take the customer's order!\nIf you make them wait too long, they'll leave :(";
				//ShowTextBubble ();
			}

			if (PlayerStatistics.Instance.CurrentLevel == m_eatingTutorialLevel) {
				InputManager.Instance.LevelDisabledEating = true;
				CustomerManager.Instance.SpawnCustomer (Customer.CustomerType.Normal, new List<Food.FoodType> () { Food.FoodType.Burger });
			}
		}
			
	}

	public void OnFirstMove()
	{
		if (TutorialMode) {
			if (PlayerStatistics.Instance.CurrentLevel == m_burgerTutorialLevel) {
				if (!m_playerMoved) {
					StartPointer (TaskManager.TaskType.Register);
				}
			}

			if (PlayerStatistics.Instance.CurrentLevel == m_friesTutorialLevel) {
//				if (!m_playerMoved) {
//					StartPointer (TaskManager.TaskType.Register);
//				}
			}
		}

		m_playerMoved = true;
	}

	public void OnTaskStarted(TaskManager.TaskType station)
	{
		if (TutorialMode) {
			if (PlayerStatistics.Instance.CurrentLevel == m_burgerTutorialLevel) {
				if (station == TaskManager.TaskType.Register) {
					//HideTextBubble ();
					StopPointer (TaskManager.TaskType.Register);
				}
				else if (station == TaskManager.TaskType.Stove) {
					StopPointer (TaskManager.TaskType.Stove);
				} else if (station == TaskManager.TaskType.Tray) {
					StopPointer (TaskManager.TaskType.Stove);
					StartPointer (TaskManager.TaskType.Tray);
				}
			}

			if (PlayerStatistics.Instance.CurrentLevel == m_friesTutorialLevel) {
				if (station == TaskManager.TaskType.Tray) {
					StopPointer (TaskManager.TaskType.Fryer);
				} else if (station == TaskManager.TaskType.Fryer) {
					StopPointer (TaskManager.TaskType.Fryer);
				}
			}

			if (PlayerStatistics.Instance.CurrentLevel == m_drinkTutorialLevel) {
				if (station == TaskManager.TaskType.Ice) {
					StopPointer (TaskManager.TaskType.Ice);
					m_iceGrabbed = true;
					if (!m_iceLoaded) {
						m_sodaText.text = "Place ice in soda machine";
						StartPointer (TaskManager.TaskType.Soda);
					}
				} else if (station == TaskManager.TaskType.Soda) {
					StopPointer (TaskManager.TaskType.Soda);
					if (!m_sodaRestarted && m_sodaStarted) {
						m_sodaRestarted = true;
					}
					m_sodaStarted = true;
				} else if (station == TaskManager.TaskType.Tray) {
					StopPointer (TaskManager.TaskType.Soda);
					m_sodaGrabbed = true;
				}
			}
			
			if (PlayerStatistics.Instance.CurrentLevel == m_eatingTutorialLevel) {
				if (station == TaskManager.TaskType.Register) {
					//StartCoroutine(ShowTextBubble (true));
				}
			}

			if (station == TaskManager.TaskType.Ice) {
				StopPointer (TaskManager.TaskType.Ice);
			}
		}
	}

	public void OnTaskFinished(TaskManager.TaskType station)
	{
		if (TutorialMode) {
			if (PlayerStatistics.Instance.CurrentLevel == m_burgerTutorialLevel) {
				if (station == TaskManager.TaskType.Register || station == TaskManager.TaskType.Stove) {
					if (station == TaskManager.TaskType.Stove) {
						m_stoveText.text = "Grab the burger";
					}
					StartPointer (TaskManager.TaskType.Stove);
				} else if (station == TaskManager.TaskType.Tray) {
					StopPointer (TaskManager.TaskType.Tray);
				}
			}

			if (PlayerStatistics.Instance.CurrentLevel == m_eatingTutorialLevel) {
				if (station == TaskManager.TaskType.Cleaning) {
					StopPointer (TaskManager.TaskType.Cleaning);
					m_registerText.text = "Now you can keep working!";
					StartPointer (TaskManager.TaskType.Register);
					CustomerManager.Instance.SpawnCustomer (Customer.CustomerType.Normal, new List<Food.FoodType> () { Food.FoodType.Burger });
				} else if (station == TaskManager.TaskType.Tray) {
					if (MoneyManager.Instance.Money >= MoneyManager.Instance.Quota) {
						LevelManager.Instance.FinishDay ();
					}
				} else if (station == TaskManager.TaskType.Register) {
					StopPointer (TaskManager.TaskType.Register);
				}
			}

			if (PlayerStatistics.Instance.CurrentLevel == m_friesTutorialLevel) {
				if (station == TaskManager.TaskType.Cutting) {
					StopPointer (TaskManager.TaskType.Cutting);
					if (!m_learnedFries) {
						m_fryerText.text = "Place potatoes to start fryer";
						StartPointer (TaskManager.TaskType.Fryer);
					}
					m_friesCut = true;
				} else if (station == TaskManager.TaskType.Register) {
					StopPointer (TaskManager.TaskType.Register);
					if (m_serveFoodManager.FoodLeftInOrder.Contains(Food.FoodType.Fries) && !m_learnedFriesPause) {
						m_tutorialText.text = "Customers are ordering fries now!";
						StartCoroutine(ShowTextBubble (true));
						AbleToResume = false;
						m_pauseCounter = 0f;
						//ShowTextBubble ();
					}
				} else if (station == TaskManager.TaskType.Fryer) {
					if (!m_learnedFries) {
						m_fryerText.text = "Grab the fries";
						StartPointer (TaskManager.TaskType.Fryer);
						m_learnedFries = true;
					}
				} else if (station == TaskManager.TaskType.Tray) {
					StopPointer (TaskManager.TaskType.Tray);
				}
			}

			if (PlayerStatistics.Instance.CurrentLevel == m_eatRightTutorialLevel) {
				if (station == TaskManager.TaskType.Register) {
					if (m_customersPastRegister == 0 && !m_eatenAtRightWindow) {
						m_tutorialText.text = "Eat customers from the righthand window too!\nTry it by pressing E!";
						//ShowTextBubble ();
						m_trayText.text = "Pro Tip: Eat customers here too!";
						StartPointer (TaskManager.TaskType.Tray);
						m_counter = 0f;
						m_countDown = true;
					}
				} else if (station == TaskManager.TaskType.Cleaning) {
					StopPointer (TaskManager.TaskType.Cleaning);
				}
			}

			if (PlayerStatistics.Instance.CurrentLevel == m_drinkTutorialLevel) {
				if (station == TaskManager.TaskType.Register) {
					if (m_serveFoodManager.FoodLeftInOrder.Contains(Food.FoodType.Drink) && !m_learnedDrinkPause) {
						m_tutorialText.text = "You obtained a soda machine, so customers will order drinks now";
						m_drinkMask.SetActive (true);
						StartPauseSystem ();
					} else if (m_serveFoodManager.FoodLeftInOrder.Contains(Food.FoodType.Drink) && !m_sodaRestarted) {
						m_sodaText.text = "Ice already filled! Start the soda";
						StartPointer (TaskManager.TaskType.Soda);
					}
				} else if (station == TaskManager.TaskType.Ice) {
					m_iceLoaded = true;
					if (!m_sodaStarted) {
						m_sodaText.text = "SPACE to start soda";
						StartPointer (TaskManager.TaskType.Soda);
						//HideTextBubble ();
					}
				} else if (station == TaskManager.TaskType.Soda) {
					if (!m_sodaGrabbed) {
						m_sodaText.text = "Grab the soda";
						StartPointer (TaskManager.TaskType.Soda);
					}
				} 
			}

			// increment m_ordersTaken whenever an order is taken
			if (station == TaskManager.TaskType.Register) {
				m_customersPastRegister += 1;
			}

			if (station == TaskManager.TaskType.Cleaning) {
				StopPointer (TaskManager.TaskType.Cleaning);
			}
				
		}
	}

	public void OnEnergyEmpty()
	{
		if (TutorialMode) {
			if (PlayerStatistics.Instance.CurrentLevel == m_eatingTutorialLevel) {
				m_tutorialText.text = "Serving ONE FOOD ITEM takes ONE ENERGY,\nand your\nENERGY METER\njust ran out";
				m_energyBarMask.SetActive (true);
				StartCoroutine(ShowTextBubble (true));
				AbleToResume = false;
				m_pauseCounter = 0f;
				InputManager.Instance.LevelDisabledEating = false;
			}
		}
	}

	public void OnIceEmpty()
	{
		if (TutorialMode) {
			if (!PlayerStatistics.Instance.LearnedRefillIce) {
				m_iceText.text = "Soda machine needs ice!";
				StartPointer (TaskManager.TaskType.Ice);
				PlayerStatistics.Instance.LearnedRefillIce = true;
				SaveManager.Instance.SaveGame();
			}
		}
	}

	public void OnCustomerEaten()
	{
		if (TutorialMode) {
			if (PlayerStatistics.Instance.CurrentLevel == m_eatingTutorialLevel) {
				StopPointer (TaskManager.TaskType.Register);
				m_tutorialText.text = "Press SPACEBAR\nat the cleaning station\nto clean up\nthe body";
				m_cleaningText.text = "Press SPACE to clean the body.";
				StartPointer (TaskManager.TaskType.Cleaning);
			}

			if (PlayerStatistics.Instance.CurrentLevel == m_eatRightTutorialLevel) {
				if (!m_eatenAtRightWindow) {
					m_eatenAtRightWindow = true;
					StopPointer (TaskManager.TaskType.Tray);
					m_cleaningText.text = "You can also save it here for later!";
					StartPointer (TaskManager.TaskType.Cleaning);
				}
			}

			m_customerEaten = true;
			m_customersPastRegister += 1;
		}
	}

	// called when the day ends, and all the customers leave, but there's still a body left on the cleaning mat
	public void OnBodyLeftOver()
	{
		if (TutorialMode) {
			if (!m_bodyLeftOverActive) {
				m_bodyLeftOverActive = true;
				m_cleaningText.text = "Clean up the body!";
				StartPointer (TaskManager.TaskType.Cleaning);
			}
		}
	}

	public void OnCustomerLeft()
	{
		if (TutorialMode) {
			if (PlayerStatistics.Instance.CurrentLevel == m_eatingTutorialLevel) {
				CustomerManager.Instance.SpawnCustomer (Customer.CustomerType.Normal, new List<Food.FoodType> () { Food.FoodType.Burger });
			}

			m_customersPastRegister += 1;
		}
	}

	private void OnWrongFood(Food.FoodType food)
	{
		if (TutorialMode) {
			if (!PlayerStatistics.Instance.LearnedTrash) {
				m_tutorialText.text = "You're holding a food item that the current customer didn't order!";
				m_wrongFood = true;
				m_wrongFoodGrabbed = food;
				m_workerMask.SetActive (true);
				StartPauseSystem ();
			}
		}
	}

	private void OnWrongPotatoes()
	{
		if (TutorialMode) {
			if (!PlayerStatistics.Instance.LearnedPotatoes && !PlayerStatistics.Instance.PurchasedOneTime.Contains(2)) {
				m_tutorialText.text = "You finished cutting potatoes before serving your previous batch of fries";
				m_wrongPotatoes = true;
				m_workerMask.SetActive (true);
				StartPauseSystem ();
			}
		}
	}

	private void OnDayOver()
	{
		//HideTextBubble ();
	}

	private void Update()
	{
		if (TutorialMode) {
			if (PlayerStatistics.Instance.CurrentLevel == m_eatingTutorialLevel) {
				if (m_worker.CurrentTaskCollider.CurrentCollidingTask != null) {
					if (m_worker.CurrentTaskCollider.CurrentCollidingTask.Type == TaskManager.TaskType.Register
					    && !m_customerEaten && m_energyDepleted && !m_eatingPrompt.GetComponent<ButtonSpawn>().Active) {
						m_eatingPrompt.GetComponent<ButtonSpawn>().AnimateIn();
					} else if((m_worker.CurrentTaskCollider.CurrentCollidingTask.Type != TaskManager.TaskType.Register ||m_customerEaten || !m_energyDepleted) &&
					m_eatingPrompt.GetComponent<ButtonSpawn>().Active){
						m_eatingPrompt.GetComponent<ButtonSpawn>().AnimateOut();
					}


				} else if(m_eatingPrompt.GetComponent<ButtonSpawn>().Active){
					m_eatingPrompt.GetComponent<ButtonSpawn>().AnimateOut();
				}
					
			} else if (PlayerStatistics.Instance.CurrentLevel == m_eatRightTutorialLevel) {
				if (m_worker.CurrentTaskCollider.CurrentCollidingTask != null) {
					if (m_worker.CurrentTaskCollider.CurrentCollidingTask.Type == TaskManager.TaskType.Tray && !m_customerEaten
						&& m_serveFoodManager.CustomerIdForTask != -1 && !m_rightEatingPrompt.GetComponent<ButtonSpawn>().Active) {
						m_rightEatingPrompt.GetComponent<ButtonSpawn>().AnimateIn();
					} else if((m_worker.CurrentTaskCollider.CurrentCollidingTask.Type != TaskManager.TaskType.Tray || m_customerEaten
					          || m_serveFoodManager.CustomerIdForTask == -1) && m_rightEatingPrompt.GetComponent<ButtonSpawn>().Active) {
						m_rightEatingPrompt.GetComponent<ButtonSpawn>().AnimateOut();
					}
				} else if(m_rightEatingPrompt.GetComponent<ButtonSpawn>().Active){
					m_rightEatingPrompt.GetComponent<ButtonSpawn>().AnimateOut();
				}
			}

			// handle spacebar inputs when game is frozen for tutorial
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (AbleToResume && PauseManager.Instance.IsGamePaused) {
					m_proceedPrompt.GetComponent<ButtonSpawn>().AnimateOut();
					m_pauseState++;
					Proceed (m_pauseState);
					AbleToResume = false;
					m_pauseCounter = 0f;
				}
			}

			if (!AbleToResume) {
				m_pauseCounter += Time.unscaledDeltaTime;
				if (m_pauseCounter >= m_pauseDelay) {
					m_pauseCounter = 0f;
					AbleToResume = true;
					if (m_textBubbleShowing) {
						m_proceedPrompt.GetComponent<ButtonSpawn>().AnimateIn();
					}
				}
			}
			if (m_countDown) {
				m_counter += Time.deltaTime;
				if (m_counter >= m_countDownTime) {
					m_counter = 0f;
					m_countDown = false;
					OnCountDown ();
				}
			}
		}

//		if (Time.time - m_lastTest >= m_testStep)
//		{
//			m_lastTest = Time.time;
//			if (m_textBubbleShowing)
//			{
//				HideTextBubble();
//			}
//			else
//			{
//				ShowTextBubble();
//			}
//		}
	}

	private void Proceed(int pauseState) {
		if (TutorialMode) {
			Debug.Log ("pause state: " + pauseState);
			if (m_wrongFood) {
				switch (pauseState) {
				case 1:
					m_workerMask.SetActive (false);
					m_tutorialText.text = "Place it back on its food station by pressing SPACE";
					switch (m_wrongFoodGrabbed) {
					case Food.FoodType.Burger:
						m_stoveMask.SetActive (true);
						break;
					case Food.FoodType.Fries:
						m_fryerMask.SetActive (true);
						break;
					case Food.FoodType.Drink:
						m_drinkMask.SetActive (true);
						break;
					}
					break;
				case 2:
					m_stoveMask.SetActive (false);
					m_fryerMask.SetActive (false);
					m_drinkMask.SetActive (false);
					StartCoroutine (HideTextBubble (true));
					m_pauseState = 0;
					m_wrongFoodGrabbed = Food.FoodType.Undef;
					m_wrongFood = false;
					PlayerStatistics.Instance.LearnedTrash = true;
					SaveManager.Instance.SaveGame();
					break;
				default:
					m_tutorialText.text = "Uh-oh, there's been a problem";
					break;
				}
				return;
			}
			if (m_wrongPotatoes) {
				switch (pauseState) {
				case 1:
					m_workerMask.SetActive (false);
					m_tutorialText.text = "Place them back on the cuttingboard by pressing SPACE";
					m_cuttingMask.SetActive (true);
					break;
				case 2:
					m_cuttingMask.SetActive (false);
					m_tutorialText.text = "Then serve the current fries from the fryer before frying your new potatoes";
					m_fryerMask.SetActive (true);
					break;
				case 3:
					m_fryerMask.SetActive (false);
					StartCoroutine (HideTextBubble (true));
					m_pauseState = 0;
					m_wrongPotatoes = false;
					PlayerStatistics.Instance.LearnedPotatoes = true;
					SaveManager.Instance.SaveGame();
					break;
				default:
					m_tutorialText.text = "Uh-oh, there's been a problem";
					break;
				}
				return;
			}
			if (PlayerStatistics.Instance.CurrentLevel == m_eatingTutorialLevel) {
				switch (pauseState) {
				case 1:
					m_energyBarMask.SetActive (false);
					m_tutorialText.text = "To restore your energy, eat a customer from the window";
					m_registerMask.SetActive (true);
					break;
				case 2:
					m_registerMask.SetActive (false);
					m_tutorialText.text = "Then place the body on the cleaning mat to clean it up";
					m_cleaningMask.SetActive (true);
					break;
				case 3:
					m_cleaningMask.SetActive (false);
					m_tutorialText.text = "When you\nFINISH CLEANING\nyou'll get your energy back";
					m_energyBarMask.SetActive (true);
					break;
				case 4:
					m_energyBarMask.SetActive (false);
					StartCoroutine (HideTextBubble (true));
					m_registerText.text = "Eat here to get more energy";
					StartPointer (TaskManager.TaskType.Register);
					m_energyDepleted = true;
					CustomerManager.Instance.SpawnCustomer (Customer.CustomerType.Normal, new List<Food.FoodType> () { Food.FoodType.Burger });
					m_pauseState = 0;
					break;
				default:
					m_tutorialText.text = "Serving food takes energy, and your\nENERGY METER\njust ran out";
					break;
				}
			}

			if (PlayerStatistics.Instance.CurrentLevel == m_friesTutorialLevel) {
				switch (pauseState) {
				case 1:
					m_tutorialText.text = "First, chop potatoes at the cutting board";
					m_cuttingMask.SetActive (true);
					break;
				case 2:
					m_cuttingMask.SetActive (false);
					m_tutorialText.text = "Then drop the potatoes in the fryer to start cooking";
					m_fryerMask.SetActive (true);
					break;
				case 3:
					m_fryerMask.SetActive (false);
					m_tutorialText.text = "As usual, serve the fries at the pick-up window";
					m_serveFoodMask.SetActive (true);
					break;
				case 4:
					m_serveFoodMask.SetActive (false);
					StartCoroutine (HideTextBubble (true));
					m_learnedFriesPause = true;
					if (!m_friesCut) {
						m_cuttingBoardText.text = "Press SPACE\nto chop potatoes";
						StartPointer (TaskManager.TaskType.Cutting);
					}
					m_pauseState = 0;
					break;
				default:
					m_tutorialText.text = "Uh-oh, something went wrong!";
					break;
				}
			}
			if (PlayerStatistics.Instance.CurrentLevel == m_drinkTutorialLevel) {
				switch (pauseState) {
				case 1:
					m_drinkMask.SetActive (false);
					m_tutorialText.text = "First, grab ice from the ice machine";
					m_iceMask.SetActive (true);
					break;
				case 2:
					m_iceMask.SetActive (false);
					m_tutorialText.text = "Then drop the ice in the soda machine to refill it to max ice capacity";
					m_drinkMask.SetActive (true);
					break;
				case 3:
					m_tutorialText.text = "Then press SPACE to start filling a drink";
					break;
				case 4:
					m_iceMask.SetActive (false);
					StartCoroutine (HideTextBubble (true));
					m_learnedDrinkPause = true;
					if (!m_iceGrabbed) {
						m_iceText.text = "Grab ice for soda machine";
						StartPointer (TaskManager.TaskType.Ice);
					}
					m_pauseState = 0;
					break;
				default:
					m_tutorialText.text = "Uh-oh, something went wrong!";
					break;
				}
			}
		}
	}

	private void StartPauseSystem() {
		StartCoroutine(ShowTextBubble (true));
		AbleToResume = false;
		m_pauseCounter = 0f;
	}

	private void OnCountDown() {
		if (TutorialMode) {
			if (PlayerStatistics.Instance.CurrentLevel == m_eatRightTutorialLevel) {
				StopPointer (TaskManager.TaskType.Tray);
			}
		}
	}

	// call this to call attention to a certain station via pointing hand
	public void StartPointer(TaskManager.TaskType station) 
	{
		m_stationPointerMap [station].SetActive (true);
	}

	// call this to deactivate the pointing hand on a station
	public void StopPointer(TaskManager.TaskType station)
	{
		m_stationPointerMap [station].GetComponent<BounceButton>().Disable();
	}
}
