using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTaskUpgradeHelper : MonoBehaviourSingleton<FoodTaskUpgradeHelper> {

	private StoveTask[] m_stoveTask;
	
	private DrinkManager[] m_drinkTask;
	
	private FryerManager[] m_fryerTask;
	
	private IceCreamManager m_iceCreamTask;

	[SerializeField] private CuttingBoardManager m_cutBoardTask;

	private Upgrade m_upgrade;
		
	// Use this for initialization
	void Awake ()
	{
		m_stoveTask = Resources.FindObjectsOfTypeAll<StoveTask>();
		m_drinkTask = Resources.FindObjectsOfTypeAll<DrinkManager>();
		m_fryerTask = Resources.FindObjectsOfTypeAll<FryerManager>();
		m_iceCreamTask = Resources.FindObjectsOfTypeAll<IceCreamManager>()[0];
		//m_cutBoardTask = Resources.FindObjectsOfTypeAll<CuttingBoardManager>()[0];
	}

	public void UpgradeAll(float StoveBoost, float DrinkBoost, float FryerBoost, float IceCreamBoost, float CutBoardBoost,  Upgrade upgrade)
	{
		foreach(StoveTask st in m_stoveTask)
		{
			st.AddStationUpgrade(upgrade);
			st.StationTimeUpgrade(StoveBoost);
		}

		foreach (var dm in m_drinkTask)
		{
			dm.StationTimeUpgrade(DrinkBoost);
		}

		foreach (var ft in m_fryerTask)
		{
			ft.StationTimeUpgrade(FryerBoost);
		}
		m_iceCreamTask.StationTimeUpgrade(IceCreamBoost);
		m_cutBoardTask.StationTimeUpgrade(CutBoardBoost);
		m_cutBoardTask.SetFoodProgressTier(PlayerStatistics.Instance.FoodPrepTier);
		m_upgrade = upgrade;
	}

	public void UpgradeIceLimit(int newIceLimit)
	{
		foreach (var dm in m_drinkTask)
		{
			dm.IncreaseIceLimit(newIceLimit);
		}
	}

	public void CutAutomatic()
	{
		m_cutBoardTask.MakeInstant();
	}
	
	public float GetStoveSpeed()
	{
		return m_stoveTask[0].m_progressForCompletion;
	}

	public float GetDrinkSpeed()
	{
		return m_drinkTask[0].m_progressForCompletion;
	}

	public float GetFryerSpeed()
	{
		return m_fryerTask[0].m_progressForCompletion;
	}

	public float GetIceCreamSpeed()
	{
		return m_iceCreamTask.m_progressForCompletion;
	}

	public float GetCuttingSpeed()
	{
		return m_cutBoardTask.m_progressForCompletion;
	}

	public bool IsApplied()
	{
		return m_stoveTask[0].StationsUpgradeApplied(m_upgrade);
	}
	
}
