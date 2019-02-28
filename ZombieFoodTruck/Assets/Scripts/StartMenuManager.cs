using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{

	[SerializeField] private string m_truckScene;

	[SerializeField] private GameObject m_continueButton;
	[SerializeField] private GameObject m_newGameButton;
	[SerializeField] private GameObject m_newGameOnlyButton;
	[SerializeField] private GameObject m_randomModeButton;

	private LoggingManager m_loggingManager;
	
	public void StartLevel()
	{
		SceneManager.LoadScene(m_truckScene);
	}

	public void ContinueGame()
	{
		SaveManager.Instance.LoadGame();
		StartLevel();
	}

	public void RandomGame()
	{
		SaveManager.Instance.LoadGame ();
		PlayerStatistics.Instance.RandomMode = true;
		StartLevel ();
	}

	public void NewGame()
	{
		SaveManager.Instance.ClearSaveData ();
		StartLevel();
	}

	private void Start()
	{
		
	}

}
