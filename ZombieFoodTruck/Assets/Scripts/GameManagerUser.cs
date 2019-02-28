using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManagerUser : MonoBehaviourSingleton<GameManagerUser>
{
	
	// EVENT CLASSES //
	public class VictoryEvent : UnityEvent{ };
	
	// int: the number of the level started, bool: whether the level is restarting (so true if restarting same level,
	// and false if starting the next level)
	public class GameStartEvent : UnityEvent<BaseLevel, bool>{ };

	public class GameOverEvent : UnityEvent{ };

	public class GamePrepEvent : UnityEvent<BaseLevel, bool>{ };
	
	
	// EVENT INSTANCES //	
	public VictoryEvent OnVictory = new VictoryEvent();
		
	public GameStartEvent OnGameStart = new GameStartEvent();
	
	public GameOverEvent OnGameOver = new GameOverEvent();

	public GamePrepEvent OnGamePrep = new GamePrepEvent();

	
	// LEVEL INFO //
	private int m_currentLevelNum;

	private BaseLevel m_currentLevelPrefab;

	[SerializeField] private GameObject[] m_levelList;

	[SerializeField] private GameObject m_randomLevel;
	
	[SerializeField] private string m_startScreen; // scene containing the start screen

	[SerializeField] private string m_upgradeScreen; // scene containing upgrade screen

	[SerializeField] private string m_truckScreen; // the truck scene where the game happens

	// Level Started? //

	private bool m_levelStarted = false;

	public bool LevelStarted
	{
		get { return m_levelStarted; }
	}

	public int NumberLevels 
	{
		get { return m_levelList.Length; }
	}

	private bool m_beatTheGame = false;

	public void LoadCurrentLevel()
	{
		// get current level number from PlayerStatistics
		m_currentLevelNum = PlayerStatistics.Instance.CurrentLevel;

		Debug.Log ("Current Level: " + m_currentLevelNum);
		
		// check if current level number is valid
		if (m_currentLevelNum > m_levelList.Length && !PlayerStatistics.Instance.RandomMode)
		{
			throw new IndexOutOfRangeException("Level " + m_currentLevelNum + " doesn't exist! Change " +
			                                   "current level num on the game manager to a valid level.");
		}
		
		// update current level prefab accordingly
		if (PlayerStatistics.Instance.RandomMode) {
			m_currentLevelPrefab = m_randomLevel.GetComponent<BaseLevel>();
		} else {
			m_currentLevelPrefab = m_levelList [m_currentLevelNum].GetComponent<BaseLevel> ();
			Debug.Log ("level prefab: " + m_currentLevelPrefab.LevelID);
		}

		// invoke the gameprepevent to prepare the level
		bool isRestarting = PlayerStatistics.Instance.Restarting;
		OnGamePrep.Invoke(m_currentLevelPrefab, isRestarting);
		TruckLogger.Instance.OnGamePrep(m_currentLevelPrefab, isRestarting);
	}
	
	private void Start()
	{
		LoadCurrentLevel();
	}
	
	// Returns the next level in levelList, if one exists. Else returns null.
	private BaseLevel GetNextLevel()
	{
		int nextNum = m_currentLevelNum + 1;
		if (nextNum < m_levelList.Length)
			return m_levelList[nextNum].GetComponent<BaseLevel>();
		else
			return null;
	}
	
	// Called at the end of the day. Invokes a victory event depending on how much money the player earned today.
	// Banks the player's earnings from the day.
	public void OnDayEnd()
	{
		Debug.Log("The day has ended.");		
		m_levelStarted = false;
		// update the player's money
		MoneyManager mm = MoneyManager.Instance;
		PlayerStatistics.Instance.CurrentMoney += mm.Money;
		PlayerStatistics.Instance.TotalEarned += mm.Money;
		
		// decide whether player has beaten the level
		if (mm.Money >= m_currentLevelPrefab.Quota)
		{
			// set restarting to false
			PlayerStatistics.Instance.Restarting = false;

			if (m_currentLevelNum + 1 < m_levelList.Length && !PlayerStatistics.Instance.RandomMode) {
				// if there exists a next level, obtain reference to its prefab
				m_currentLevelPrefab = GetNextLevel ();
				m_currentLevelNum += 1;
			} else {
				// then the player beat the game
				m_beatTheGame = true;
				// unlock random mode
				PlayerStatistics.Instance.UnlockedRandomMode = true;
			}
			
			OnVictory.Invoke();
		}
		else
		{
			// set restarting to true
			PlayerStatistics.Instance.Restarting = true;

			OnGameOver.Invoke();
		}
		
		PlayerStatistics.Instance.CurrentLevel = m_currentLevelNum;
		if (m_currentLevelNum > PlayerStatistics.Instance.MaxLevelReached)
			PlayerStatistics.Instance.MaxLevelReached = m_currentLevelNum;
		
		SaveManager.Instance.SaveGame();
	}

	public void RestartDay()
	{
		PlayerStatistics.Instance.Restarting = true;
		PauseManager.Instance.Resume ();
		SceneManager.LoadScene (m_truckScreen);
	}

	// Invoke the OnGameStart event to begin the day.
	public void BeginNextDay()
	{
		m_levelStarted = true;
		if (!PlayerStatistics.Instance.Restarting) {
			Debug.Log ("Starting next day");
			OnGameStart.Invoke (m_currentLevelPrefab, false);
			TruckLogger.Instance.OnGameStart(m_currentLevelPrefab, false);
		} else {
			Debug.Log ("Restarting day");
			OnGameStart.Invoke (m_currentLevelPrefab, true);
			TruckLogger.Instance.OnGameStart(m_currentLevelPrefab, true);
		}
	}

	public void GoToUpgradeScreen(bool win)
	{

		// update the current level and the max level reached by the player
//		PlayerStatistics.Instance.CurrentLevel = m_currentLevelNum;
//		if (m_currentLevelNum > PlayerStatistics.Instance.MaxLevelReached)
//			PlayerStatistics.Instance.MaxLevelReached = m_currentLevelNum;
//		
//		SaveManager.Instance.SaveGame();

		if (m_beatTheGame) {
			GameMusicManager.Instance.StopMusic ();
			Destroy (GameMusicManager.Instance.gameObject);
			PlayerStatistics.Instance.RandomMode = false;
			SceneManager.LoadScene (m_startScreen);
		} else {
			SceneManager.LoadScene (m_truckScreen);
		}
	}
	
}
