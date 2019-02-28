using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level10 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Normal, 1, 2, 3, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 12, 14, new Food.FoodType[]{Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 15, 16, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 27, 29, new Food.FoodType[]{Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 34, 35, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 42, 46, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 47, 48, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 61, 62, new Food.FoodType[]{Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 63, 64, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 76, 77, new Food.FoodType[]{Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 78, 79, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 90, 92, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
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
		get { return 125; }
	}

	public override float DayLength
	{
		get { return 92; }
	}

	public override int LevelID
	{
		get { return 10; }
	}

	public override List<Food.FoodType> FoodStations
	{
		get
		{
			return new List<Food.FoodType>
			{
				Food.FoodType.Burger,
				Food.FoodType.Fries,
				Food.FoodType.Drink,
			};
		}
	}

	public override int MaxOrder
	{
		get { return 3; }
	}

	public override bool DisableEating 
	{
		get { return false; }
	}

	public override string StartMessage
	{
		get { return "Two items is not enough... The people want more! Get ready for bigger orders!"; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
