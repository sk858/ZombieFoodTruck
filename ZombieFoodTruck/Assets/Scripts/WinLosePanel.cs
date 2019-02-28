using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLosePanel : MonoBehaviour
{

	[SerializeField] private Text m_winText;
	
	[SerializeField] private Text m_loseText;

	[SerializeField] private string m_victoryText;
	
	[SerializeField] private string m_gameOverText;

	private string m_gameEndText = "Ok, you won.\nCongrats and stuff.\nBut think about all the lives you ended along the way.\nWas it worth it?";

	[SerializeField] private Button m_successProceedButton;

	[SerializeField] private Button m_failProceedButton;

	[SerializeField] private GameObject m_successScreen;

	[SerializeField] private GameObject m_failScreen;

	[SerializeField] private GameObject m_coinSystem;

	[SerializeField] private GameObject[] m_confettiSystems;

	// For money animation
	[SerializeField] private GameObject m_moneyFrame;
	[SerializeField] private GameObject m_moneyText;
	[SerializeField] private float m_moneyCountStep;
	[SerializeField] private Color m_moneyFlashColor;
	[SerializeField] private float m_moneyFlashStep;
	[SerializeField] private float m_moneyFlashTimes;
	
	private System.Random rng = new System.Random();
	
	// Use this for initialization
	void Start () 
	{
		UnityEngine.Random.InitState((int)Time.time);
		gameObject.SetActive(false);
		m_successScreen.gameObject.SetActive(false);
		m_failScreen.gameObject.SetActive(false);
		GameManagerUser.Instance.OnVictory.AddListener(PanelVictory);
		GameManagerUser.Instance.OnGameStart.AddListener(PanelStart);
		GameManagerUser.Instance.OnGameOver.AddListener(PanelGameOver);
		
	}
	
	// Called on game start
	void PanelStart(BaseLevel level, bool restart)
	{
		gameObject.SetActive(false);
	}
	
	// Called on a victory
	void PanelVictory()
	{
		MenuInputManager.Instance.OnWinLossEnter(true);
		int level = PlayerStatistics.Instance.CurrentLevel;
		Debug.Log(level);
		Debug.Log(GameManagerUser.Instance.NumberLevels);
		if (level == (GameManagerUser.Instance.NumberLevels-1))
		{
			if (PlayerStatistics.Instance.RandomMode)
			{
				m_winText.text = m_victoryText;
				gameObject.SetActive(true);
				m_successScreen.gameObject.SetActive(true);
				m_successProceedButton.gameObject.SetActive(true);
				m_coinSystem.SetActive(true);
				StartCoroutine(LaunchConfetti());
			}
			else
			{
				EndingSequenceManager.Instance.StartSequence();
				return;
			}
			m_winText.text = m_gameEndText;
			Debug.Log(GameManagerUser.Instance.NumberLevels);
			gameObject.SetActive(true);
			m_successScreen.gameObject.SetActive(true);
			m_successProceedButton.gameObject.SetActive(true);
		}
		else
		{
			m_winText.text = m_victoryText;
			gameObject.SetActive(true);
			m_successScreen.gameObject.SetActive(true);
			m_successProceedButton.gameObject.SetActive(true);
			m_coinSystem.SetActive(true);
			StartCoroutine(LaunchConfetti());
		}
		RectTransform transform = m_successScreen.GetComponent<RectTransform>();
		float original = transform.position.y;
		Vector3 start = transform.position + (Vector3.up * 6.22f);
		transform.position = start;
		LeanTween.moveY(transform, original/100f, 0.5f).setEase(LeanTweenType.easeOutBack);

		StartCoroutine(CountUpMoney());
		StartCoroutine(FlashMoneyColor());
	}

	private IEnumerator CountUpMoney()
	{
//		m_moneyText.GetComponent<Text>().text = start.ToString();

		int goalMoney = PlayerStatistics.Instance.CurrentMoney;
		
		int currText = 0;
		Int32.TryParse(m_moneyText.GetComponent<Text>().text, out currText);
		if (currText > goalMoney)
		{
			currText = 0;
		}
		
		float lastStep = Time.time;
		while (currText < goalMoney)
		{
			currText += 1;
			m_moneyText.GetComponent<Text>().text = currText.ToString();
			yield return new WaitForSeconds(m_moneyCountStep);
		}
	}

	private IEnumerator FlashMoneyColor()
	{
		for (int i = 0; i < m_moneyFlashTimes; i++)
		{
			m_moneyText.GetComponent<Text>().color = m_moneyFlashColor;
			yield return new WaitForSeconds(m_moneyFlashStep);
			m_moneyText.GetComponent<Text>().color = Color.black;
			yield return new WaitForSeconds(m_moneyFlashStep);
		}
	}

	// called on a game over
	void PanelGameOver()
	{
		MenuInputManager.Instance.OnWinLossEnter(false);
		m_loseText.text = m_gameOverText;
		gameObject.SetActive(true);
		m_failScreen.gameObject.SetActive(true);
		m_failProceedButton.gameObject.SetActive(true);
		RectTransform transform = m_failScreen.GetComponent<RectTransform>();
		float original = transform.position.y;
		Vector3 start = transform.position + (Vector3.up * 6.22f);
		transform.position = start;
		LeanTween.moveY(transform, original/100f, 0.5f).setEase(LeanTweenType.easeOutBack);
	}

	private IEnumerator LaunchConfetti()
	{
		List<GameObject> confetti = new List<GameObject>(m_confettiSystems);
		Shuffle(confetti);
		for (int i = 0; i < confetti.Count; i++)
		{
			confetti[i].SetActive(true);
			yield return new WaitForSeconds(0.2f);
		}
	}
	
	private void Shuffle<T>(List<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
}
