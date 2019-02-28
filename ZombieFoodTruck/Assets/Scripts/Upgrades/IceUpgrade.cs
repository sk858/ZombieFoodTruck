using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IceUpgrade : Upgrade {

	public float IceIncreasedBy;

	private Upgrade m_iceUpgrade;

	[SerializeField] private DrinkManager m_iceUpdate;

	public int maxIceTier;

	public int maxIceLimit;

	
	public void Init(int IceIncreasedBy, int upgradeId)
	{
//		if (maxIceLimit - initialIceLimt < maxIceTier)
//		{
//			throw new Exception();
//		}
		IceIncreasedBy = IceIncreasedBy;
		m_upgradeId = upgradeId;
	}
	
	public override void ApplyUpgrade()
	{
		var curRatio = (float)PlayerStatistics.Instance.IceTier / maxIceTier;
		var newIceLimit = (int) Math.Round(curRatio * maxIceLimit);
		m_iceUpdate.IncreaseIceLimit(newIceLimit);
		
		Debug.Log("Increased by" + ((float)m_iceUpdate.m_iceLimit/newIceLimit));
		
		m_iceUpdate.IncreaseIceLimit(m_iceUpdate.m_iceLimit + 2);
		
		int GetIceLimit = m_iceUpdate.m_iceLimit;
		if (PlayerStatistics.Instance.IceTier == 1)
		{
			if (!IsApplied())
			{
				m_iceUpdate.AddStationUpgrade(this);
				int NewIceLimit = (int) Math.Round(GetIceLimit * 1.2f);
				m_iceUpdate.IncreaseIceLimit(NewIceLimit);
				Debug.Log("Ice Applied");	
			}	
		}
		
		if (PlayerStatistics.Instance.IceTier == 2)
		{
			if (!IsApplied())
			{
				m_iceUpdate.AddStationUpgrade(this);
				int NewIceLimit = (int) Math.Round(GetIceLimit * 1.4f);
				m_iceUpdate.IncreaseIceLimit(NewIceLimit);
				Debug.Log("Ice Applied");	
			}	
		}
		if (PlayerStatistics.Instance.IceTier == 3)
		{
			if (!IsApplied())
			{
				m_iceUpdate.AddStationUpgrade(this);
				int NewIceLimit = (int) Math.Round(GetIceLimit * 1.6f);
				m_iceUpdate.IncreaseIceLimit(NewIceLimit);
				Debug.Log("Ice Applied");	
			}	
		}	
		
	}
	
	public override bool IsApplied()
	{
		Debug.Log("applied");
		return m_iceUpdate.StationsUpgradeApplied(this);
	}

	private int IceUpgradeAllowedMinLevel = 3;

	public override void UpgradeTier()
	{
		PlayerStatistics.Instance.IceTier += 1;
		
//		if (PlayerStatistics.Instance.CurrentLevel < IceUpgradeAllowedMinLevel)
//		{
//			throw LevelTooLowException("Ice Upgrade is not allowed at level" + PlayerStatistics.Instance.CurrentLevel);
//		}
		
		
		if (PlayerStatistics.Instance.CurrentLevel >= 3 && PlayerStatistics.Instance.IceTier == 0)
		{
			//BuyUpgrade function here
			PlayerStatistics.Instance.IceTier = 1;
		}
		if (PlayerStatistics.Instance.CurrentLevel >= 5 && PlayerStatistics.Instance.IceTier == 1)
		{
			//BuyUpgrade function here
			PlayerStatistics.Instance.IceTier = 2;
		}
		if (PlayerStatistics.Instance.CurrentLevel >= 7 && PlayerStatistics.Instance.IceTier == 2)
		{
			//BuyUpgrade function here
			PlayerStatistics.Instance.IceTier = 3;
		}
	}
}
