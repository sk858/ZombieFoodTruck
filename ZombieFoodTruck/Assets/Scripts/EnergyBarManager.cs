using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBarManager : MonoBehaviourSingleton<EnergyBarManager>
{
	
	[SerializeField] 
	private GameObject m_sixEnergyBar;

	[SerializeField] 
	private GameObject m_eightEnergyBar;
	
	[SerializeField] 
	private GameObject m_twelveEnergyBar;
	
	[SerializeField] 
	private GameObject m_sixteenEnergyBar;

	private GameObject m_currentEnergyBar;

	private int m_maxEnergy;
	
	// Use this for initialization
	void Awake ()
	{
		if (EnergyBarUpgradeManager.Instance.IsUpgrade)
		{
			m_maxEnergy = EnergyBarUpgradeManager.Instance.GetMaxEnergy();
		}
		else
		{
			m_maxEnergy = EatingMechanicManager.Instance.MaxEnergy;
		}
		if(m_maxEnergy == 6)
		{
			m_currentEnergyBar = m_sixEnergyBar;
		}
		else if(m_maxEnergy == 8)
		{
			m_currentEnergyBar = m_eightEnergyBar;
		}
		else if(m_maxEnergy == 12)
		{
			m_currentEnergyBar = m_twelveEnergyBar;
		}
		else if (m_maxEnergy == 16)
		{
			m_currentEnergyBar = m_sixteenEnergyBar;
		}
		m_currentEnergyBar.SetActive(true);
		m_currentEnergyBar.GetComponent<EnergyBar>().SetValue(EatingMechanicManager.Instance.GetEnergy(), true);
	}

	public void SetEnergy(int energy, bool skipAnim = false)
	{
		m_currentEnergyBar.GetComponent<EnergyBar>().SetValue(energy, skipAnim);
	}

	public void SetColor(Color color)
	{
		m_currentEnergyBar.GetComponent<EnergyBar>().SetColor(color);
	}

	public void SetEnergyBar(int tier)
	{
		if (m_currentEnergyBar != null)
		{
			m_currentEnergyBar.SetActive(false);
		}
		if (tier == 0)
		{
			m_currentEnergyBar = m_sixEnergyBar;
			m_maxEnergy = 6;
		}
		else if (tier == 1)
		{
			m_currentEnergyBar = m_eightEnergyBar;
			m_maxEnergy = 8;
		}
		else if (tier == 2)
		{
			m_currentEnergyBar = m_twelveEnergyBar;
			m_maxEnergy = 12;
		}
		else
		{
			m_currentEnergyBar = m_sixteenEnergyBar;
			m_maxEnergy = 16;
		}
		m_currentEnergyBar.SetActive(true);
		m_currentEnergyBar.GetComponent<EnergyBar>().SetValue(EatingMechanicManager.Instance.GetEnergy(), true);
	}
}
