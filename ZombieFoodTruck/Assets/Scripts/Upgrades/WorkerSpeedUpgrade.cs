using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WorkerSpeedUpgrade : Upgrade
{   
    [SerializeField]
    private int m_workerIdToApplyTo;
    
    [SerializeField]
    private float m_speedBoost;

    private Upgrade m_workerUpgrade;
    
    [SerializeField]
    private TaskManager.TaskType m_boostType;

    public void Init(int workerId, float speedBoost, TaskManager.TaskType boostType, int upgradeId)
    {
        m_workerIdToApplyTo = workerId;
        m_speedBoost = speedBoost;
        m_boostType = boostType;
        m_upgradeId = upgradeId;
    }
    
    public override void ApplyUpgrade()
    {
        Worker w = WorkerManager.Instance.GetWorker(m_workerIdToApplyTo);
        if (!w.IsUpgradeApplied(this))
        {
            float newSpeed = w.GetSpeed(m_boostType) + m_speedBoost;
            w.SetSpeed(m_boostType, newSpeed);
            w.AddUpgrade(this);
        }
    }
    
    
    public override bool IsApplied()
    {
        Worker w = WorkerManager.Instance.GetWorker(m_workerIdToApplyTo);
        return w.IsUpgradeApplied(this);
    }
    public override void UpgradeTier()
    {
        
    }
}
