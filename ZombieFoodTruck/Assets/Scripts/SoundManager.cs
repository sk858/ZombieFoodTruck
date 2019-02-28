using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{

	private AudioSource m_audioSource;

	private AudioClip m_screamClip;

	private AudioClip m_cleaningClip;

	private AudioClip m_orderTakenClip;

	private AudioClip m_onOrderFinishedClip;

	private AudioClip m_iceRefillClip;

	private AudioClip m_biteClip;

	private AudioClip m_hungerClip;

	[SerializeField] 
	private float m_volumeForChopClip;

	[SerializeField] 
	private float m_volumeForOrderFinished;

	[SerializeField] private float m_volumeForScream;

	[SerializeField] private float m_volumeForBite;

	[SerializeField] private float m_volumeForIce;

	[SerializeField] private float m_volumeForClean;

	[SerializeField] private float m_volumeForHunger;
	
	private AudioClip m_chopClip;
	
	// Use this for initialization
	void Awake ()
	{
		m_audioSource = GetComponent<AudioSource>();
		m_chopClip = (AudioClip)Resources.Load("Sounds/chopping");
		m_onOrderFinishedClip = (AudioClip)Resources.Load("Sounds/money_sound");
		m_screamClip = (AudioClip) Resources.Load("Sounds/eating_scream");
		m_biteClip = (AudioClip) Resources.Load("Sounds/bite_sound");
		m_iceRefillClip = (AudioClip) Resources.Load("Sounds/load_ice");
		m_cleaningClip = (AudioClip) Resources.Load("Sounds/cleaning_anim");
		m_hungerClip = (AudioClip) Resources.Load("Sounds/zombie_hungry");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnEat()
	{
		m_audioSource.clip = m_screamClip;
		m_audioSource.Play();
		m_audioSource.volume = m_volumeForScream;
	}
	
	
	public void OnBite()
	{
		m_audioSource.clip = m_biteClip;
		m_audioSource.Play();
		m_audioSource.volume = m_volumeForBite;
	}

	public void OnEatPeopleSee()
	{
		
	}

	public void OnCleanStart()
	{
		m_audioSource.clip = m_cleaningClip;
		m_audioSource.Play();
		m_audioSource.volume = m_volumeForClean;
	}

	public void OnCleanStop()
	{
		m_audioSource.Stop();
	}

	public void OnOrderTaken()
	{
		
	}

	public void OnOrderServed()
	{
		m_audioSource.clip = m_onOrderFinishedClip;
		m_audioSource.Play();
		m_audioSource.volume = m_volumeForOrderFinished;
	}

	public void OnBurgerStart()
	{
		
	}

	public void OnBurgerEnd()
	{
		
	}

	public void OnFryerStart()
	{
		
	}

	public void OnFryerEnd()
	{
		
	}

	public void OnChop()
	{
		m_audioSource.clip = m_chopClip;
		m_audioSource.Play();
		m_audioSource.volume = m_volumeForChopClip;
	}

	public void StopChop()
	{
		m_audioSource.Stop();
	}

	public void OnDrinkMachineStart()
	{
		
	}

	public void OnDrinkMachineEnd()
	{
		
	}

	public void IceRefilled()
	{
		m_audioSource.clip = m_iceRefillClip;
		m_audioSource.Play();
		m_audioSource.volume = m_volumeForIce;
	}

	public void PlayHungerSound()
	{
		m_audioSource.clip = m_hungerClip;
		m_audioSource.Play();
		m_audioSource.volume = m_volumeForHunger;
	}
	
}
