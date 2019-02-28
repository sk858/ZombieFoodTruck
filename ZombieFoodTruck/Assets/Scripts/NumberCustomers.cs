using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberCustomers : MonoBehaviour
{
	
	[SerializeField]
	private Text m_text;

	private int m_max;
	
	// Use this for initialization
	public void Init (int max)
	{
		m_max = max;
		m_text.text = "Customers Left: " + m_max + "/" + m_max;
	}

	public void UpdateText(int newNum)
	{
		m_text.text = "Customers Left: " + newNum + "/" + m_max;
	}
}
