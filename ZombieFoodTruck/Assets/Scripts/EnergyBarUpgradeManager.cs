using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBarUpgradeManager : MonoBehaviourSingleton<EnergyBarUpgradeManager>
{

	[SerializeField] private bool m_isUpgrade;

	public bool IsUpgrade
	{
		get { return m_isUpgrade; }
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int GetMaxEnergy()
	{
		return 6;
	}
}
