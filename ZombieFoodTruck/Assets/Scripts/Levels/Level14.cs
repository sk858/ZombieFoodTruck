using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level14 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Police, 1, 3, 4, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 5, 6, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Police, 1, 10, 11, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 27, 28, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 34, 36, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 38, 39, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 41, 42, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 59, 60, new Food.FoodType[]{Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 73, 74, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Police, 1, 78, 79, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 94, 95, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 97, 98, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 112, 113, new Food.FoodType[]{Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 114, 115, new Food.FoodType[]{Food.FoodType.Drink, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Police, 1, 125, 127, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 135, 136, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 147, 148, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 153, 154, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
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
		get { return 165; }
	}

	public override float DayLength
	{
		get { return 155; }
	}

	public override int LevelID
	{
		get { return 14; }
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
		get { return "More cops are on the way!\nBut hey, you're on a roll, so don't let a little law enforcement stop you"; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
