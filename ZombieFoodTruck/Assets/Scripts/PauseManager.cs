using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviourSingleton<PauseManager> {

	private bool m_paused = false;

	private bool m_forcePaused;

	[SerializeField]
	private GameObject m_pauseButton;

	[SerializeField]
	private Button m_muteButton;

	[SerializeField] 
	private Button m_restartButton;

	public bool IsGamePaused
	{
		get { return m_paused; }
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ForcePause()
	{
		Time.timeScale = 0;
		m_paused = true;
		m_forcePaused = true;
		Debug.Log ("game paused");
	}

	public void ForceResume()
	{
		Time.timeScale = 1;
		m_paused = false;
		m_forcePaused = false;
		Debug.Log ("game unpaused");
	}
	
	public bool Pause()
	{
		if (m_forcePaused)
		{
			return false;
		}
		// TODO Show pause menu
		Time.timeScale = 0;
		m_paused = true;
		Debug.Log ("game paused");
		return true;
	}

	public bool Resume()
	{
		if (m_forcePaused)
		{
			return false;
		}
		// TODO Hide pause menu
		Time.timeScale = 1;
		m_paused = false;
		Debug.Log ("game unpaused");
		return true;
	}

	public void OnUpgradeScreenDone()
	{
	//	m_pauseButton.SetActive(true);
		m_pauseButton.GetComponent<PauseButton>().OnUpgradeScreenDone();
		m_muteButton.interactable = true;
		m_restartButton.interactable = true;
	}
	
}
