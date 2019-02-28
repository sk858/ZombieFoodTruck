using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLogger : Logger
{
	
	// Initializes the logging manager and records the page load (only on the first time the app starts up)
	
	private int gameId = 438;
	[SerializeField] private int m_versionId;
	
	private static bool m_isInitialized = false;
	
	private void Start () {
		// initialize logging manager and record page load
		if (!m_isInitialized)
		{
			Debug.Log("Initializing logging system.");
			m_loggingManager.Initialize(gameId, m_versionId, m_isDebugging);
			m_loggingManager.RecordPageLoad();
			//int abtestvalue = m_loggingManager.assignABTestValue (Random.Range (0, 2));
			//PlayerStatistics.Instance.ABTestingValue = abtestvalue;
			//Debug.Log ("abtest value: " + abtestvalue);
			//m_loggingManager.RecordABTestValue ();
			m_isInitialized = true;
		}
	}
	
}
