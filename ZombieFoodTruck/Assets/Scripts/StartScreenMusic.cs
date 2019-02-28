using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenMusic : MonoBehaviour
{

	[SerializeField]
	private AudioClip m_startClip;
	
	[SerializeField]
	private AudioClip m_loopClip;

	[SerializeField] private AudioSource m_loopSource;

	private AudioSource m_audioSource;
	
	// Use this for initialization
	void Awake ()
	{
		m_audioSource = GetComponent<AudioSource>();
		m_audioSource.clip = m_startClip;
		m_audioSource.Play();
	}

	void Update()
	{
		if (!m_audioSource.isPlaying)
		{
			m_audioSource.clip = m_loopClip;
			m_audioSource.Play();
		}
	}
}
