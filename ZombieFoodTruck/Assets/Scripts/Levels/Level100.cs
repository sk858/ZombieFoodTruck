using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level100 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Normal, 1, 5, 6),
				new CustomerBatch(Customer.CustomerType.Buff, 1, 36, 38),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 100, 101),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 150, 165),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 173, 175, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Fries}, 1),
				new CustomerBatch(Customer.CustomerType.Rich, 1, 181, 182, new Food.FoodType[]{Food.FoodType.Burger, Food.FoodType.Burger}, 1)
			};
		}
	}


	public override CustomerBatch[][] KnockoutGroups
	{
		get
		{
			return new CustomerBatch[][]
			{
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Rich,1,1,2),
					new CustomerBatch(Customer.CustomerType.Buff,1,1,2)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Police,1,3,4),
					new CustomerBatch(Customer.CustomerType.Police,1,25,26)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Buff,1,12,14),
					new CustomerBatch(Customer.CustomerType.Rich,1,12,14)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Rich,1,23,24),
					new CustomerBatch(Customer.CustomerType.Buff,1,23,24)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Normal,1,34,35),
					new CustomerBatch(Customer.CustomerType.Normal,1,39,44)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Police,1,55,56),
					new CustomerBatch(Customer.CustomerType.Police,1,145,146)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Rich,1,53,54),
					new CustomerBatch(Customer.CustomerType.Rich,1,57,62)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Police,1,81,82),
					new CustomerBatch(Customer.CustomerType.Police,1,96,97)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Buff,1,79,80),
					new CustomerBatch(Customer.CustomerType.Buff,1,102,104)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Normal,1,85,86),
					new CustomerBatch(Customer.CustomerType.Rich,1,85,86)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Normal,1,98,99),
					new CustomerBatch(Customer.CustomerType.Normal,1,105,112)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Normal,1,123,124),
					new CustomerBatch(Customer.CustomerType.Buff,1,120,122)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Rich,1,132,134),
					new CustomerBatch(Customer.CustomerType.Buff,1,132,134)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Buff,1,146,147),
					new CustomerBatch(Customer.CustomerType.Rich,1,146,147)
				},
				new CustomerBatch[] 
				{
					new CustomerBatch(Customer.CustomerType.Buff,1,148,149),
					new CustomerBatch(Customer.CustomerType.Buff,1,166,167)
				},
			};
		}
	}

	public override int Quota
	{
		get { return 200; }
	}

	public override float DayLength
	{
		get { return 185; }
	}

	public override int LevelID
	{
		get { return 100; }
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
		get { return "Get ready for a bonus level that's scrambled, randomized, and served fresh off the grill! Play multiple times for a different experience!"; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
