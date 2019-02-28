using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
	[SerializeField] private PanelSpawn m_childPanel;

	[SerializeField] private ButtonSpawn[] m_buttonSpawns;
	
	// Use this for initialization
	void Start () {
	//	gameObject.SetActive(false);
	}
	
	// Start the level
	public void StartLevel()
	{
		GameManagerUser.Instance.BeginNextDay();
		MenuInputManager.Instance.OnStartExit();
		GetComponent<PanelSpawn>().AnimateOut(0.25f, UIAnimationStartManager.Instance.AnimateIn);
	}

	public void OnUpgradeScreenDone()
	{
		MenuInputManager.Instance.OnStartEnter();
		m_childPanel.AnimateIn(0.5f, 0f, SpawnButtons);
	}

	public void SpawnButtons()
	{
		for (int i = 0; i < m_buttonSpawns.Length; i++)
		{
			m_buttonSpawns[i].AnimateIn();
		}
	}
}
