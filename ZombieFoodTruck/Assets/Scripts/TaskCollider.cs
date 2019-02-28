using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskCollider : MonoBehaviour
{

	private TaskManager m_currentCollidingTask;

	[SerializeField] 
	private ButtonSpawn m_spaceBar;

	private Worker m_worker;

	private bool m_spaceBarStatus;

	public TaskManager CurrentCollidingTask
	{
		get
		{
			return m_currentCollidingTask;
		}
	}

	private void Awake()
	{
		m_worker = GetComponentInParent<Worker>();
	}

	private void FixedUpdate()
	{
		if (m_currentCollidingTask != null)
		{
			if (m_spaceBar.Active != m_currentCollidingTask.CanDock(m_worker))
			{
				SetSpaceBar(m_currentCollidingTask.CanDock(m_worker));
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("BoundingBox") && m_currentCollidingTask == null)
		{
			m_currentCollidingTask = other.GetComponentInParent<TaskManager>();
			SetSpaceBar(m_currentCollidingTask.CanDock(m_worker));
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("BoundingBox"))
		{
			SetSpaceBar(false);
			m_currentCollidingTask = null;
		}
	}

	private void SetSpaceBar(bool state)
	{
		if (state)
		{
			m_spaceBar.AnimateIn();
		}
		else
		{
			m_spaceBar.AnimateOut();
		}
	}
}
