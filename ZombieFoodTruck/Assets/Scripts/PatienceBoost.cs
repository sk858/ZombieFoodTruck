using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBoost : MonoBehaviour
{
	
	[SerializeField]
	private float m_timeLasting;

	[SerializeField] 
	private float m_buffValue;

	[SerializeField] 
	private float m_timeToRecharge;

	[SerializeField] 
	private ProgressBar m_progressBar;
	
	[SerializeField]
	private bool m_isInstant = true;

	private float m_timeSpentRecharging;

	private bool m_isRecharging;

	private Button m_button;

	private void Awake()
	{
		m_button = GetComponent<Button>();
		m_progressBar.Value = 1f;
	}
	
	private void Update()
	{
		//If charging, keep track of time spent charging
		//and leave charging state when finished
		if (m_isRecharging)
		{
			m_timeSpentRecharging += Time.deltaTime;
			m_progressBar.Value = m_timeSpentRecharging / m_timeToRecharge;
			if (m_timeSpentRecharging >= m_timeToRecharge)
			{
				m_isRecharging = false;
				m_timeSpentRecharging = 0f;
				m_button.interactable = true;
			}
		}
	}

	public void OnButtonClick()
	{
		if (m_isRecharging)
		{
			return;
		}
		//Boost patience for all customers
		CustomerManager.Instance.BoostPatience(m_buffValue);
		m_button.interactable = false;
		StartCoroutine("WaitForDebuff");
	}

	public IEnumerator WaitForDebuff()
	{
		//Wait m_timeLasting seconds to reset patience boost,
		//or disable boost instantly (i.e. no more new customers spawn with boost) 
		float timeWaiting = 0f;
		while (timeWaiting < m_timeLasting && !m_isInstant)
		{
			timeWaiting += Time.deltaTime;
			m_progressBar.Value = (m_timeLasting - timeWaiting)/m_timeLasting;
			yield return null;
		}
		
		CustomerManager.Instance.BoostPatience(0);
		m_isRecharging = true;
	}
}
