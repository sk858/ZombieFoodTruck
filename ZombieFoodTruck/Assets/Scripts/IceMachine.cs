using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMachine : TaskManager
{
	
	[SerializeField]
	private GameObject m_iceBagPrefab;

	private AudioSource m_audioSource;

	protected override void Awake()
	{
		base.Awake();
		m_audioSource = GetComponent<AudioSource>();
	}
	
	public override bool OnWorkingChanged(bool isWorking, Worker worker)
	{
		if (worker.HoldingIce)
		{
			worker.RemoveIce();
			worker.QuickTaskStart();
		}
		if (!worker.HoldingSomething)
		{
			GameObject iceBag = Instantiate(m_iceBagPrefab, worker.transform);
			iceBag.transform.localPosition = new Vector3(0, -0.1f, 0);
			worker.AddIce(iceBag);
			TutorialManager.Instance.OnTaskStarted (TaskType.Ice);
			TruckLogger.Instance.LogGetIce ();
			m_audioSource.Play();
			worker.QuickTaskStart();
		}
		return false;
	}

	public override bool CanDock(Worker worker)
	{
		if (EatingMechanicManager.Instance.IsEnergyEmpty)
		{
			return false;
		}
		if(worker.HoldingIce)
		{
			return true;
		}
		return !worker.HoldingSomething;
	}
}
