using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

	public enum FoodType
	{
		Undef,
		Burger,
		Fries,
		Drink,
		IceCream
	}

	[SerializeField]
	private FoodType m_type;

	public FoodType Type
	{
		get { return m_type; }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
