using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimePurchaseUpgrade : MonoBehaviourSingleton<OneTimePurchaseUpgrade>
{

	[SerializeField] public int NewStoveId;

	[SerializeField] public int NewFryerId;

	[SerializeField] public int NewDrinkId;

	[SerializeField] public int NewCleaningId;

	[SerializeField] public int NewIceId;

	[SerializeField] public int NewMovementId;

	[SerializeField] public int NewTipChanceId;
	
	[SerializeField] public int NewAutoCutId;
	
	[SerializeField] public int NewHatId;

	[SerializeField] public int NewIncognitoId;

	[SerializeField] public int StoveCost;

	[SerializeField] public int FryerCost;

	[SerializeField] public int DrinkCost;

	[SerializeField] public int CleaningCost;
	
	[SerializeField] public int IceCost;
	
	[SerializeField] public int MovementCost;

	[SerializeField] public int TipChanceCost;
	
	[SerializeField] public int AutoCutCost;
	
	[SerializeField] public int HatCost;

	[SerializeField] public int IncognitoCost;
//	
//	[SerializeField] private DrinkManager m_iceUpdate;
//	
//	[SerializeField] private int m_newIceLimit;

	private void Start()
	{
		
	}

	public void PurchaseStoveOnce()
	{
		UpgradeManager.Instance.PurchaseUpgrade(StoveCost);
		PlayerStatistics.Instance.PurchasedOneTime.Add(NewStoveId);
		UpgradeManager.Instance.StoveUpClicked();
		//Button dissappears changes to fully upgraded button
		SaveManager.Instance.SaveGame();
		TruckLogger.Instance.LogStovePurchased ();
		EventSystemManager.Instance.ResetSelection();
	}
	
	public void PurchaseFryerOnce()
	{
		UpgradeManager.Instance.PurchaseUpgrade(FryerCost);
		PlayerStatistics.Instance.PurchasedOneTime.Add(NewFryerId);
		UpgradeManager.Instance.FryerUpClicked();
		//Button dissappears
		SaveManager.Instance.SaveGame();
		TruckLogger.Instance.LogFryerPurchased ();
		EventSystemManager.Instance.ResetSelection();
	}
	
	public void PurchaseDrinkOnce()
	{
		//Buying function here
		UpgradeManager.Instance.PurchaseUpgrade(DrinkCost);
		PlayerStatistics.Instance.PurchasedOneTime.Add(NewDrinkId);
		UpgradeManager.Instance.DrinkUpClicked();
		//Button dissappears
		SaveManager.Instance.SaveGame();
		TruckLogger.Instance.LogDrinkMachinePurchased ();
		EventSystemManager.Instance.ResetSelection();
	}
	
	public void PurchaseCleaningOnce()
	{
		//Buying function here
		UpgradeManager.Instance.PurchaseUpgrade(CleaningCost);
		PlayerStatistics.Instance.PurchasedOneTime.Add(NewCleaningId);
		UpgradeManager.Instance.MatUpClicked();
		//Button dissappears
		SaveManager.Instance.SaveGame();
		TruckLogger.Instance.LogCleaningMatPurchased ();
		EventSystemManager.Instance.ResetSelection();
	}

	public void PurchaseIceOnce()
	{
		UpgradeManager.Instance.PurchaseUpgrade(IceCost);
		PlayerStatistics.Instance.PurchasedOneTime.Add(NewIceId);
		UpgradeManager.Instance.IceUpClicked();
		SaveManager.Instance.SaveGame();
		TruckLogger.Instance.LogIcePurchased ();
		EventSystemManager.Instance.ResetSelection();
	}

	public void PurchaseMovementOnce()
	{
		UpgradeManager.Instance.PurchaseUpgrade(MovementCost);
		PlayerStatistics.Instance.PurchasedOneTime.Add(NewMovementId);
		UpgradeManager.Instance.MovementUpClicked();
		SaveManager.Instance.SaveGame();
		TruckLogger.Instance.LogMovementPurchased ();
		EventSystemManager.Instance.ResetSelection();
	}
	
	public void PurchaseAutoCutOnce()
	{
		UpgradeManager.Instance.PurchaseUpgrade(AutoCutCost);
		PlayerStatistics.Instance.PurchasedOneTime.Add(NewAutoCutId);
		UpgradeManager.Instance.AutoCutUpClicked();
		SaveManager.Instance.SaveGame();
		TruckLogger.Instance.LogAutoCutPurchased ();
		EventSystemManager.Instance.ResetSelection();
	}
	
	public void PurchaseHatOnce()
	{
		UpgradeManager.Instance.PurchaseUpgrade(HatCost);
		PlayerStatistics.Instance.PurchasedOneTime.Add(NewHatId);
		UpgradeManager.Instance.HatUpClicked();
		SaveManager.Instance.SaveGame();
		TruckLogger.Instance.LogHatPurchased ();
		EventSystemManager.Instance.ResetSelection();
	}

	public void PurchaseTipChance()
	{
		UpgradeManager.Instance.PurchaseUpgrade(TipChanceCost);
		PlayerStatistics.Instance.PurchasedOneTime.Add(NewTipChanceId);
		UpgradeManager.Instance.TipChanceUpClicked();
		TruckLogger.Instance.LogExtraTipPurchased();
		SaveManager.Instance.SaveGame();
		EventSystemManager.Instance.ResetSelection();

	}

	public void PurchaseIncognito()
	{
		UpgradeManager.Instance.PurchaseUpgrade(IncognitoCost);
		PlayerStatistics.Instance.PurchasedOneTime.Add(NewIncognitoId);
		UpgradeManager.Instance.IncognitoUpClicked();
		TruckLogger.Instance.LogIncognitoPurchased();
		SaveManager.Instance.SaveGame();
		EventSystemManager.Instance.ResetSelection();

	}
	
}
