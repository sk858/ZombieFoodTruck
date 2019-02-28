using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRTileDetector : MonoBehaviour
{

	private DDRGame m_ddrGame;
	// Use this for initialization
	void Awake ()
	{
		m_ddrGame = GetComponentInParent<DDRGame>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("DDRTile"))
		{
			m_ddrGame.TileToHit = other.GetComponent<DDRTile>();
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("DDRTile"))
		{
			m_ddrGame.TileToHit = null;
		}	
	}
}
