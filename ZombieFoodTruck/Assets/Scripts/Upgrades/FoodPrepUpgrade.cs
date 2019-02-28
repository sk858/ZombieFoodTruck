using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodPrepUpgrade : Upgrade {

	public float SpeedBoostedBy;
	
	private Upgrade m_stoveUpgrade;
	
	[SerializeField] 
	public int[] m_foodPrepCostArray;

	[SerializeField] 
	private float[] m_stoveTierEffect;
	
	[SerializeField] 
	private float[] m_fryerTierEffect;
	
	[SerializeField] 
	private float[] m_drinkTierEffect;
	
	[SerializeField] 
	private float[] m_cuttingTierEffect;
	
	
	public void Init(int speedBoostedBy, int upgradeId)
	{
		SpeedBoostedBy = speedBoostedBy;
		m_upgradeId = upgradeId;
	}
	
	public override void ApplyUpgrade()
	{
		int CurrentFoodTier = PlayerStatistics.Instance.FoodPrepTier;
		float stoveTierEffect = m_stoveTierEffect[CurrentFoodTier];
		float fryerTierEffect = m_fryerTierEffect[CurrentFoodTier];
		float drinkTierEffect = m_drinkTierEffect[CurrentFoodTier];
		float cuttingTierEffect = m_cuttingTierEffect[CurrentFoodTier];
		
		
//		FoodTaskUpgradeHelper.Instance.UpgradeAll(1f,1f,1f,1f,1f,this);
		FoodTaskUpgradeHelper.Instance.UpgradeAll(stoveTierEffect, drinkTierEffect, fryerTierEffect
		, 0, cuttingTierEffect, this);
		Debug.Log("Food Prep Applied");	
		Debug.Log(FoodTaskUpgradeHelper.Instance.GetFryerSpeed());
		
	}
	
	public override bool IsApplied()
	{
		Debug.Log("applied");
		return FoodTaskUpgradeHelper.Instance.IsApplied();
	}

	public override void UpgradeTier()
	{
		int nextTierIndex = PlayerStatistics.Instance.FoodPrepTier;
		UpgradeManager.Instance.PurchaseUpgrade(m_foodPrepCostArray[nextTierIndex]);
		PlayerStatistics.Instance.FoodPrepTier += 1;
		UpgradeManager.Instance.FoodPrepUpClicked();
		Debug.Log("FoodTier upgraded");
		SaveManager.Instance.SaveGame();
		TruckLogger.Instance.LogFoodTierPurchased (PlayerStatistics.Instance.FoodPrepTier);
		if (PlayerStatistics.Instance.FoodPrepTier == 3 || m_foodPrepCostArray[PlayerStatistics.Instance.FoodPrepTier] >
		    PlayerStatistics.Instance.CurrentMoney)
		{
			EventSystemManager.Instance.ResetSelection();
		}
	}
	
}
