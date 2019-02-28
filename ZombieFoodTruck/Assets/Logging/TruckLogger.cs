using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckLogger : MonoBehaviourSingleton<TruckLogger> {

	protected LoggingManager m_loggingManager;

	[SerializeField] protected bool m_isDebugging;

	[SerializeField] private int m_eatCustomerAction;
	[SerializeField] private int m_takeOrderAction;
	[SerializeField] private int m_startStoveAction;
	[SerializeField] private int m_grabFoodAction;
	[SerializeField] private int m_serveFoodAction;
	[SerializeField] private int m_startCuttingAction;
	[SerializeField] private int m_finishCuttingAction;
	[SerializeField] private int m_startFryerAction;
	[SerializeField] private int m_getIceAction;
	[SerializeField] private int m_depositIceAction;
	[SerializeField] private int m_startDrinkAction;
	[SerializeField] private int m_startIceCreamAction;
	[SerializeField] private int m_meltIceCreamAction;
	[SerializeField] private int m_useTrashAction;
	[SerializeField] private int m_startCleaningAction;
	[SerializeField] private int m_finishCleaningAction;
	[SerializeField] private int m_pressSpacebarAction;
	[SerializeField] private int m_victoryAction;
	[SerializeField] private int m_gameOverAction;
	[SerializeField] private int m_purchaseStove;
	[SerializeField] private int m_purchaseFryer;
	[SerializeField] private int m_purchaseDrinkMachine;
	[SerializeField] private int m_purchaseCleaningMat;
	[SerializeField] private int m_purchaseIceCapacity;
	[SerializeField] private int m_purchaseFoodTier;
	[SerializeField] private int m_purchasePatienceTier;
	[SerializeField] private int m_purchaseCleaningTier;
	[SerializeField] private int m_purchaseMovementSpeed;
	[SerializeField] private int m_purchaseExtraTip;
	[SerializeField] private int m_purchaseAutoCut;
	[SerializeField] private int m_purchaseHat;
	[SerializeField] private int m_purchaseIncognito;

	[SerializeField] private GameObject m_worker;

	private void Awake()
	{
		// obtain reference to logging manager
		var go = GameObject.FindWithTag("LoggingManager");
		if (go != null) {
			m_loggingManager = go.GetComponent<LoggingManager> ();
		}
		m_isDebugging = DebugManager.Instance.DebugMode;
	}

	private void Start()
	{
		GameManagerUser.Instance.OnVictory.AddListener(OnVictory);
		GameManagerUser.Instance.OnGameOver.AddListener (OnGameOver);
		CustomerManager.Instance.OnCustomerEaten.AddListener (OnCustomerEaten);
	}

	public void OnGamePrep(BaseLevel level, bool restart)
	{
		if (restart)
			return;
		if (!m_isDebugging) {
			Debug.Log ("recording the start of a level!");
			m_loggingManager.RecordLevelStart (level.LevelID);
		}
	}

	public void OnGameStart(BaseLevel level, bool restart)
	{
		
	}

	public void OnGameOver()
	{
		string money = "" + MoneyManager.Instance.Money;
		string detail = money;
		Debug.Log ("logging gameover: " + detail);
		Debug.Log ("gameover id" + m_gameOverAction);
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_gameOverAction, detail);
		}
	}

	public void OnVictory()
	{
		string money = "" + MoneyManager.Instance.Money;
		string detail = money;
		Debug.Log ("logging victory: " + detail);
		Debug.Log ("victory id: " + m_victoryAction);
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_victoryAction, detail);
			m_loggingManager.RecordLevelEnd ();
		}
	}

	private void OnCustomerEaten() 
	{
		string energy = "" + EatingMechanicManager.Instance.GetEnergy();
		Debug.Log ("eating customer. player at energy " + energy);
		string timeOfDay = "" + LevelManager.Instance.TimeOfDay;
		Debug.Log ("eat customer, at time: " + timeOfDay);
		string detail = energy + "," + timeOfDay;
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_eatCustomerAction, detail);
		}
	}

	public void LogOrderTaken()
	{
		Debug.Log ("logging order");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_takeOrderAction);
		}
	}

	public void LogStoveStarted()
	{
		Debug.Log ("logging stove start");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_startStoveAction);
		}
	}

	public void LogFoodGrabbed()
	{
		Debug.Log ("logging food grabbed");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_grabFoodAction);
		}
	}

	public void LogFoodServed()
	{
		Debug.Log ("logging food served");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_serveFoodAction);
		}
	}

	// called when the player STARTS or RESUMES cutting at the cutting board
	public void LogResumeCutting()
	{
		Debug.Log ("logging cutting resumed");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_startCuttingAction);
		}
	}

	public void LogFinishCutting()
	{
		Debug.Log ("logging cutting finished");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_finishCuttingAction);
		}
	}

	public void LogStartFryer()
	{
		Debug.Log ("logging fryer started");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_startFryerAction);
		}
	}

	public void  LogGetIce()
	{
		Debug.Log ("logging ice picked up");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_getIceAction);
		}
	}

	public void LogDepositIce()
	{
		Debug.Log ("logging ice deposited");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_depositIceAction);
		}
	}

	public void LogStartDrink()
	{
		Debug.Log ("logging drink started");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_startDrinkAction);
		}
	}

	public void LogStartIceCream()
	{
		Debug.Log ("logging ice cream started");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_startIceCreamAction);
		}
	}

	public void LogIceCreamMelted()
	{
		Debug.Log ("logging ice cream melted");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_meltIceCreamAction);
		}
	}

	public void LogUseTrash()
	{
		Debug.Log ("logging used trash");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_useTrashAction);
		}
	}

	public void LogStartCleaning()
	{
		Debug.Log ("logging start cleaning");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_startCleaningAction);
		}
	}

	public void LogFinishCleaning()
	{
		Debug.Log ("logging finish cleaning");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_finishCleaningAction);
		}
	}

	// called whenever the player presses spacebar. records the player's current position in the actionDetail
	public void LogPressSpacebar()
	{
		float x = m_worker.transform.position.x;
		float y = m_worker.transform.position.y;
		string position = "(" + x + "," + y + ")";
		Debug.Log ("logging space pressed at " + position);
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_pressSpacebarAction, position);
		}
	}

	// called when player purchases the second stove
	public void LogStovePurchased() 
	{
		Debug.Log ("logging new stove purchased");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseStove);
		}
	}

	// called when player purchases the second fryer
	public void LogFryerPurchased()
	{
		Debug.Log ("logging new fryer purchased");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseFryer);
		}
	}

	// called when player purchases the second drink machine
	public void LogDrinkMachinePurchased()
	{
		Debug.Log ("logging new drink machine purchased");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseDrinkMachine);
		}
	}

	// called when player purchases the second cleaning mat
	public void LogCleaningMatPurchased()
	{
		Debug.Log ("logging new cleaning mat purchased");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseCleaningMat);
		}
	}

	// called when player purchases ice capacity
	public void LogIcePurchased()
	{
		Debug.Log ("logging ice capacity purchased");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseIceCapacity);
		}
	}

	// called when a new food prep speed tier is purchased
	// int tier is the new tier the player purchased
	public void LogFoodTierPurchased(int tier)
	{
		string tierString = "" + tier;
		Debug.Log ("logging food tier purchased: " + tierString);
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseFoodTier, tierString);
		}
	}
		
	// called when a new patience boost tier is purchased
	// int tier is the new tier the player purchased
	public void LogPatienceTierPurchased(int tier)
	{
		string tierString = "" + tier;
		Debug.Log ("logging patience tier purchased: " + tierString);
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchasePatienceTier, tierString);
		}

	}
	
	// called when a new cleaning speed tier is purchased
	// int tier is the new tier the player purchased
	public void LogCleaningTierPurchased(int tier)
	{
		string tierString = "" + tier;
		Debug.Log ("logging cleaning tier purchased: " + tierString);
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseCleaningTier, tierString);
		}
	}
	// called when player purchases movement speed
	public void LogMovementPurchased()
	{
		Debug.Log ("logging movement upgrade purchased");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseMovementSpeed);
		}
	}
	// called when player purchases Extra tip
	public void LogExtraTipPurchased()
	{
		Debug.Log ("logging extra tip purchased");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseExtraTip);
		}
	}
	// called when player purchases auto cutting
	public void LogAutoCutPurchased()
	{
		Debug.Log ("logging auto cutting purchased");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseAutoCut);
		}
	}
	
	// called when player purchases chef's hat
	public void LogHatPurchased()
	{
		Debug.Log ("logging Chef's hat purchased");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseHat);
		}
	}
	
	// called when player purchases incognito mode
	public void LogIncognitoPurchased()
	{
		Debug.Log ("logging incognito mode purchased");
		if (!m_isDebugging) {
			m_loggingManager.RecordEvent (m_purchaseIncognito);
		}
	}
	
}
