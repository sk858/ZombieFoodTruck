using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable()
	{
		ButtonSpawn[] buttonSpawn = GetComponentsInChildren<ButtonSpawn>();
		if (buttonSpawn != null)
		{
			for (int i = 0; i < buttonSpawn.Length; i++)
			{
				buttonSpawn[i].AnimateIn();

			}
		}
	}

/*	void OnDisable()
	{
		ButtonSpawn buttonSpawn = GetComponentInChildren<ButtonSpawn>();
		if (buttonSpawn != null)
		{
			buttonSpawn.AnimateOut();
		}
	}*/

	public void Disable()
	{
		ButtonSpawn[] buttonSpawn = GetComponentsInChildren<ButtonSpawn>();
		if (buttonSpawn != null)
		{
			for (int i = 0; i < buttonSpawn.Length; i++)
			{
				buttonSpawn[i].AnimateOut(() => gameObject.SetActive(false));

			}
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
}
