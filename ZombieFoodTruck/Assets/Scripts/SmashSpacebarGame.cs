using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashSpacebarGame : MonoBehaviour
{

	[SerializeField] 
	private TaskManager m_taskManager;

	[SerializeField]
	private float m_amtProgressOnSmash;
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			m_taskManager.AddProgress(m_amtProgressOnSmash);
		}
	}
}
