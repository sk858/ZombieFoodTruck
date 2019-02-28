using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelSpawn : MonoBehaviour
{

	[SerializeField]
	private LeanTweenType m_leanTweenType;

	[SerializeField]
	private float m_startFloat;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AnimateIn(float delay = 0f, float target = 0f, UnityAction onComplete = null)
	{
		gameObject.SetActive(true);
		LeanTween.cancel(gameObject);
		Vector3 start = transform.localPosition + (Vector3.up * m_startFloat);
		transform.localPosition = start;
		LeanTween.moveLocalY(gameObject, target, 0.5f).setEase(LeanTweenType.easeOutBack).setDelay(delay).setOnComplete(() =>
		{
			if (onComplete != null)
			{
				onComplete.Invoke();
			}
		}).setUseEstimatedTime(true);
	}

	public void AnimateOut(float delay = 0f, UnityAction onComplete = null)
	{
		LeanTween.cancel(gameObject);
		LeanTween.moveLocalY(gameObject, m_startFloat, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
		{
			gameObject.SetActive(false);
			if (onComplete != null)
			{
				onComplete.Invoke();
			}
		}).setUseEstimatedTime(true);
	}
}
