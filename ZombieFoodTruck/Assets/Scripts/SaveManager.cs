using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviourSingleton<SaveManager>
{

	[Serializable]
	private class SaveFile
	{
		public int Money = 0;
		public int Level = 0;

		// Tiered upgrades
		public int PatienceTier = 0;
		public int FoodPrepTier = 0;
		public int CleaningTier = 0;
		public int IceTier = 0;

//		public Dictionary<String, bool> OneTimeUpgrades = new Dictionary<string, bool>()
//		{
//			{"ExtraStove", false},
//			{"ExtraFryer", false},
//			{"ExtraSodaMachine", false},
//			{"ExtraCleaningMat", false},
//		};
		
		public List<int> OneTimeUpgrades = new List<int>();

		// Tutorials
		public bool LearnedTrash = false;
		public bool LearnedPotatoes = false;
		public bool LearnedRefillIce = false;

		// Random Mode
		public bool UnlockedRandomMode = false;
	}
	
	
	[SerializeField] private string m_saveKey;

	private SaveFile m_activeFile = new SaveFile();

	public bool UnlockedRandomMode
	{
		get 
		{
			if (HasSaveData ()) {
				Debug.Log("FOUND SAVE DATA");
				string serializedFile = PlayerPrefs.GetString(m_saveKey);
				JsonUtility.FromJsonOverwrite(serializedFile, m_activeFile);
				return m_activeFile.UnlockedRandomMode;
			}
			return false;
		}
	}

	
	public void SaveGame()
	{
		Debug.Log("SAVING GAME");
		
		m_activeFile.Level = PlayerStatistics.Instance.CurrentLevel;
		m_activeFile.Money = PlayerStatistics.Instance.CurrentMoney;
		
		m_activeFile.PatienceTier = PlayerStatistics.Instance.PatienceTier;
		m_activeFile.FoodPrepTier = PlayerStatistics.Instance.FoodPrepTier;
		m_activeFile.CleaningTier = PlayerStatistics.Instance.CleaningTier;
//		m_activeFile.IceTier = PlayerStatistics.Instance.IceTier;

		m_activeFile.OneTimeUpgrades.Clear();
		foreach (var upgrade in PlayerStatistics.Instance.PurchasedOneTime)
		{
			m_activeFile.OneTimeUpgrades.Add(upgrade);
		}

		m_activeFile.LearnedTrash = PlayerStatistics.Instance.LearnedTrash;
		m_activeFile.LearnedPotatoes = PlayerStatistics.Instance.LearnedPotatoes;
		m_activeFile.LearnedRefillIce = PlayerStatistics.Instance.LearnedRefillIce;

		m_activeFile.UnlockedRandomMode = PlayerStatistics.Instance.UnlockedRandomMode;
		
		string serializedFile = JsonUtility.ToJson(m_activeFile);
		PlayerPrefs.SetString(m_saveKey, serializedFile);
		PlayerPrefs.Save();
		
		Debug.Log(serializedFile);
		Debug.Log("SERIALIZED SAVE DATA");
	}

	public void LoadGame()
	{
		Debug.Log("LOADING GAME");
		
		if (PlayerPrefs.HasKey(m_saveKey))
		{
			Debug.Log("FOUND SAVE DATA");
			
			string serializedFile = PlayerPrefs.GetString(m_saveKey);
			JsonUtility.FromJsonOverwrite(serializedFile, m_activeFile);
			
			Debug.Log(serializedFile);
			Debug.Log("DESERIALIZED SAVE DATA");

			PlayerStatistics.Instance.CurrentLevel = m_activeFile.Level;
//			GameManagerUser.Instance.LoadCurrentLevel();
			PlayerStatistics.Instance.CurrentMoney = m_activeFile.Money;

			PlayerStatistics.Instance.LearnedTrash = m_activeFile.LearnedTrash;
			PlayerStatistics.Instance.LearnedPotatoes = m_activeFile.LearnedPotatoes;
			PlayerStatistics.Instance.LearnedRefillIce = m_activeFile.LearnedRefillIce;

			PlayerStatistics.Instance.UnlockedRandomMode = m_activeFile.UnlockedRandomMode;
		}
	}

	public void ClearSaveData()
	{
		Debug.Log("CLEARING GAME DATA");

		PlayerStatistics.Instance.CurrentLevel = 0;
		PlayerStatistics.Instance.CurrentMoney = 0;

		PlayerStatistics.Instance.PatienceTier = 0;
		PlayerStatistics.Instance.FoodPrepTier = 0;
		PlayerStatistics.Instance.CleaningTier = 0;
		//		m_activeFile.IceTier = PlayerStatistics.Instance.IceTier;

		PlayerStatistics.Instance.PurchasedOneTime.Clear();

		PlayerStatistics.Instance.LearnedTrash = false;
		PlayerStatistics.Instance.LearnedPotatoes = false;
		PlayerStatistics.Instance.LearnedRefillIce = false;

		PlayerStatistics.Instance.UnlockedRandomMode = false;
		Debug.Log ("unlocked random mode: " + PlayerStatistics.Instance.UnlockedRandomMode);

		SaveGame ();
	}

	public void LoadUpgradeStuff()
	{
		UpgradeManager.Instance.SetPatienceTier(m_activeFile.PatienceTier);
		UpgradeManager.Instance.SetFoodPrepTier(m_activeFile.FoodPrepTier);
		UpgradeManager.Instance.SetCleaningTier(m_activeFile.CleaningTier);
//			UpgradeManager.Instance.SetIceTier(m_activeFile.IceTier);

		UpgradeManager.Instance.LoadOneTimeUpgrades(m_activeFile.OneTimeUpgrades);
	}

	public bool HasSaveData()
	{
		return PlayerPrefs.HasKey(m_saveKey);
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
