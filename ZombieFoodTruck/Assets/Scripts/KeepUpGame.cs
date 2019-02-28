using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUpGame : MonoBehaviour
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
	private float m_gravity;

	[SerializeField] 
	private float m_velocityUp;

	[SerializeField] 
	private float m_velocityLimit;

	[SerializeField] 
	private TaskManager m_taskManager;

	private bool m_hitBottom;

	// Update is called once per frame
	void Update ()
	{
		Vector3 pos = m_upPiece.transform.localPosition;
		float yPos = Mathf.Abs(pos.y);
		float output = 0;
		if (yPos > m_redBound)
		{
			output = m_redOutput;
		}
		else if (yPos > m_yellowBound)
		{
			output = m_yellowOutput;
		}
		else
		{
			output = m_greenOutput;
		}
		m_taskManager.AddProgress(output*Time.deltaTime);
	}

	private void LateUpdate()
	{
		Vector3 pos = m_upPiece.transform.localPosition;
		Vector2 velocity = m_upPiece.GetComponent<Rigidbody2D>().velocity;
		if (Input.GetKeyDown(KeyCode.Space))
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
		m_upPiece.transform.localPosition = pos;
		m_upPiece.GetComponent<Rigidbody2D>().velocity = velocity;
	}
}
