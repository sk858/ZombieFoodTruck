using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{

	[SerializeField] private GameObject m_pausePanel;

	[SerializeField] private GameObject m_pauseDim;

	public void TogglePause()
	{
		PauseManager pauseManager = PauseManager.Instance;
		if (pauseManager.IsGamePaused)
		{
			if (pauseManager.Resume())
			{
				m_pausePanel.GetComponent<PanelSpawn>().AnimateOut(0f, () => m_pauseDim.SetActive(false));
			}
		}
		else
		{
			if (pauseManager.Pause())
			{
				m_pausePanel.gameObject.SetActive(true);
				m_pausePanel.GetComponent<PanelSpawn>().AnimateIn(0f);
				m_pauseDim.SetActive(true);
			}
		}
	}

	public void OnUpgradeScreenDone()
	{
	//	GetComponent<Canvas>().sortingOrder = 1;
	}
}
