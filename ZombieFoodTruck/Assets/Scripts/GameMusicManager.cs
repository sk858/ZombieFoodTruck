using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicManager : MonoBehaviourSingleton<GameMusicManager>
{

	private AudioSource m_audioSource;

	private AudioClip[] m_introClips;

	private AudioClip[] m_loopClips;

	private AudioClip m_congrats;

	private AudioClip m_sadStart;

	private AudioClip m_sadLoop;

	private bool m_started;

	private int m_clipNumber;

	private int m_loopCounter;

	private int m_loopLimit = 1;

	private bool m_inIntro;

	private bool m_inited;

	private bool m_inEnd;

	private bool m_inSad;

	private bool m_stopped;

	public bool Started
	{
		get { return m_started; }
	}

	public void Init(AudioSource audioSource)
	{
		m_inited = true;
		m_introClips = new AudioClip[2];
		m_loopClips = new AudioClip[2];
		m_introClips[0] = (AudioClip)Resources.Load("Music/track_1_start");
		m_loopClips[0] = (AudioClip)Resources.Load("Music/track_1_loop");
		m_introClips[1] = (AudioClip) Resources.Load("Music/track_2_start");
		m_loopClips[1] = (AudioClip) Resources.Load("Music/track_2_loop");
		m_congrats = (AudioClip) Resources.Load("Music/congratsmusic2");
		m_sadStart = (AudioClip) Resources.Load("Music/sad_track_intro");
		m_sadLoop = (AudioClip) Resources.Load("Music/sad_track_loop");
		m_audioSource = audioSource;
		m_audioSource.transform.parent = transform;
		m_started = true;
		PlayMusic();
	}

	void Update()
	{
		if (m_stopped)
		{
			return;
		}
		if (m_inEnd)
		{
			if (m_inSad && !m_audioSource.isPlaying)
			{
				m_audioSource.clip = m_sadLoop;
				m_audioSource.Play ();
			}
			return;
		}
		if (m_inited && !m_audioSource.isPlaying)
		{
			if (m_inIntro)
			{
				m_audioSource.clip = m_loopClips[m_clipNumber];
				m_audioSource.Play ();
				m_inIntro = false;
				return;
			}
			m_loopCounter++;
			if (m_loopCounter == m_loopLimit)
			{
				m_loopCounter = 0;
				m_clipNumber = (m_clipNumber + 1) % m_introClips.Length;
				PlayMusic();
			}
			else
			{
				m_audioSource.Play();
			}
		}
	}
	
	public void PlayMusic ()
	{
		m_audioSource.clip = m_introClips[m_clipNumber];
		m_audioSource.loop = false;
		m_audioSource.Play ();
		m_inIntro = true;
	}
	

	public void StopMusic()
	{
		m_audioSource.Stop ();
		m_stopped = true;
	}

	public void HappyMusic()
	{
		m_stopped = false;
		m_inEnd = true;
		m_audioSource.clip = m_congrats;
		m_audioSource.Play();
	}

	public void SadMusic()
	{
		m_audioSource.clip = m_sadStart;
		m_audioSource.Play();
		m_inSad = true;
	}
}
