using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Normal, 1, 1, 2),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 10, 13),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 25, 28),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 31, 33),
				new CustomerBatch(Customer.CustomerType.Normal, 1, 39, 40),
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
		get { return 25; }
	}

	public override float DayLength
	{
		get { return 45; }
	}

	public override int LevelID
	{
		get { return 2; }
	}

	public override List<Food.FoodType> FoodStations
	{
		get
		{
			return new List<Food.FoodType>
			{
				Food.FoodType.Burger
			};
		}
	}

	public override int MaxOrder
	{
		get { return 1; }
	}

	public override bool DisableEating 
	{
		get { return false; }
	}

	public override string StartMessage
	{
		get { return "More customers on the way! Serve food fast for better tips! But be aware, only so many people come to eat each day..."; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}