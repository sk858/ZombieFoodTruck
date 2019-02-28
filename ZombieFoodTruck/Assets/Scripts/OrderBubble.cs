using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBubble : MonoBehaviour
{
	
	[SerializeField] 
	private GameObject m_hamburger;

	[SerializeField] 
	private GameObject m_fries;

	[SerializeField] 
	private GameObject m_drink;

	[SerializeField] 
	private GameObject m_iceCream;

	[SerializeField] 
	private Transform m_foodAnchor;

	[SerializeField] 
	private float m_horizontalDist;

	[SerializeField] 
	private GameObject m_exclamationPoint;
	

	private List<GameObject> m_foodInBubble = new List<GameObject>();

	private List<Food.FoodType> m_order;

	public void Init(List<Food.FoodType> order)
	{
		m_order = order;
		for (int i = 0; i < m_order.Count; i++)
		{
			GameObject food;
			switch (m_order[i])
			{
				case Food.FoodType.Burger:
					food = Instantiate(m_hamburger);
					break;
				case Food.FoodType.Drink:
					food = Instantiate(m_drink);
					break;
				case Food.FoodType.Fries:
					food = Instantiate(m_fries);
					break;
				case Food.FoodType.IceCream:
					food = Instantiate(m_iceCream);
					break;
				default:
					Debug.LogError("ERROR IN PROCESSING FOOD BUBBLE");
					return;
			}
			food.transform.parent = m_foodAnchor;
			m_foodInBubble.Add(food);
		}

		float m_startingX = 0f;
		
		if (m_order.Count % 2 == 0)
		{
			m_startingX = (-m_horizontalDist / 2f) - (m_horizontalDist * ((m_order.Count / 2) - 1));
		}
		else
		{
			m_startingX = -(m_horizontalDist * (m_order.Count / 2));
		}
		
		foreach (GameObject food in m_foodInBubble)
		{
			food.transform.localPosition = new Vector3(m_startingX, 0, -0.1f);
			m_startingX += m_horizontalDist;
		}
	}

	public void ExcalamationPoint()
	{
		gameObject.SetActive(true);
		m_exclamationPoint.SetActive(true);
	}
}
