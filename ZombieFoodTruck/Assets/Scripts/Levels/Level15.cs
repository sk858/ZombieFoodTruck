using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level15 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Buff, 1, 2, 3, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 4, 5, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 10, 11, new Food.FoodType[]{Food.FoodType.Drink, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Police, 1, 15, 17, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 34, 35, new Food.FoodType[]{Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 36, 37, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Police, 1, 42, 44, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 61, 62, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 73, 74, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 88, 89, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Police, 1, 91, 92, new Food.FoodType[]{Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 101, 103, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 113, 114, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 115, 116, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Police, 1, 128, 129, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 133, 134, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 145, 146, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Police, 1, 149, 150, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 153, 154, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Burger}, 1),
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
		get { return 175; }
	}

	public override float DayLength
	{
		get { return 155; }
	}

	public override int LevelID
	{
		get { return 15; }
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
		get { return "This is it!\nIf you perform well today, you'll have the most famous food truck in the world!"; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
