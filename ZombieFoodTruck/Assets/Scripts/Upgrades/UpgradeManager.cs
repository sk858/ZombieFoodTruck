using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviourSingleton<UpgradeManager>
{
	[SerializeField]
	private Upgrade[] m_allUpgrades;

	[SerializeField] private int m_cleaningUpgradeId = 3;

	[SerializeField] private int m_patienceUpgradeId = 2;

	[SerializeField] private int m_foodUpgradeId = 1;

	[SerializeField] private string m_truckScene;

	[SerializeField] private Upgrade[] m_upgradeScripts;

	[SerializeField] private GameObject m_upgradeScreen;

	[SerializeField] private Button m_continueButton;

	[SerializeField] private FoodPrepUpgrade m_foodprepUp;

	[SerializeField] private PatienceUpgrade m_patienceUp;

	[SerializeField] private CleaningUpgrade m_cleaningUp;
	
	[SerializeField] private int m_maxCleaningTiers;
	
	[SerializeField] private int m_maxFoodPrepTiers;
	
	[SerializeField] private int m_maxPatienceTiers;
	
	[SerializeField] private int fryerUnlockedlevel;
	
	[SerializeField] private int iceUnlockedlevel;
	
	[SerializeField] private int drinkUnlockedlevel;
	
	[SerializeField] private DrinkManager m_iceUpdate;
	
	[SerializeField] private int[] m_IceLimit;

	[SerializeField] public float[] m_MovementSpeed;

	[SerializeField] private float[] m_patienceIncrease;
	
	public GameObject fryerlockedicon;

	public GameObject icelockedicon;

	public GameObject drinklockedicon;

	public GameObject cuttingLockedIcon;

	public GameObject NewStove;

	public GameObject NewFryer;

	public GameObject NewDrink;

	public GameObject NewCleaning;

	public GameObject WallWithBadge;

	private Dictionary<int, Upgrade> m_upgradeMap = new Dictionary<int, Upgrade>();

	private Upgrade m_activeUpgrade;

	public Text CostForUpgrade;

	public Text Description;

	public Text ItemDesc;

	public Text CurrentBank;

	public Text UpgradeTitle;

	public Text TierText;

	public Slider TierBar;

	public GameObject FoodFullyUpgradedTier;
	
	public GameObject CleaningFullyUpgradedTier;
	
	public GameObject PatienceFullyUpgradedTier;

	public GameObject StoveAlreadyBought;
	
	public GameObject FryerAlreadyBought;
	
	public GameObject IceAlreadyBought;
	
	public GameObject DrinkAlreadyBought;
	
	public GameObject MatAlreadyBought;

	public GameObject MovementAlreadyBought;

	public GameObject AutoCutAlreadyBought;

	public GameObject ExtraTipAlreadyBought;

	public GameObject IncognitoAlreadyBought;

	public GameObject HatAlreadyBought;
	
	public GameObject FoodPicture;

	public Button FoodButton;

	public GameObject CleaningPicture;

	public Button CleaningButton;

	public GameObject PatiencePicture;

	public Button PatienceButton;

	public GameObject StovePicture;

	public Button StoveButton;

	public GameObject FryerPicture;

	public Button FryerButton;

	public GameObject IcePicture;

	public Button IceButton;

	public GameObject DrinkPicture;

	public Button DrinkButton;

	public GameObject MatPicture;

	public Button MatButton;

	public GameObject MovementPicture;

	public Button MovementButton;
	
	public GameObject ChanceTipPicture;

	public Button ChanceTipButton;
	
	public GameObject AutoCutPicture;

	public Button AutoCutButton;
	
	public GameObject IncognitoPicture;

	public Button IncognitoButton;
	
	public GameObject HatPicture;

	public Button HatButton;

	public Button NewFryerButton;

	public Button NewDrinkButton;

	public Button NewIceButton;

	public Button NewMovementButton;

	public Button NewChanceTipButton;

	public Button NewAutoCutButton;

	public Button NewIncognitoButton;

	public Button NewHatButton;
	

	private void Start()
	{
		SaveManager.Instance.LoadUpgradeStuff();

		// don't let them use cutting board yet!
		//cuttingLockedIcon.gameObject.SetActive(true);
		//NewAutoCutButton.interactable = false;
		cuttingLockedIcon.gameObject.SetActive(false);
		NewAutoCutButton.interactable = true;
		if (PlayerStatistics.Instance.CurrentLevel > fryerUnlockedlevel)
		{
			fryerlockedicon.gameObject.SetActive(false);
		}
		else
		{
			NewFryerButton.gameObject.SetActive(false);
		}
		if (PlayerStatistics.Instance.CurrentLevel > drinkUnlockedlevel)
		{
			drinklockedicon.gameObject.SetActive(false);
			NewDrinkButton.interactable = true;
		}
		else
		{
			NewDrinkButton.gameObject.SetActive(false);
		}
		if (PlayerStatistics.Instance.CurrentLevel > iceUnlockedlevel)
		{
			icelockedicon.gameObject.SetActive(false);
			NewIceButton.interactable = true;
		}
		else
		{
			NewIceButton.gameObject.SetActive(false);
		}
		NewStove.gameObject.SetActive(false);
		NewFryer.gameObject.SetActive(false);
		NewDrink.gameObject.SetActive(false);
		NewCleaning.gameObject.SetActive(false);
		WallWithBadge.gameObject.SetActive(false);
		
		m_continueButton.interactable = false;
		CurrentBank.text = "" + PlayerStatistics.Instance.CurrentMoney;
		UpgradeTitle.text = "";
		Description.text = "";
		ItemDesc.text = "";
		CostForUpgrade.text = "";
		TierBar.gameObject.SetActive(false);
		TierBar.maxValue = m_maxPatienceTiers;
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		StoveAlreadyBought.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		
		// create the upgrade map
		foreach (Upgrade u in m_upgradeScripts)
		{
			m_upgradeMap.Add(u.UpgradeId, u);
		}
		Debug.Log(PlayerStatistics.Instance.PreviousUpgrade);

		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
	}


	public void FoodPrepUpClicked()
	{
		UpgradeTitle.text = "Food Preparation Speed Upgrade";
		Description.text = "Next tier will decrease prep time by 1 second";
		ItemDesc.text = "";
		FoodPicture.gameObject.SetActive(true);
		FoodButton.gameObject.SetActive(true);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		
		int currentTier = PlayerStatistics.Instance.FoodPrepTier;
		int cost = m_foodprepUp.m_foodPrepCostArray[currentTier];
		CostForUpgrade.text = "$"+cost;
		TierBar.gameObject.SetActive(true);
		TierBar.value = currentTier;
		TierText.text = currentTier + "/" + m_maxFoodPrepTiers;
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		StoveAlreadyBought.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		
	
		if (currentTier >= m_maxFoodPrepTiers)
		{
			FoodButton.gameObject.SetActive(false);
			FoodFullyUpgradedTier.gameObject.SetActive(true);
			Description.text = "You have reached the max tier!";
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			FoodButton.interactable = false;
		}
	}
	
	public void PatienceUpClicked()
	{
		UpgradeTitle.text = "Customer Patience Upgrade";
		Description.text = "Next tier will increase customer patience by " + m_patienceIncrease[PlayerStatistics.Instance.PatienceTier]*100 + "%";
		ItemDesc.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(true);
		PatienceButton.gameObject.SetActive(true);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		int currentTier = PlayerStatistics.Instance.PatienceTier;
		int cost = m_patienceUp.m_patienceCostArray[currentTier];
		CostForUpgrade.text = "$"+cost;
		TierBar.gameObject.SetActive(true);
		TierBar.value = currentTier;
		TierText.text = currentTier + "/" + m_maxPatienceTiers;
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		StoveAlreadyBought.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		if (currentTier >= m_maxPatienceTiers)
		{
			PatienceButton.gameObject.SetActive(false);
			PatienceFullyUpgradedTier.gameObject.SetActive(true);
			Description.text = "You have reached the max tier!";
			
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			PatienceButton.interactable = false;
		}
	}
	
	public void CleaningUpClicked()
	{
		UpgradeTitle.text = "Body Cleaning Speed Upgrade";
		Description.text = "Next tier will increase cleaning speed by " + m_patienceIncrease[PlayerStatistics.Instance.CleaningTier]*100 + "%";
		Debug.Log(PlayerStatistics.Instance.CleaningTier);
		ItemDesc.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(true);
		CleaningButton.gameObject.SetActive(true);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		int currentTier = PlayerStatistics.Instance.CleaningTier;
		int cost = m_cleaningUp.m_cleaningCostArray[currentTier];
		CostForUpgrade.text = "$"+cost;
		TierBar.gameObject.SetActive(true);
		TierBar.value = currentTier;
		TierText.text = currentTier + "/" + m_maxCleaningTiers;
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		StoveAlreadyBought.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		if (currentTier >= m_maxCleaningTiers)
		{
			CleaningButton.gameObject.SetActive(false);
			CleaningFullyUpgradedTier.gameObject.SetActive(true);
			Description.text = "You have reached the max tier!";
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			CleaningButton.interactable = false;
		}
	}
	public void StoveUpClicked()
	{
		UpgradeTitle.text = "Add a new stove";
		ItemDesc.text = "Buy another stove once!";
		Description.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(true);
		StoveButton.gameObject.SetActive(true);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		TierBar.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		int cost = OneTimePurchaseUpgrade.Instance.StoveCost;
		CostForUpgrade.text = "$"+cost;
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewStoveId))
		{
			StoveButton.gameObject.SetActive(false);
			StoveAlreadyBought.gameObject.SetActive(true);
			ItemDesc.text = "You already purchased this!";
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			StoveButton.interactable = false;
		}
	}
	
	public void FryerUpClicked()
	{
		UpgradeTitle.text = "Add a new Fryer";
		ItemDesc.text = "Buy another fryer once!";
		Description.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(true);
		FryerButton.gameObject.SetActive(true);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		TierBar.gameObject.SetActive(false);
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		StoveAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		int cost = OneTimePurchaseUpgrade.Instance.FryerCost;
		CostForUpgrade.text = "$"+cost;
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewFryerId))
		{
			FryerButton.gameObject.SetActive(false);
			FryerAlreadyBought.gameObject.SetActive(true);
			ItemDesc.text = "You already purchased this!";
		}
		
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			FryerButton.interactable = false;
		}
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewStoveId))
		{
			StoveButton.interactable = false;
		}

		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewFryerId))
		{
			FryerButton.interactable = false;
		}

		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewDrinkId))
		{
			DrinkButton.interactable = false;
		}

		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewCleaningId))
		{
			CleaningButton.interactable = false;
		}

		if (CheckOneTimePurchased((OneTimePurchaseUpgrade.Instance.NewIceId)))
		{
			IceButton.interactable = false;
		}
	}
	
	public void IceUpClicked()
	{
		UpgradeTitle.text = "Increase Ice Capacity";
		ItemDesc.text = "Buy to increase ice capacity once!";
		Description.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(true);
		IceButton.gameObject.SetActive(true);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		TierBar.gameObject.SetActive(false);
		int cost = OneTimePurchaseUpgrade.Instance.IceCost;
		CostForUpgrade.text = "$"+cost;
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		StoveAlreadyBought.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewIceId))
		{
			IceButton.gameObject.SetActive(false);
			IceAlreadyBought.gameObject.SetActive(true);
			ItemDesc.text = "You already purchased this!";
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			IceButton.interactable = false;
		}
	}
	
	public void DrinkUpClicked()
	{
		UpgradeTitle.text = "Add a new drink machine";
		ItemDesc.text = "Buy another drink machine once!";
		Description.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(true);
		DrinkButton.gameObject.SetActive(true);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		TierBar.gameObject.SetActive(false);
		int cost = OneTimePurchaseUpgrade.Instance.DrinkCost;
		CostForUpgrade.text = "$"+ cost ;
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		StoveAlreadyBought.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewDrinkId))
		{
			DrinkButton.gameObject.SetActive(false);
			DrinkAlreadyBought.gameObject.SetActive(true);
			ItemDesc.text = "You already purchased this!";
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			DrinkButton.interactable = false;
		}
	}
	
	public void MatUpClicked()
	{
		UpgradeTitle.text = "Add a cleaning mat";
		ItemDesc.text = "Buy another cleaning mat once!";
		Description.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(true);
		MatButton.gameObject.SetActive(true);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		
		TierBar.gameObject.SetActive(false);
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		StoveAlreadyBought.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		int cost = OneTimePurchaseUpgrade.Instance.CleaningCost;
		CostForUpgrade.text = "$"+cost;
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewCleaningId))
		{
			MatButton.gameObject.SetActive(false);
			MatAlreadyBought.gameObject.SetActive(true);
			ItemDesc.text = "You already purchased this!";
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			MatButton.interactable = false;
		}
	}

	public void MovementUpClicked()
	{
		UpgradeTitle.text = "Upgrade movement speed";
		ItemDesc.text = "Movement speed will be increased by " + (m_MovementSpeed[1] - m_MovementSpeed[0])/m_MovementSpeed[0] * 100 +"%";
		Description.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(true);
		MovementButton.gameObject.SetActive(true);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		
		TierBar.gameObject.SetActive(false);
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		
		int cost = OneTimePurchaseUpgrade.Instance.MovementCost;
		CostForUpgrade.text = "$"+cost;
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewMovementId))
		{
			MovementButton.gameObject.SetActive(false);
			MovementAlreadyBought.gameObject.SetActive(true);
			ItemDesc.text = "You already purchased this!";
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			MovementButton.interactable = false;
		}
		
	}

	public void TipChanceUpClicked()
	{
		UpgradeTitle.text = "Chance to earn extra tip";
		ItemDesc.text =  "You get " + CustomerManager.Instance.GetChanceTip() + "% chance of earning extra $" + CustomerManager.Instance.GetExtraTipAmount()+ 
			" per each order.";
		Description.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(true);
		ChanceTipButton.gameObject.SetActive(true);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		
		TierBar.gameObject.SetActive(false);
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		
		int cost = OneTimePurchaseUpgrade.Instance.TipChanceCost;
		CostForUpgrade.text = "$"+cost;
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewTipChanceId))
		{
			ChanceTipButton.gameObject.SetActive(false);
			ExtraTipAlreadyBought.gameObject.SetActive(true);
			ItemDesc.text = "You already purchased this!";
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			ChanceTipButton.interactable = false;
		}
	}
	
	public void AutoCutUpClicked()
	{
		UpgradeTitle.text = "Automatic Potato Cutting Board";
		ItemDesc.text = "Potato will be cut automatically once you put a potato in a fryer";
		Description.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(true);
		AutoCutButton.gameObject.SetActive(true);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		
		TierBar.gameObject.SetActive(false);
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		
		int cost = OneTimePurchaseUpgrade.Instance.AutoCutCost;
		CostForUpgrade.text = "$"+cost;
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewAutoCutId))
		{
			AutoCutButton.gameObject.SetActive(false);
			AutoCutAlreadyBought.gameObject.SetActive(true);
			ItemDesc.text = "You already purchased this!";
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			AutoCutButton.interactable = false;
		}
	}

	public void IncognitoUpClicked()
	{
		UpgradeTitle.text = "Incognito Eating Mode";
		ItemDesc.text = "You bribed the police officer and got a special police badge. Customers will no longer run away when you eat a customer.";
		Description.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(true);
		IncognitoButton.gameObject.SetActive(true);
		HatPicture.gameObject.SetActive(false);
		HatButton.gameObject.SetActive(false);
		
		TierBar.gameObject.SetActive(false);
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		
		int cost = OneTimePurchaseUpgrade.Instance.IncognitoCost;
		CostForUpgrade.text = "$"+cost;
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewIncognitoId))
		{
			IncognitoButton.gameObject.SetActive(false);
			IncognitoAlreadyBought.gameObject.SetActive(true);
			ItemDesc.text = "You already purchased this!";
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			IncognitoButton.interactable = false;
		}
	}

	public void HatUpClicked()
	{
		UpgradeTitle.text = "Fancy Chef's Hat";
		ItemDesc.text = "It's just a hat";
		Description.text = "";
		FoodPicture.gameObject.SetActive(false);
		FoodButton.gameObject.SetActive(false);
		PatiencePicture.gameObject.SetActive(false);
		PatienceButton.gameObject.SetActive(false);
		CleaningPicture.gameObject.SetActive(false);
		CleaningButton.gameObject.SetActive(false);
		StovePicture.gameObject.SetActive(false);
		StoveButton.gameObject.SetActive(false);
		FryerPicture.gameObject.SetActive(false);
		FryerButton.gameObject.SetActive(false);
		IcePicture.gameObject.SetActive(false);
		IceButton.gameObject.SetActive(false);
		DrinkPicture.gameObject.SetActive(false);
		DrinkButton.gameObject.SetActive(false);
		MatPicture.gameObject.SetActive(false);
		MatButton.gameObject.SetActive(false);
		MovementPicture.gameObject.SetActive(false);
		MovementButton.gameObject.SetActive(false);
		ChanceTipPicture.gameObject.SetActive(false);
		ChanceTipButton.gameObject.SetActive(false);
		AutoCutPicture.gameObject.SetActive(false);
		AutoCutButton.gameObject.SetActive(false);
		IncognitoPicture.gameObject.SetActive(false);
		IncognitoButton.gameObject.SetActive(false);
		HatPicture.gameObject.SetActive(true);
		HatButton.gameObject.SetActive(true);
		
		TierBar.gameObject.SetActive(false);
		FoodFullyUpgradedTier.gameObject.SetActive(false);
		CleaningFullyUpgradedTier.gameObject.SetActive(false);
		PatienceFullyUpgradedTier.gameObject.SetActive(false);
		FryerAlreadyBought.gameObject.SetActive(false);
		IceAlreadyBought.gameObject.SetActive(false);
		DrinkAlreadyBought.gameObject.SetActive(false);
		MatAlreadyBought.gameObject.SetActive(false);
		MovementAlreadyBought.gameObject.SetActive(false);
		ExtraTipAlreadyBought.gameObject.SetActive(false);
		AutoCutAlreadyBought.gameObject.SetActive(false);
		IncognitoAlreadyBought.gameObject.SetActive(false);
		HatAlreadyBought.gameObject.SetActive(false);
		
		int cost = OneTimePurchaseUpgrade.Instance.HatCost;
		CostForUpgrade.text = "$"+cost;
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewHatId))
		{
			HatButton.gameObject.SetActive(false);
			HatAlreadyBought.gameObject.SetActive(true);
			ItemDesc.text = "You already purchased this!";
		}
		if (PlayerStatistics.Instance.CurrentMoney < cost)
		{
			HatButton.interactable = false;
		}
	}
	
	public void ContinueButtonClicked()
	{
	
		ApplyUpgrade();
		m_upgradeScreen.GetComponent<UpgradePanel>().UpdateFinished();
		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewStoveId))
		{
			//set new stove object active
			NewStove.gameObject.SetActive(true);
		}

		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewFryerId))
		{
			//set new fryer object active
			NewFryer.gameObject.SetActive(true);
		}

		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewDrinkId))
		{
			//set new drink object active\
			NewDrink.gameObject.SetActive(true);
		}

		if (CheckOneTimePurchased(OneTimePurchaseUpgrade.Instance.NewCleaningId))
		{
			//set new cleaning mat object active
			NewCleaning.gameObject.SetActive(true);
		}
