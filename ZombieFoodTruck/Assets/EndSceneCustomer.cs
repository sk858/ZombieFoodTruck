using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneCustomer : MonoBehaviour
{

	[SerializeField]
	private Sprite m_sprite;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetBloody()
	{
		if (m_sprite != null)
		{
			GetComponentInChildren<SpriteRenderer>().sprite = m_sprite;
		}
	}
	
	
	public void Die()
	{
		gameObject.SetActive(false);
	}
}
