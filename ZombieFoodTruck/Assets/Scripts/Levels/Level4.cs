using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4 : BaseLevel {

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Rich, 1, 0, 7, new Food.FoodType[]{Food.FoodType.Burger}),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 13, 17, new Food.FoodType[]{Food.FoodType.Fries}),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 23, 27, new Food.FoodType[]{Food.FoodType.Fries}),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 33, 37, new Food.FoodType[]{Food.FoodType.Burger}),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 43, 47, new Food.FoodType[]{Food.FoodType.Fries}),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 53, 60, new Food.FoodType[]{Food.FoodType.Burger}),
			};
		}
	}


	public override CustomerBatch[][] KnockoutGroups
	{
		get
		{
			return new CustomerBatch[][]
			{
			/*	new CustomerBatch[]
				{
					new CustomerBatch(Customer.CustomerType.Rich, 1, 12, 14),
					new CustomerBatch(Customer.CustomerType.Rich, 1, 30, 32),
				},*/
			};
		}
	}

	public override int Quota
	{
		get { return 55; }
	}

	public override float DayLength
	{
		get { return 60; }
	}

	public override int LevelID
	{
		get { return 4; }
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
		get { return 2; }
	}

	public override bool DisableEating 
	{
		get { return false; }
	}

	public override string StartMessage
	{
		get { return "Wow, those customers look rich. I bet they tip really well. Or maybe you can take their money when you eat them..."; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
