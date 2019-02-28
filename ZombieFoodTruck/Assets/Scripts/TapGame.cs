using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapGame : MonoBehaviour {

	private int m_currentButton;

	[SerializeField]
	private KeyCode m_tapKey;
	
	[SerializeField]
	private KeyCode m_breakKey;
	
	[SerializeField] 
	private TaskManager m_taskManager;

	[SerializeField] 
	private int m_maxTaps;

	[SerializeField] 
	private int m_minTaps;

	private bool m_isTapping = false;

	[SerializeField]
	private float m_progressOnComplete;

	[SerializeField] 
	private Text m_text;
	
	private int m_tapsLeft;

	[SerializeField]
	private float m_stunTime;

	private bool m_isStunned;

	private void OnEnable()
	{
		m_text.enabled = true;
		Random.InitState(0);
		m_tapsLeft = Random.Range(m_minTaps, m_maxTaps + 1);
		m_text.text = m_tapsLeft.ToString();
		m_isTapping = true;
	}

	private void OnDisable()
	{
		m_text.enabled = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if (m_isStunned)
		{
			return;
		}
		if (Input.GetKeyDown(m_tapKey))
		{
			if (m_isTapping)
			{
				m_tapsLeft--;
				m_text.text = m_tapsLeft.ToString();
				if (m_tapsLeft == 0)
				{
					m_isTapping = false;
					m_text.text = "BREAK";
				}
			}
			else
			{
				StartCoroutine(Stun());
				return;
			}
		}
		else if (Input.GetKeyDown(m_breakKey))
		{
			if (m_isTapping)
			{
				StartCoroutine(Stun());
				return;
			}
			else
			{
				m_taskManager.AddProgress(m_progressOnComplete);
				OnBreak();
			}
		}
	}

	private void OnBreak()
	{
		m_isTapping = true;
		m_tapsLeft = Random.Range(m_minTaps, m_maxTaps + 1);
		m_text.text = m_tapsLeft.ToString();
	}

	IEnumerator Stun()
	{
		m_isStunned = true;
		m_text.text = "STUNNED";
		yield return new WaitForSeconds(m_stunTime);
		m_isStunned = false;
		OnBreak();
	}
}
