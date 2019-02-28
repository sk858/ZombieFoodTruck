using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletDrop : BaseConsumable
{

	[SerializeField] private int m_moneyAwarded;

	private string m_patienceModId;
	
	
	public override void Activate()
	{
		MoneyManager.Instance.EarnMoney(m_moneyAwarded);
	}

	public override void Deactivate()
	{
		
	}

	public override string GetAlertMessage()
	{
		return "STOLE THEIR WALLET: +$" + m_moneyAwarded;
	}

	public int GetMoneyAward()
	{
		return m_moneyAwarded;
	}
	
}
