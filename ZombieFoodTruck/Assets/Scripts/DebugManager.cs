	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviourSingleton<DebugManager> {

	[SerializeField] private bool m_debugMode;
	[SerializeField] private bool m_randomMode;
	[SerializeField] private int m_debugStartLevel;

	public bool DebugMode
	{
		get { return m_debugMode; }
	}

	public bool RandomMode
	{
		get { return m_randomMode; }
	}

	public int DebugStartLevel
	{
		get { return m_debugStartLevel; }
	}

}
