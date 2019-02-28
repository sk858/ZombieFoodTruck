using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{

	public GameObject UpgradeScreen;
	
	public GameObject Panel;

	[SerializeField]
	private StartPanel m_startPanel;
	
	// Use this for initialization
	void Start () {
		if (PlayerStatistics.Instance.CurrentLevel >= 3) {
			UpgradeScreen.gameObject.SetActive(true);
			RectTransform transform = Panel.GetComponent<RectTransform>();
			float original = transform.position.y;
			Vector3 start = transform.position + (Vector3.up * 8f);
			transform.position = start;
			LeanTween.moveY(transform, 0f, 0.5f).setEase(LeanTweenType.easeOutBack);
		} else {
			UpgradeScreen.gameObject.SetActive(false);
			PauseManager.Instance.OnUpgradeScreenDone();
			m_startPanel.OnUpgradeScreenDone();
		}
	}
	
	// Update is called once per frame
	public void UpdateFinished () {
		RectTransform transform = Panel.GetComponent<RectTransform>();
		PauseManager.Instance.OnUpgradeScreenDone();
		LeanTween.moveY(transform, 800f, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(
			() => UpgradeScreen.gameObject.SetActive(false)
		);
		m_startPanel.OnUpgradeScreenDone();
	}
}
