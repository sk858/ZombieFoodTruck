using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSelectionUI : MonoBehaviour
{

	[SerializeField]
	private GameObject m_eatingButton;
	
	// Use this for initialization
	private void OnEnable()
	{
		m_eatingButton.SetActive(!InputManager.Instance.DisableEating);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
