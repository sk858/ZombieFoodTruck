using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLevel : MonoBehaviour
{
	
	public class CustomerBatch
	{
		// Type and amount of customer to spawn
		public Customer.CustomerType CustomerType;
		public int Amount;
		
		// To be spawned at a random time between StartTime and EndTime (in seconds)
		public float StartTime;
		public float EndTime;

		// food to order, if specified
		public Food.FoodType[] FoodOrder = null;
		
		// Size of the marker on day bar (0 if no marker)
		public int MarkerSize = 0;

		/// <summary>
		/// Initializes a new CustomerBatch.
		/// </summary>
		/// <param name="type">Type of customer to spawn.</param>
		/// <param name="amt">Amount of customer to spawn.</param>
		/// <param name="startT">Earliest possible time to spawn.</param>
		/// <param name="endT">Latest possible time to spawn.</param>
		public CustomerBatch(Customer.CustomerType type, int amt, float startT, float endT, int markerSize = 0)
		{
			CustomerType = type;
			Amount = amt;
			StartTime = startT;
			EndTime = endT;
			MarkerSize = markerSize;
		}

		public CustomerBatch(Customer.CustomerType type, int amt, float startT, float endT, Food.FoodType[] foodOrder, int markerSize = 0)
		{
			CustomerType = type;
			Amount = amt;
			StartTime = startT;
			EndTime = endT;
			FoodOrder = foodOrder;
			MarkerSize = markerSize;
		}
	}

	// List of customer batches to spawn during the day
	public abstract CustomerBatch[] CustomerBatches { get; }

	// List of customer batch sets where only one batch from each set is randomly selected to be spawned
	public abstract CustomerBatch[][] KnockoutGroups { get; }
	
	// money quota for this level
	public abstract int Quota { get; }
	
	// the length of this level in seconds
	public abstract float DayLength { get; }
	
	// the ID of this level
	public abstract int LevelID { get; }
	
	// the food stations allowed for this level
	public abstract List<Food.FoodType> FoodStations { get; }   
	
	// max number of items per order this level
	public abstract int MaxOrder { get; }

	// whether or not eating is enabled for this level
	public abstract bool DisableEating { get; }
	
	// message to be displayed at the start of the day
	public abstract string StartMessage { get; }

	// whether or not the time meter should be present for this level
	// (we may deactivate the time meter if we want the TutorialManager to handle when customers arrive and when the day ends
	public abstract bool TimeMeter { get; }
	
}
