using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : BaseLevel
{

	// this level is controlled by the TutorialManager script
	// no customer batches necessary!

	public override CustomerBatch[] CustomerBatches
	{
		get 
		{
			return new CustomerBatch[] 
			{
				
			};
		}
	}


	public override CustomerBatch[][] KnockoutGroups
	{
		get
		{
			return new CustomerBatch[][]
			{
		/*		new CustomerBatch[]
				{
					new CustomerBatch(Customer.CustomerType.Normal, 1, 23, 24), 
					new CustomerBatch(Customer.CustomerType.Normal, 1, 34, 35), 
				},*/
			};
		}
	}

	public override int Quota
	{
		get { return 20; }
	}

	public override float DayLength
	{
		get { return 60; }
	}

	public override int LevelID
	{
		get { return 1; }
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
		get { return "Looks like all this work is tiring you out. Once you run out of energy, you won't be able to cook anything until you eat someone!"; }
	}

	public override bool TimeMeter
	{
		get { return false; }
	}
}
