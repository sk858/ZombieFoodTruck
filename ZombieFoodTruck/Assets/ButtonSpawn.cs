using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class ButtonSpawn : MonoBehaviour
{

	[SerializeField]
	private float m_delay;

	private LTDescr m_currentTween;

	[SerializeField] private bool m_animOnAwake;
	[SerializeField] private bool m_randomButton;
	[SerializeField] private float m_speed = 0.5f;
	private bool m_active;

	[SerializeField]
	private Vector3 m_originalScale;

	public bool Active
	{
		get { return m_active; }
	}

	void Awake()
	{
		if (m_originalScale == Vector3.zero)
		{
			m_originalScale = transform.localScale;
		}
	}
	
	// Use this for initialization
	void Start () {
		// if this is the random button, deactivate it if random mode isn't unlocked
		if (m_randomButton && !SaveManager.Instance.UnlockedRandomMode) {
			m_animOnAwake = false;
			gameObject.SetActive (false);
		}
		if (m_originalScale == Vector3.zero)
		{
			m_originalScale = transform.localScale;
		}

		if (!gameObject.activeSelf || !m_animOnAwake)
		{
			return;
		}
		if (m_currentTween != null)
		{
			LeanTween.cancel(m_currentTween.id);
		}
		transform.localScale = m_originalScale * 0.1f;
		gameObject.SetActive(false);
		m_currentTween = LeanTween.scale(gameObject, m_originalScale, m_speed).setEase(LeanTweenType.easeOutElastic).setDelay(m_delay).setOnStart(() => gameObject.SetActive(true))
			.setUseEstimatedTime(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AnimateIn()
	{
		if (m_originalScale == Vector3.zero)
		{
			m_originalScale = transform.localScale;
		}
		m_active = true;
		if (m_currentTween != null)
		{
			LeanTween.cancel(m_currentTween.id);
		}		
		transform.localScale = m_originalScale * 0.1f;
		gameObject.SetActive(false);
		m_currentTween = LeanTween.scale(gameObject, m_originalScale, m_speed).setEase(LeanTweenType.easeOutElastic).setDelay(m_delay).setOnStart(
				() =>
				{
					gameObject.SetActive(true);
					m_currentTween = null;
				})
			.setUseEstimatedTime(true);
	}

	public void AnimateOut(UnityAction onComplete = null)
	{
		if (m_currentTween != null)
		{
			LeanTween.cancel(m_currentTween.id);
		}
		m_currentTween = LeanTween.scale(gameObject, m_originalScale * 0.1f, m_speed).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
		{
			gameObject.SetActive(false);
			if (onComplete != null)
			{
				onComplete.Invoke();
			}
			m_currentTween = null;
		}).setUseEstimatedTime(true);
		m_active = false;
	}

	public void SetDelay(float delay)
	{
		m_delay = delay;
	}
}
