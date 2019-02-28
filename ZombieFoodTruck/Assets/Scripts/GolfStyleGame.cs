using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.VR.WSA;

public class GolfStyleGame : MonoBehaviour
{

	[SerializeField]
	private float m_yellowBound;

	[SerializeField] 
	private float m_redBound;

	[SerializeField] 
	private float m_outerBound;

	[SerializeField] 
	private float m_yellowOutput;

	[SerializeField] 
	private float m_redOutput;

	[SerializeField] 
	private float m_greenOutput;

	[SerializeField] 
	private GameObject m_upPiece;

	[SerializeField] 
	private float m_baseMaxSpeed;

	[SerializeField] 
	private float m_baseMinSpeed;

	private float m_maxSpeed;

	[SerializeField]
	private float m_maxSpeedIncrement;

	[SerializeField] 
	private float m_minSpeedIncrement;

	private float m_minSpeed;

	[SerializeField] 
	private TaskManager m_taskManager;

	private bool m_hitBottom;

	private bool m_isRunning;

	private bool m_isFrozen;

	private bool m_goingUp = true;

	private bool m_justEnabled = true;

	[SerializeField] 
	private float m_freezeTime;
	
	private void OnEnable()
	{
		m_isRunning = true;
		m_isFrozen = false;
		m_justEnabled = true;
		m_maxSpeed = m_baseMaxSpeed;
		m_minSpeed = m_baseMinSpeed;
	}

	private void OnDisable()
	{
		m_isRunning = false;
	}

	// Update is called once per frame
	void Update ()
	{
		Vector3 pos = m_upPiece.transform.localPosition;
		float yPos = Mathf.Abs(pos.y);
		float output = 0;

	//	m_taskManager.AddProgress(output*Time.deltaTime);
	}

	private void LateUpdate()
	{
		if (m_isFrozen)
		{
			return;
		}
		Vector3 pos = m_upPiece.transform.localPosition;

		float posModifier = Mathf.Lerp(m_minSpeed, m_maxSpeed, (m_outerBound - Mathf.Abs(pos.y)) / m_outerBound) * Time.deltaTime;

		if (!m_goingUp)
		{
			posModifier *= -1;
		}

		pos.y += posModifier;

		if (Mathf.Abs(pos.y) > m_outerBound)
		{
			m_goingUp = !m_goingUp;
			pos.y = Mathf.Clamp(pos.y, -m_outerBound, m_outerBound);
		}
		
		m_upPiece.transform.localPosition = pos;

		if (Input.GetKeyDown(KeyCode.Space) && !m_justEnabled)
		{
			OnButtonPressed();
			return;
		}
		if (m_justEnabled)
		{
			m_justEnabled = false;
		}
	}

	private void OnButtonPressed()
	{
		if (m_isFrozen || !m_isRunning)
		{
			return;
		}
		m_upPiece.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		float output;
		float yPos = Mathf.Abs(m_upPiece.transform.localPosition.y);
		if (yPos > m_redBound)
		{
			output = m_redOutput;
			m_maxSpeed = m_baseMaxSpeed;
			m_minSpeed = m_baseMinSpeed;
		}
		else if (yPos > m_yellowBound)
		{
			output = m_yellowOutput;
			m_maxSpeed += m_maxSpeedIncrement;
			m_minSpeed += m_minSpeedIncrement;
		}
		else
		{
			output = m_greenOutput;
			m_maxSpeed += m_maxSpeedIncrement;
			m_minSpeed += m_minSpeedIncrement;
		}
		StartCoroutine(Freeze(output));
	}

	IEnumerator Freeze(float progress)
	{
		m_isFrozen = true;
		
		yield return new WaitForSeconds(m_freezeTime);
		m_taskManager.AddProgress(progress);
		m_isFrozen = false;
		
		Vector3 pos = m_upPiece.transform.localPosition;
		pos.y = m_outerBound * -1;
		m_upPiece.transform.localPosition = pos;
	}
}
