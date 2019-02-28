using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{

	private List<int> m_line = new List<int>();

	[SerializeField] 
	private int m_width;

	[SerializeField] 
	private Customer.CustomerState m_inLineState;

	[SerializeField] 
	private Customer.CustomerState m_stateOnFront;
	
	[SerializeField]
	private float m_verticalOffset;

	[SerializeField] 
	private float m_horizontalOffset;

	public int Length
	{
		get
		{
			return m_line.Count;
		}
	}
	
	/*
	 * Adds customer to line and returns the physical position
	 * the customer will be in line as well as whether they will
	 * be in front or not
	 */
	public Vector3 AddCustomer(int customerId)
	{

		Vector3 newPos = GetPos(m_line.Count);
		m_line.Add(customerId);
		return newPos;
	}
	
	/*
	 * Remove customer from given line and move all customers
	 * in line ahead to their new spots
	 */
	public void RemoveCustomer(int customerId)
	{
		int index = m_line.FindIndex((x) => x == customerId);
		if (index == -1)
		{
			return;
		}
		m_line.RemoveAt(index);
		for (int i = index; i < m_line.Count; i++)
		{
			Customer.CustomerState state = m_inLineState;
			if (i == 0 && InputManager.Instance.ArrowMode)
			{
				state = m_stateOnFront;
			}
			Vector3 newPos = GetPos(i);
			Customer c = CustomerManager.Instance.GetCustomer(m_line[i]);
			CustomerManager.Instance.MoveCustomer(m_line[i], newPos, state);
		}
	}

	private Vector3 GetPos(int i)
	{
		Vector3 newPos = transform.position;
	/*	Vector3 horizontal = new Vector3(m_horizontalOffset, 0, 0);
		if (i % 2 == 0)
		{
			newPos -= horizontal;
		}
		else
		{
			newPos += horizontal;
		}*/
		newPos += new Vector3(0, m_verticalOffset * i, 0);
		return newPos;
	}

	public Vector3 GetFrontPos()
	{
		return transform.position;
	}

	public int GetCustomer(int i)
	{
		return m_line[i];
	}
	
	/*
	 * Returns ID of customer at the head of the line
	 */
	public int NextInLine()
	{
		if (m_line.Count == 0)
		{
			return -1;
		}
		else
		{
			return m_line[0];
		}
	}

	public bool HasCustomer(int customerId)
	{
		return m_line.Contains(customerId);
	}

	public bool IsCustomersWaiting()
	{
		for (int i = 0; i < m_line.Count; i++)
		{
			if (!CustomerManager.Instance.GetCustomer(m_line[i]).IsMoving)
			{
				return true;
			}
		}
		return false;
	}

	public void MoveToFront(int index, Customer.CustomerState stateOnFront)
	{
		int id = m_line[index];
		m_line.RemoveAt(index);
		m_line.Insert(0, id);
		for (int i = 0; i < m_line.Count; i++)
		{
			Customer.CustomerState state = m_inLineState;
			if (i == 0)
			{
				state = stateOnFront;
			}
			Vector3 newPos = GetPos(i);
			Customer c = CustomerManager.Instance.GetCustomer(m_line[i]);
			CustomerManager.Instance.MoveCustomer(m_line[i], newPos, state);
		}
	}
}
