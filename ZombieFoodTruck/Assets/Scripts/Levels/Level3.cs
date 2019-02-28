using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Normal, 1, 1, 2, new Food.FoodType[]{Food.FoodType.Fries}),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 10, 12, new Food.FoodType[]{Food.FoodType.Fries}),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 27, 29, new Food.FoodType[]{Food.FoodType.Burger}),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 34, 36, new Food.FoodType[]{Food.FoodType.Burger}),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 50, 51, new Food.FoodType[]{Food.FoodType.Fries}),
			};
		}
	}


	public override CustomerBatch[][] KnockoutGroups
	{
		get
		{
			return new CustomerBatch[][]
			{
				
			};
		}
	}

	public override int Quota
	{
		get { return 15; }
	}

	public override float DayLength
	{
		get { return 55; }
	}

	public override int LevelID
	{
		get { return 3; }
	}

	public override List<Food.FoodType> FoodStations
	{
		get
		{
			return new List<Food.FoodType>
			{
				Food.FoodType.Burger,
				Food.FoodType.Fries
			};
		}
	}

	public override int MaxOrder
	{
		get { return 1; }
	}

	public override bool DisableEating 
	{
		get { return false; }
	}

	public override string StartMessage
	{
		get { return "You can serve fries now too! Cut the potatoes at the cutting" +
			"board, and then toss them into the fryer!"; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
