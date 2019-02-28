using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBackgroundSwapper : MonoBehaviour {

	[SerializeField] private Sprite m_startScreen;
	[SerializeField] private Sprite m_postGameStartScreen;

	// Use this for initialization
	void Start () {
		if (SaveManager.Instance.UnlockedRandomMode) {
			Image img = gameObject.GetComponent<Image> ();
			img.sprite = m_postGameStartScreen;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
