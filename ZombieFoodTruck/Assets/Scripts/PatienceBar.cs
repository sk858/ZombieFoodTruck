using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{

	[SerializeField]
	private Image m_barBackgroundImage;
	
	[SerializeField]
	private Image m_iconImage;

	[SerializeField] 
	private Image m_fill;

	[SerializeField] 
	private Color m_green;

	[SerializeField] 
	private Color m_yellow;

	[SerializeField] 
	private Color m_red;
	
	private Sprite m_greenBar;

	private Sprite m_yellowBar;

	private Sprite m_redBar;

	private Sprite m_greenIcon;

	private Sprite m_yellowIcon;
	
	private Sprite m_redIcon;

	private float m_maxPatience;

	private float m_patience;

	//3 is green, 2 is yellow, 1 is red
	private int m_patienceLevel;

	public Color Green
	{
		get { return m_green; }
	}

	public Color Yellow
	{
		get { return m_yellow; }
	}
	
	public Color Red
	{
		get { return m_red; }
	}
	
	// Use this for initialization
	void Awake ()
	{
		m_greenBar = m_barBackgroundImage.sprite;
		m_greenIcon = m_iconImage.sprite;
		m_yellowBar = (Sprite)Resources.Load<Sprite>("Sprites/yellow_patience_bar");
		m_redBar = (Sprite)Resources.Load<Sprite>("Sprites/red_patience_bar");
		m_yellowIcon = (Sprite)Resources.Load<Sprite>("Sprites/yellow_patience_icon");
		m_redIcon = (Sprite)Resources.Load<Sprite>("Sprites/red_patience_icon");
	}

	public void Init(float maxPatience, float patience)
	{
		m_maxPatience = maxPatience;
		m_patience = patience;
	}
	
	// Update is called once per frame
	void Update ()
	{
		int level = m_patienceLevel;
		if (m_patience <= m_maxPatience * 0.33f)
		{
			level = 1;
		}
		else if (m_patience <= (2f * m_maxPatience) * 0.33f)
		{
			level = 2;
		}
		else
		{
			level = 3;
		}
		if (level != m_patienceLevel)
		{
			if (level == 1)
			{
				m_iconImage.sprite = m_redIcon;
				m_barBackgroundImage.sprite = m_redBar;
				m_fill.color = m_red;
			}
			else if (level == 2)
			{
				m_iconImage.sprite = m_yellowIcon;
				m_barBackgroundImage.sprite = m_yellowBar;
				m_fill.color = m_yellow;
			}
			else
			{
				m_iconImage.sprite = m_greenIcon;
				m_barBackgroundImage.sprite = m_greenBar;
				m_fill.color = m_green;
			}
			m_patienceLevel = level;
		}
	}

	public void SetPatience(float patience)
	{
		m_patience = patience;
	}
}
