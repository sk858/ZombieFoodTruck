using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingSequenceManager : MonoBehaviourSingleton<EndingSequenceManager>
{

	[SerializeField] private GameObject m_lightOverlay;

	[SerializeField] private PanelSpawn m_congratulations;

	[SerializeField] private GameObject m_confetti;

	[SerializeField] private GameObject m_firstPlace;

	[SerializeField] private GameObject[] m_customers;

	[SerializeField] private GameObject m_worker;

	[SerializeField] private GameObject m_dim;

	[SerializeField] private GameObject m_dim2;

	[SerializeField] private GameObject m_dim3;

	[SerializeField] private GameObject m_dim4;

	[SerializeField] private GameObject m_dim5;

	[SerializeField] private GameObject m_dim6;

	[SerializeField] private GameObject m_sangmin;

	[SerializeField] private GameObject m_actualWorker;

	[SerializeField] private AudioClip[] m_bodySounds;

	[SerializeField] private AudioClip m_bigSwitch;

	private AudioSource m_audioSource;
	
	private Animator m_workerAnimator;

	private Text m_congratsText;

	private bool m_started;

	public bool Started
	{
		get { return m_started; }
	}

	// Use this for initialization
	void Start ()
	{
		m_audioSource = GetComponent<AudioSource>();
		m_workerAnimator = m_worker.GetComponent<Animator>();
		m_congratsText = m_congratulations.GetComponentInChildren<Text>();
	//	StartSequence();
	//	StartCoroutine(Sequence());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartSequence()
	{
		m_started = true;
		StartCoroutine(Sequence());
	}

	IEnumerator Sequence()
	{
		GameMusicManager.Instance.StopMusic();
		UIAnimationStartManager.Instance.AnimateOut();
		yield return new WaitForSeconds(1f);
		m_actualWorker.SetActive(false);
		m_worker.SetActive(true);
		m_lightOverlay.SetActive(true);
		m_firstPlace.SetActive(true);
		yield return new WaitForSeconds(3f);
		GameMusicManager.Instance.HappyMusic();
		m_congratulations.AnimateIn(1f, 1.63f, () =>
		{
			m_confetti.SetActive(true);
			m_workerAnimator.SetTrigger("Dance");
		});
		yield return new WaitForSeconds(12f);
		m_dim.SetActive(true);
		m_congratsText.text = "but...";
		m_audioSource.clip = m_bigSwitch;
		m_audioSource.Play();
		yield return new WaitForSeconds(1f);
		m_workerAnimator.SetTrigger("EndDance");
		yield return new WaitForSeconds(2f);
		GameMusicManager.Instance.SadMusic();
		m_dim2.SetActive(true);
		for (int i = 0; i < m_customers.Length; i++)
		{
			m_customers[i].SetActive(true);
			yield return new WaitForSeconds(2.25f);
		}
		yield return new WaitForSeconds(0.75f);
		m_workerAnimator.SetTrigger("Turn");
		yield return new WaitForSeconds(2f);
		for (int i = 0; i < m_customers.Length; i++)
		{
			m_customers[i].GetComponent<Animator>().SetTrigger("Bloody");
			m_audioSource.clip = m_bodySounds[i];
			m_audioSource.Play();
			yield return new WaitForSeconds(1.5f);
		}
		yield return new WaitForSeconds(0.75f);
		m_workerAnimator.SetTrigger("EndTurn");
		yield return new WaitForSeconds(1f);
		m_workerAnimator.SetTrigger("Fall");
		yield return new WaitForSeconds(1f);
		m_congratsText.text = "Was it worth it?";
		m_dim3.SetActive(true);
		yield return new WaitForSeconds(3f);
		for (int i = 0; i < m_customers.Length; i++)
		{
			m_customers[i].GetComponent<Animator>().SetTrigger("FadeOut");
			yield return new WaitForSeconds(1.5f);
		}
		yield return new WaitForSeconds(2f);
		m_sangmin.SetActive(true);
		yield return new WaitForSeconds(6f);
		m_sangmin.GetComponent<Animator>().SetTrigger("FadeOut");
		yield return new WaitForSeconds(0.5f);
		m_workerAnimator.SetTrigger("EndKneel");
		yield return new WaitForSeconds(1.5f);
		m_dim4.SetActive(true);
		yield return new WaitForSeconds(2f);
		m_dim6.SetActive(true);
		yield return new WaitForSeconds(2f);
		m_dim5.SetActive(true);
		yield return new WaitForSeconds(2f);
		GameManagerUser.Instance.GoToUpgradeScreen(true);
	}
}
