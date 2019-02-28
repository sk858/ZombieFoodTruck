using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviourSingleton<EventSystemManager>
{

	private EventSystem m_eventSystem;

	[SerializeField]
	private GameObject m_upgradePanel;
	
	// Use this for initialization
	void Start ()
	{
		m_eventSystem = GetComponent<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ResetSelection()
	{
		m_eventSystem.SetSelectedGameObject(m_eventSystem.firstSelectedGameObject);
	}
}
