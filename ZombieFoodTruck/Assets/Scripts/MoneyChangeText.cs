using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyChangeText : MonoBehaviour
{

	[SerializeField] private Vector3 m_fadeSpeed;
	[SerializeField] private float m_fadeDuration;

	private float m_startT;
	private Color m_startColor;
	
	
	// Use this for initialization
	void Start ()
	{
		m_startT = Time.time;
		m_startColor = gameObject.GetComponent<Text>().color;
//		gameObject.GetComponent<Text>().CrossFadeAlpha(1, m_fadeDuration, false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.localPosition = transform.localPosition + (m_fadeSpeed*Time.deltaTime);

		float prog = (Time.time - m_startT) / m_fadeDuration;
		if (prog >= 1)
		{
			Destroy(gameObject);
		}
		else
		{
			gameObject.GetComponent<Text>().color = m_startColor * new Color(1, 1, 1, 1-prog);
		}
	}
}
