using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Logger : MonoBehaviour {
	
	protected LoggingManager m_loggingManager;

	[SerializeField] protected bool m_isDebugging;
	
	protected void Awake()
	{
		// obtain reference to logging manager
		var go = GameObject.FindWithTag("LoggingManager");
		m_loggingManager = go.GetComponent<LoggingManager>();
	}
	
}
