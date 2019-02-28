using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceEatHide : MonoBehaviour {

	// deactivates the gameobject (the eating ui) if eating is forced
	private void Start() {
		if (InputManager.Instance.ForceEat)
			gameObject.SetActive (false);
	}
}
