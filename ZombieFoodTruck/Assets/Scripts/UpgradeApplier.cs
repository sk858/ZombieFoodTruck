using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeApplier : MonoBehaviour {
	
	[SerializeField] private Upgrade[] m_upgradeScripts;

	private Dictionary<int, Upgrade> m_upgradeMap = new Dictionary<int, Upgrade>();
	

	private void Start()
	{
		// create the upgrade map
		foreach (Upgrade u in m_upgradeScripts)
		{
			m_upgradeMap.Add(u.UpgradeId, u);
		}

		Upgrade upgrade = m_upgradeMap [PlayerStatistics.Instance.ActiveUpgrade];
		if (!upgrade.IsApplied())
		{
			upgrade.ApplyUpgrade();
			Debug.Log("Upgrade applied!");
		}
			
	}
	
}
