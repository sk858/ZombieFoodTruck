using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
	[SerializeField]
	protected int m_upgradeId;

	[SerializeField] 
	protected int m_cost;

	public int UpgradeId
	{
		get
		{
			return m_upgradeId;
		}
	}

	public virtual int Cost
	{
		get { return m_cost; }
		set { m_cost = value; }

	}

	public abstract void ApplyUpgrade();

	public abstract void UpgradeTier();
	
	public abstract bool IsApplied();
}
