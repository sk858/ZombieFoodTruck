using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatienceUpgrade : Upgrade {

	public float PatienceBoost;

	private Upgrade m_patienceUpgrade;

	[SerializeField] 
	private Customer RetrieveCustomer;
	
	[SerializeField] 
	public int[] m_patienceCostArray;

	[SerializeField] 
	private float[] m_patienceTierEffect;
	
	public void Init(float patienceBoost, int upgradeId)
	{
		
		PatienceBoost = patienceBoost;
		m_upgradeId = upgradeId;

	}
	public override void ApplyUpgrade()
	{

		int CurrentPatienceTier = PlayerStatistics.Instance.PatienceTier;
		float PatienceTierEffect = m_patienceTierEffect[CurrentPatienceTier];
		float getPatience = RetrieveCustomer.Patience;
		Debug.Log(getPatience);
		CustomerManager.Instance.BoostPatience(getPatience*PatienceTierEffect);
		Debug.Log("Patience Upgrade Applied");
		
		
	}
	
	public override bool IsApplied()
	{
		return CustomerManager.Instance.IsUpgradeApplied(this);
	}

	public override void UpgradeTier()
	{
		int nextTierIndex = PlayerStatistics.Instance.PatienceTier;
		UpgradeManager.Instance.PurchaseUpgrade(m_patienceCostArray[nextTierIndex]);
		PlayerStatistics.Instance.PatienceTier += 1;
		UpgradeManager.Instance.PatienceUpClicked();
		Debug.Log("PatienceTier upgraded");
		SaveManager.Instance.SaveGame();
		TruckLogger.Instance.LogCleaningTierPurchased (PlayerStatistics.Instance.PatienceTier);
		if (PlayerStatistics.Instance.PatienceTier == 3 || m_patienceCostArray[PlayerStatistics.Instance.PatienceTier] >
		    PlayerStatistics.Instance.CurrentMoney)
		{
			EventSystemManager.Instance.ResetSelection();
		}
	}
}
