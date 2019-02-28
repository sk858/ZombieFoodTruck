using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StardewValleyFishGame : MonoBehaviour
{

	[SerializeField]
	private GameObject m_target;

	[SerializeField]
	private GameObject m_playerPiece;

	[SerializeField]
	private float m_gravity;

	[SerializeField] 
	private float m_targetGravity;

	[SerializeField]
	private float m_velocityLimit;

	[SerializeField] 
	private float m_targetVelocityLimit;

	[SerializeField] 
	private float m_targetAddVelocity;

	[SerializeField] 
	private float m_velocityUp;

	[SerializeField] 
	private float m_outerBound;

	[SerializeField] 
	private float m_targetHalfSize;

	[SerializeField] 
	private TaskManager m_taskManager;

	[SerializeField] 
	private float m_progressOnSucess;

	private bool m_hitBottom;

	private bool m_targetHitBottom;

	void Awake()
	{
		UnityEngine.Random.InitState(DateTime.Now.GetHashCode());
	}

	private void OnEnable()
	{
		Vector3 pos = m_target.transform.localPosition;
		pos.y = 0f;
		m_target.transform.localPosition = pos;
		pos = m_playerPiece.transform.localPosition;
		pos.y = -m_outerBound;
	}

	// Update is called once per frame
	void Update ()
	{
		float playerPosY = m_playerPiece.transform.localPosition.y;
		float targetPosY = m_target.transform.localPosition.y;

		if (playerPosY <= targetPosY + m_targetHalfSize && playerPosY >= targetPosY - m_targetHalfSize)
		{
			m_taskManager.AddProgress(m_progressOnSucess*Time.deltaTime);
		}
	}

	private void LateUpdate()
	{
		Vector3 pos = m_playerPiece.transform.localPosition;
		Vector2 velocity = m_playerPiece.GetComponent<Rigidbody2D>().velocity;
		if (Input.GetKey(KeyCode.Space))
		{
			velocity.y = Mathf.Clamp(velocity.y, 0, m_velocityLimit);
			velocity.y += m_velocityUp*Time.deltaTime;
			m_hitBottom = false;
		}
		else if(!m_hitBottom)
		{
			velocity.y = Mathf.Clamp(velocity.y, -m_velocityLimit, m_velocityLimit);
			velocity.y -= m_gravity*Time.deltaTime;
		}
				
		if (Mathf.Abs(pos.y) > m_outerBound)
		{
			m_hitBottom = pos.y < 0;
			velocity = Vector2.zero;
			pos.y = Mathf.Clamp(pos.y, -m_outerBound, m_outerBound);
		}

		m_playerPiece.transform.localPosition = pos;
		m_playerPiece.GetComponent<Rigidbody2D>().velocity = velocity;
		
		pos = m_target.transform.localPosition;
		velocity = m_target.GetComponent<Rigidbody2D>().velocity;

		velocity.y += UnityEngine.Random.Range(-m_targetAddVelocity, m_targetAddVelocity) * Time.deltaTime;

		              
		if(!m_targetHitBottom)
		{
	//		velocity.y = Mathf.Clamp(velocity.y, -m_velocityLimit, m_velocityLimit);
			velocity.y -=  m_targetGravity*Time.deltaTime;
		}
		
		velocity.y = Mathf.Clamp(velocity.y, -m_targetVelocityLimit, m_targetVelocityLimit);

		if (Mathf.Abs(pos.y) > m_outerBound - m_targetHalfSize)
		{
			m_targetHitBottom = pos.y < 0;
			velocity = Vector2.zero;
			pos.y = Mathf.Clamp(pos.y, -m_outerBound + m_targetHalfSize, m_outerBound - m_targetHalfSize);
		}
		
		m_target.transform.localPosition = pos;
		m_target.GetComponent<Rigidbody2D>().velocity = velocity;
	}
}
