using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0 : BaseLevel
{

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				new CustomerBatch(Customer.CustomerType.Normal, 1, 0, 3, 1),
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
		get { return 10; }
	}

	public override float DayLength
	{
		get { return 15; }
	}

	public override int LevelID
	{
		get { return 0; }
	}

	public override List<Food.FoodType> FoodStations
	{
		get
		{
			return new List<Food.FoodType>
			{
				Food.FoodType.Burger,
			};
		}
	}

	public override int MaxOrder
	{
		get { return 1; }
	}

	public override bool DisableEating 
	{
		get { return true; }
	}

	public override string StartMessage
	{
		get { return "Look! It's a guy! He wants your food! Serve it to him faster for a better tip!"; }
	}

	public override bool TimeMeter
	{
		get { return true; }
	}
}
