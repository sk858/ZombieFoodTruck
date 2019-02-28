using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level13 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Normal, 1, 3, 4, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 5, 6, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Police, 1, 9, 10, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 23, 24, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 38, 39, new Food.FoodType[]{Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 45, 48, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 53, 54, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 74, 76, new Food.FoodType[]{Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Police, 1, 80, 81, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 92, 93, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 95, 96, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 108, 109, new Food.FoodType[]{Food.FoodType.Drink, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 112, 114, new Food.FoodType[]{Food.FoodType.Fries, Food.FoodType.Drink, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Police, 1, 118, 119, new Food.FoodType[]{Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 135, 136, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 139, 140, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Drink}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 148, 149, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger,}, 1),
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
		get { return 160; }
	}

	public override float DayLength
	{
		get { return 150; }
	}

	public override int LevelID
	{
		get { return 13; }
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
		get { return "The police caught wind of your\nzombie shenanigans!\nDon't let them catch you eating someone!"; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
