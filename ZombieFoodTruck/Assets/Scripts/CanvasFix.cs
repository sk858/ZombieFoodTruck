using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFix : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Canvas>().sortingOrder++;
		GetComponent<Canvas>().sortingOrder--; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
