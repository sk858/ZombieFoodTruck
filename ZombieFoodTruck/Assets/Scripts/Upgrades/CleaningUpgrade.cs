using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningUpgrade : Upgrade {

	public float SpeedIncreasedBy;
	
	private Upgrade m_cleaningUpgrade;
	
	[SerializeField]
	public int[] m_cleaningCostArray;

	[SerializeField] 
	private float[] m_cleaningTierEffect;
	
	public void Init(int speedIncreasedBy, int upgradeId)
	{
		SpeedIncreasedBy = speedIncreasedBy;
		m_upgradeId = upgradeId;
	}
	
	public override void ApplyUpgrade()
	{
		Worker w = WorkerManager.Instance.SelectedWorker;
		int currentTier = PlayerStatistics.Instance.CleaningTier;
		
		float getCleaningSpeed = w.GetSpeed((TaskManager.TaskType.Cleaning));
		float newCleaningSpeed = getCleaningSpeed + (getCleaningSpeed* m_cleaningTierEffect[currentTier]);
		w.SetSpeed(TaskManager.TaskType.Cleaning, newCleaningSpeed);
		w.AddUpgrade(this);
		Debug.Log(w.GetSpeed(TaskManager.TaskType.Cleaning));
		CustomerManager.Instance.SetCleaningProgressBar(PlayerStatistics.Instance.CleaningTier);
		Debug.Log("CleaningApplied");
			
	}
	
	public override bool IsApplied()
	{
		Worker w = WorkerManager.Instance.GetWorker(0);
		return w.IsUpgradeApplied(this);
	}
	
	
	public override void UpgradeTier()
	{
		int nextTierIndex = PlayerStatistics.Instance.CleaningTier;
		UpgradeManager.Instance.PurchaseUpgrade(m_cleaningCostArray[nextTierIndex]);
		PlayerStatistics.Instance.CleaningTier += 1;
		UpgradeManager.Instance.CleaningUpClicked();
		Debug.Log("CleaningTier upgraded");
		SaveManager.Instance.SaveGame();
		TruckLogger.Instance.LogCleaningTierPurchased (PlayerStatistics.Instance.CleaningTier);
		if (PlayerStatistics.Instance.CleaningTier == 3 || m_cleaningCostArray[PlayerStatistics.Instance.CleaningTier] >
		    PlayerStatistics.Instance.CurrentMoney)
		{
			EventSystemManager.Instance.ResetSelection();
		}
	}
}