//		PreviousUpgradeStore();
	}

	private void ApplyUpgrade()
	{
		Upgrade FoodUp = m_upgradeMap[m_foodUpgradeId];
		Upgrade PatienceUp = m_upgradeMap[m_patienceUpgradeId];
		Upgrade CleaningUp = m_upgradeMap[m_cleaningUpgradeId];
		FoodUp.ApplyUpgrade();
		PatienceUp.ApplyUpgrade();
		CleaningUp.ApplyUpgrade();
		Debug.Log("Three Upgrade applied!");
		//IceUpgrade
		if (PlayerStatistics.Instance.PurchasedOneTime.Contains(OneTimePurchaseUpgrade.Instance.NewIceId))
		{
			Debug.Log("IceApplied");
			FoodTaskUpgradeHelper.Instance.UpgradeIceLimit(m_IceLimit[1]);
		}
		else
		{
			FoodTaskUpgradeHelper.Instance.UpgradeIceLimit(m_IceLimit[0]);
		}
		//MovementUpgrade
		if (PlayerStatistics.Instance.PurchasedOneTime.Contains(OneTimePurchaseUpgrade.Instance.NewMovementId))
		{
			Debug.Log("MovementApplied");
			WorkerManager.Instance.WorkerMovementChange(m_MovementSpeed[1]);
		}
		else
		{
			Debug.Log("MovementNotApplied");
			WorkerManager.Instance.WorkerMovementChange(m_MovementSpeed[0]);
		}
		//Automatic Cutting Board
		if (PlayerStatistics.Instance.PurchasedOneTime.Contains(OneTimePurchaseUpgrade.Instance.NewAutoCutId))
		{
			FoodTaskUpgradeHelper.Instance.CutAutomatic();
		}
		//Incognito mode
		if (PlayerStatistics.Instance.PurchasedOneTime.Contains(OneTimePurchaseUpgrade.Instance.NewIncognitoId))
		{
			CustomerManager.Instance.IsIncognito = true;
			WallWithBadge.gameObject.SetActive(true);
		}
		//New Hat
		if (PlayerStatistics.Instance.PurchasedOneTime.Contains(OneTimePurchaseUpgrade.Instance.NewHatId))
		{
			WorkerManager.Instance.SelectedWorker.MakeChef();
		}
		PauseManager.Instance.OnUpgradeScreenDone();
	}

	public void PurchaseUpgrade(int UpgradeCost)
	{
		int CurrentMoney = PlayerStatistics.Instance.CurrentMoney;
		if (CurrentMoney >= UpgradeCost)
		{
			int MoneyAfterPurchase = CurrentMoney - UpgradeCost;
			PlayerStatistics.Instance.CurrentMoney = MoneyAfterPurchase;
		}
		else
		{
			Debug.Log("Not enough money to purchase upgrade");
		}
		CurrentBank.text = ""+PlayerStatistics.Instance.CurrentMoney;
	}
	
	public bool CheckOneTimePurchased(int PurchasedId)
	{
		return PlayerStatistics.Instance.PurchasedOneTime.Contains(PurchasedId);
	}

	public void SetFoodPrepTier(int tier)
	{
		PlayerStatistics.Instance.FoodPrepTier = tier;
	}
	
	public void SetPatienceTier(int tier)
	{
		PlayerStatistics.Instance.PatienceTier = tier;
	}
	
	public void SetCleaningTier(int tier)
	{
		PlayerStatistics.Instance.CleaningTier = tier;
	}

	public void LoadOneTimeUpgrades(List<int> OneTime)
	{
		PlayerStatistics.Instance.PurchasedOneTime.Clear();
		for (int i = 0; i < OneTime.Count; i++)
		{
			PlayerStatistics.Instance.PurchasedOneTime.Add(OneTime[i]);
		}
	}
}
