using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicStarter : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		if (!GameMusicManager.Instance.Started)
		{
			AudioSource audioSource = GetComponentInChildren<AudioSource>();
			GameMusicManager.Instance.Init(audioSource);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
