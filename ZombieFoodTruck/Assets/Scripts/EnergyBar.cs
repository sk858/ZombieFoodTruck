using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{

	[SerializeField] 
	private GameObject[] m_spriteMasks;

	[SerializeField] 
	private GameObject[] m_energyBars;

	private int m_size;

	private int m_currentValue;

	private int m_oldVal;

	private int m_newVal;

	private AudioSource m_audioSource;

	private Animator m_animator;

	[SerializeField]
	private float m_timeBtwDecAnim;

	[SerializeField] 
	private float m_timeBtwIncAnim;
	
	
	
	
	// Use this for initialization
	void Awake ()
	{
		m_size = m_spriteMasks.Length;
		m_audioSource = GetComponent<AudioSource>();
		
		for (int i = 0; i < m_spriteMasks.Length; i++)
		{
			m_spriteMasks[i].SetActive(true);
		}
	}

	public void SetValue(int newVal, bool skipAnim)
	{
		if (newVal > m_size || newVal == m_currentValue)
		{
			return;
		}
		if (skipAnim)
		{
			SetValWithoutAnim(newVal);
			return;
		}
/*		for (int i = 0; i < m_spriteMasks.Length; i++)
		{
			if (i >= newVal)
			{
				m_spriteMasks[i].SetActive(true);
			}
			else
			{
				m_spriteMasks[i].SetActive(false);
			}
		}*/
		StopCoroutine("UpdateBar");
		m_oldVal = m_currentValue;
		m_newVal = newVal;
		StartCoroutine("UpdateBar");
	}

	public void SetValWithoutAnim(int newVal)
	{
		for (int i = 0; i < m_spriteMasks.Length; i++)
		{
			if (i >= newVal)
			{
				m_spriteMasks[i].SetActive(true);
			}
			else if(i < newVal)
			{
				m_spriteMasks[i].SetActive(false);
			}
		}
		m_currentValue = newVal;
	}

	IEnumerator UpdateBar()
	{
		m_audioSource.pitch = 1;
		int count = m_oldVal;
		if (m_newVal > count)
		{
		//	m_animator.SetTrigger("Add");
			while (count < m_newVal)
			{
				m_spriteMasks[count].SetActive(false);
				m_audioSource.Play();
				m_audioSource.pitch += 0.1f;
				count++;
				yield return new WaitForSeconds(m_timeBtwIncAnim);
			}
		}
		else
		{
		//	m_animator.SetTrigger("Remove");
			while (count > m_newVal)
			{
				count--;
				m_spriteMasks[count].SetActive(true);
				yield return new WaitForSeconds(m_timeBtwDecAnim);
			}
		}
		m_currentValue = m_newVal;
	}

	public void SetColor(Color color)
	{
		for (int i = 0; i < m_energyBars.Length; i++)
		{
			m_energyBars[i].GetComponent<SpriteRenderer>().color = color;
		}
	}
	
}
