using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviourSingleton<PlayerStatistics>
{
	// the A/B testing version that this player is currently playing
	// -1 means undefined, 0 means "Time Bar", 1 means "Number Customers Remaining"
	public int ABTestingValue = -1;

	// the level the player is currently playing or is about to play
	public int CurrentLevel = 0;
	
	// the max level the player has reached so far
	public int MaxLevelReached = 0;

	// whether the player is restarting the current level
	public bool Restarting = false;

	// whether the player is currently playing the random level
	public bool RandomMode = false;

	// whether the player has unlocked random mode
	public bool UnlockedRandomMode = false;
	
	// the money the player currently has in the bank
	public int CurrentMoney = 0;
	
	// the total amount of money the player has ever earned
	public int TotalEarned = 0;
	
	// map of level id to high score for that level
	public List<int> HighScores = new List<int>();

	// whether the player has had the tutorial on putting back food yet
	public bool LearnedTrash = false;

	// whether the player has had the putting potatoes back tutorial yet
	public bool LearnedPotatoes = false;

	// whether the player has had the refill ice tutorial yet
	public bool LearnedRefillIce = false;
	
	// the upgrades that the player currently has active
	public int ActiveUpgrade = -1;

	// the upgrades the player currently has unlocked in the upgrade store
	public List<int> UnlockedUpgrades = new List<int>();

	[SerializeField] private Upgrade m_previousUpgrade;
	
	// keep one time purchase upgrades
	public List<int> PurchasedOneTime = new List<int>();
	
	//Tiers for each upgrade
	public int CleaningTier;

	public int PatienceTier;

	public int FoodPrepTier;

	public int IceTier;

	public Upgrade PreviousUpgrade
	{
		get
		{
			return m_previousUpgrade;
		}
		set
		{
			m_previousUpgrade = value;
		}
	}

	private void Awake()
	{
	//	DontDestroyOnLoad(gameObject);
		if (DebugManager.Instance.DebugMode) {
			RandomMode = DebugManager.Instance.RandomMode;
			if (RandomMode) {
				CurrentLevel = 100;
			} else {
				CurrentLevel = DebugManager.Instance.DebugStartLevel;
			}
		}
	}

	void Update()
	{
	//	Debug.Log(PreviousUpgrade);
		
	}
	
}
