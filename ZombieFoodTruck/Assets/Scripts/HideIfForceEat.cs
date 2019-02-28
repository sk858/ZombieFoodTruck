using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfForceEat : MonoBehaviour {

	// deactivates the gameobject (the eating ui) if eating is forced
	private void Start() {
		Debug.Log("AAAAAAAAAAAA");
		if (InputManager.Instance.ForceEat)
			gameObject.SetActive (false);
	}
}
