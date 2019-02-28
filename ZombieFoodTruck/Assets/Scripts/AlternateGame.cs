using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateGame : MonoBehaviour
{

	private int m_currentButton;

	[SerializeField]
	private KeyCode[] m_keys;
	
	[SerializeField] 
	private TaskManager m_taskManager;

	[SerializeField]
	private float m_amtProgressOnSmash;
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(m_keys[m_currentButton]))
		{
			m_currentButton = (m_currentButton + 1) % m_keys.Length;
			m_taskManager.AddProgress(m_amtProgressOnSmash);
		}
	}
}
