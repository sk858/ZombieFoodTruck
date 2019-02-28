using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level11 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Normal, 1, 0, 4, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 10, 12, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 16, 20, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 26, 30, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 40, 44, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 52, 54, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 54, 58, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries, }, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 67, 68, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 74, 75, new Food.FoodType[]{Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 76, 77, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 88, 89, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 90, 92, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 108, 109, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 112, 114, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
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
		get { return 150; }
	}

	public override float DayLength
	{
		get { return 115; }
	}

	public override int LevelID
	{
		get { return 11; }
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
		get { return "Your truck's gone viral! Now you have even more energy to keep up with the traffic!"; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
