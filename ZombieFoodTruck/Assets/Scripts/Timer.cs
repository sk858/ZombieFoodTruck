using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

	private Slider m_slider;

	[SerializeField]
	private Image m_image;
	
	// Use this for initialization
	void Awake ()
	{
		m_slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetTime(float time)
	{
		transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(0, -360, time));
		m_image.fillAmount = time;
	}
}
