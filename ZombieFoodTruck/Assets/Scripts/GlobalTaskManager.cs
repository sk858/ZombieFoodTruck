using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTaskManager : MonoBehaviourSingleton<GlobalTaskManager>
{

	[SerializeField]
	private TaskManager m_stoveManager;
	
	[SerializeField]
	private TaskManager m_registerManager;

	[SerializeField]
	private TaskManager m_cleaningManager;

	public TaskManager StoveManager
	{
		get
		{
			return m_stoveManager;
		}
	}

	public TaskManager RegisterManager
	{
		get
		{
			return m_registerManager;
		}
	}

	public TaskManager CleaningManager
	{
		get
		{
			return m_cleaningManager;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
