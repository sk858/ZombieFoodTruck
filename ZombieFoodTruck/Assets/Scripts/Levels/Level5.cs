using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Normal, 1, 0, 4, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 16, 20, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 26, 30, new Food.FoodType[]{Food.FoodType.Burger}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 36, 40, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 2, 46, 50, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 56, 60, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 68, 72, new Food.FoodType[]{Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 68, 72, new Food.FoodType[]{Food.FoodType.Burger}, 1),
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
		get { return 75; }
	}

	public override float DayLength
	{
		get { return 72; }
	}

	public override int LevelID
	{
		get { return 5; }
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
//		get { return "Looks like all your hard zombie work has paid off. Now you have even more energy to get through the day!"; }
		get { return "Seems like your meals have been well-received, but aren't that filling. Maybe they want... two food items?"; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
