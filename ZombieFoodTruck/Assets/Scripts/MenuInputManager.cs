using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputManager : MonoBehaviourSingleton<MenuInputManager>
{

	private bool m_inUpgradeScreen;

	private bool m_inMainMenu;

	private bool m_inStart;

	private bool m_inWinLossScreen;

	private bool m_didWin;

	private bool m_inGame;

	private bool m_timing;

	private float m_timer;

	[SerializeField] private float m_delayTime;

	[SerializeField] private StartPanel m_startPanel;

	[SerializeField] private PauseButton m_pauseButton;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_timing) {
			m_timer += Time.unscaledDeltaTime;
			if (m_timer >= m_delayTime) {
				m_timer = 0f;
				m_timing = false;
			}
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			
		}
		
		if (Input.GetKeyDown(KeyCode.P) && m_inGame && !EndingSequenceManager.Instance.Started)
		{
			m_pauseButton.TogglePause();
		}
		
		if (Input.GetKeyDown(KeyCode.Space) && !m_timing)
		{
			if (m_inStart)
			{
				m_startPanel.StartLevel();
				m_inStart = false;
			}
			else if (m_inWinLossScreen && !EndingSequenceManager.Instance.Started)
			{
				GameManagerUser.Instance.GoToUpgradeScreen(m_didWin);
				m_inWinLossScreen = false;	
			}
			else if (m_inUpgradeScreen)
			{
				
			}
			else if (m_inMainMenu)
			{
				
			}
		}
		
	}

	public void OnStartEnter()
	{
		StartCoroutine(Delay());
	}

	public void OnStartExit()
	{
		m_inStart = false;
		m_inGame = true;
	}

	public void OnWinLossEnter(bool didWin)
	{
		m_didWin = didWin;
		m_inWinLossScreen = true;
		m_timing = true;
		m_inGame = false;
	}
	
	public void OnWinLossExit()
	{
		m_inWinLossScreen = false;
	}

	IEnumerator Delay()
	{
		yield return new WaitForSeconds(0.5f);
		m_inStart = true;
	}
}
