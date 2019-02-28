using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatingMechanicManager : MonoBehaviourSingleton<EatingMechanicManager>
{

	[SerializeField]
	private bool m_isMeatEnabled;

	[SerializeField]
	private bool m_isHungerEnabled;
	 
	private int m_energy;

	[SerializeField]
	private int m_startingEnergy;

	[SerializeField]
	private int m_maxEnergyTier1;

	[SerializeField]
	private int m_tier2Level;

	[SerializeField]
	private int m_maxEnergyTier2;

	[SerializeField]
	private int m_startingEnergyTier2;

	[SerializeField]
	private int m_tier3Level;

	[SerializeField]
	private int m_maxEnergyTier3;

	[SerializeField] 
	private int m_startingEnergyTier3;

	[SerializeField] 
	private int m_energyPerTask;

	[SerializeField] 
	private int m_energyOnEat;

	[SerializeField]
	private int m_energyOnEatBuff;

	[SerializeField] 
	private float m_movementModifier;  // float between 0 and 1, the fraction of the base movement speed that the player moves when out of energy

	[SerializeField]
	private ServeFoodManager m_serveFoodManager;

	[SerializeField]
	private CleaningManager m_cleaningmanager;

	public int MaxEnergy
	{
		get {
			if (EnergyBarUpgradeManager.Instance.IsUpgrade)
			{
				return EnergyBarUpgradeManager.Instance.GetMaxEnergy();
			}
			else
			{
				int level = PlayerStatistics.Instance.CurrentLevel;
				if (level >= m_tier3Level)
					return m_maxEnergyTier3;
				else if (level >= m_tier2Level)
					return m_maxEnergyTier2;
				else
					return m_maxEnergyTier1;
			}
		}
	}

	private int Energy
	{
		get { return m_energy; }
		set 
		{
			m_energy = (int) Mathf.Clamp(value,0,MaxEnergy);
			EnergyBarManager.Instance.SetEnergy(m_energy);
		}
	}

	public int GetEnergy()
	{
		return m_energy;

	}

	public bool IsMeatEnabled
	{
		get
		{
			return m_isMeatEnabled;
		}
	}

	public bool IsHungerEnabled
	{
		get
		{
			return m_isHungerEnabled;
		}
	}

	public float MovementModifier
	{
		get
		{
			// if the player is out of energy, then return the mod, else return 1
			if (m_energy <= 0)
				return Mathf.Clamp(m_movementModifier,0f,1f);
			else
				return 1;
		}
	}

	public bool IsEnergyEmpty
	{
		get { return m_energy <= 0; }
	}

	public bool IsEnergyFull 
	{
		get { return m_energy >= MaxEnergy; }
	}

	private void Start() 
	{
		m_serveFoodManager.OnFoodServed.AddListener (OnFoodServed);
//		m_cleaningmanager.OnBodyCleaned.AddListener (OnBodyCleaned);
		int level = PlayerStatistics.Instance.CurrentLevel;
		if (!EnergyBarUpgradeManager.Instance.IsUpgrade)
		{
			if (level >= m_tier3Level)
			{
				EnergyBarManager.Instance.SetEnergy(m_startingEnergyTier3, true);
				m_energy = m_startingEnergyTier3;
			}
			else if (level >= m_tier2Level)
			{
				EnergyBarManager.Instance.SetEnergy(m_startingEnergyTier2, true);
				m_energy = m_startingEnergyTier2;
			}
			else
			{
				EnergyBarManager.Instance.SetEnergy(m_startingEnergy, true);
				m_energy = m_startingEnergy;
			}
		}
		else
		{
			
		}
		// start with 1 energy on level 1, the eating tutorial
		if (level == 1) {
			EnergyBarManager.Instance.SetEnergy(1, true);
			m_energy = 1;
		}
	}

	public void OnBodyCleaned(Customer.CustomerType bodyType)
	{
		if (IsHungerEnabled)
		{
			if (bodyType == Customer.CustomerType.Buff)
			{
				Energy += m_energyOnEatBuff;
			}
			else
			{
				Energy += m_energyOnEat;
			}
			WorkerManager.Instance.SetHungry(false);
		}
	}

	private void OnFoodServed()
	{
		if (IsHungerEnabled) {
			Energy -= m_energyPerTask;
			if (Energy <= 1)
			{
				WorkerManager.Instance.SetHungry(true);
			}
			if (Energy <= 0) {
				TutorialManager.Instance.OnEnergyEmpty ();
				WorkerManager.Instance.OnEnergyEmpty();
			}
		}
	}
}
