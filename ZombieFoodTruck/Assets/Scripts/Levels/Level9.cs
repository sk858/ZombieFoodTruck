using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level9 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Rich, 1, 0, 4, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 6, 10, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 12, 16, new Food.FoodType[]{Food.FoodType.Drink, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 16, 18, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 2, 28, 32, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 36, 40, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 44, 48, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 52, 56, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 62, 66, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 72, 76, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 82, 86, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
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
		get { return 105; }
	}

	public override float DayLength
	{
		get { return 86; }
	}

	public override int LevelID
	{
		get { return 9; }
	}

	public override List<Food.FoodType> FoodStations
	{
		get
		{
			return new List<Food.FoodType>
			{
				Food.FoodType.Burger,
				Food.FoodType.Fries,
				Food.FoodType.Drink
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
		get { return "A bunch of body builders are coming to try your food! They sure eat a lot! " +
		             "You might get an extra bit of energy when you eat them!"; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
