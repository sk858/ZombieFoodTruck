using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class MoneyPerOrderUpgrade : Upgrade
{
    [SerializeField]
    private int m_amtToIncreaseBy;

    private Upgrade m_moneyUpgrade;

    public void Init(int amtToIncreaseBy, int upgradeId)
    {
        m_amtToIncreaseBy = amtToIncreaseBy;
        m_upgradeId = upgradeId;
    }
    
    public override void ApplyUpgrade()
    {
        if (!IsApplied())
        {
            CustomerManager.Instance.AddUpgrade(this);
            CustomerManager.Instance.IncreaseMoneyPerOrder(m_amtToIncreaseBy);
        }
    }
    
    public override bool IsApplied()
    {
        return CustomerManager.Instance.IsUpgradeApplied(this);
    }

    public override void UpgradeTier()
    {
        
    }
}
