using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationStartManager : MonoBehaviourSingleton<UIAnimationStartManager>
{
	[SerializeField] private ButtonSpawn[] m_buttonSpawns;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AnimateIn()
	{
		for (int i = 0; i < m_buttonSpawns.Length; i++)
		{
			m_buttonSpawns[i].AnimateIn();
		}
	}
	
	public void AnimateOut()
	{
		for (int i = 0; i < m_buttonSpawns.Length; i++)
		{
			m_buttonSpawns[i].AnimateOut();
		}
	}
}
