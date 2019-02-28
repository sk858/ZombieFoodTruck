using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Rich, 1, 0, 4, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 13, 17, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 24, 28, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 38, 42, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 2, 48, 52, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 60, 64, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 70, 74, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 74, 80, new Food.FoodType[]{Food.FoodType.Burger}, 1),
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
		get { return 85; }
	}

	public override float DayLength
	{
		get { return 80; }
	}

	public override int LevelID
	{
		get { return 6; }
	}

	public override List<Food.FoodType> FoodStations
	{
		get
		{
			return new List<Food.FoodType>
			{
				Food.FoodType.Burger,
				Food.FoodType.Fries,
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

		get { return "Looks like all your hard zombie work has paid off. Now you have even more energy to get through the day!"; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
