using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingPrompt : MonoBehaviour {

	// deactivates the gameobject (the eating ui) if eating is disabled
	private void Start() {
		if (InputManager.Instance.DisableEating)
			gameObject.SetActive (false);
	}
}
