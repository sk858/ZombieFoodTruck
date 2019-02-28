using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviourSingleton<MoneyManager> {

	//Total Money

	private int m_money;

	private int m_bank;

	private int m_startingMoney;

	public Slider MoneySlider;

	public Text MoneySliderText;
	
	public int Money
	{
		get { return m_money; }
        set { m_money = value; }
	}

	public int Bank
	{
		get { return m_bank; }
		set { m_bank = value; }
	}
	
	//Quota for each day
	private int m_quota;

	public int Quota
	{
		get { return m_quota; }
		set
		{
			m_quota = value;
			//m_quotaText.text = "GOAL: $" + value;
			MoneySliderText.text = "/" + value;
		}
	}
	
	[SerializeField]
	private Text m_moneyText;

	[SerializeField] 
	private Text m_quotaText;

	[SerializeField]
	private GameObject m_moneyChangeText;

	[SerializeField] private Color m_plusMoneyColor;

	[SerializeField] private Color m_minusMoneyColor;

	[SerializeField] private Color m_greenTipColor;

	[SerializeField] private Color m_yellowTipColor;

	[SerializeField] private Color m_redTipColor;

	[SerializeField] private GameObject m_gameUI;

	[SerializeField] private GameObject m_camera;


	void Start()
	{
		Money = 0;
   //     m_moneyText.text = "$" + Money;
		GameManagerUser.Instance.OnGameStart.AddListener(OnGameStart);
		MoneySlider.value = MoneySliderCalc();
	}

	
	//Add money when order is taken from customers
	public void EarnMoney(int addMoney)
	{
		Money += addMoney;
  		// m_moneyText.text = "$" + Money;
		MoneySlider.value = MoneySliderCalc();
		MoneySliderText.text = Money + "/" + Quota;
	}

	//Remove money (i.e. tickets)
	public void RemoveMoney(int amount)
	{
		Money = Math.Max(0, Money - amount);
		MoneySlider.value = MoneySliderCalc();
		MoneySliderText.text = Money + "/" + Quota;
	}
	
	//Use money to buy upgrades
	public void UseMoney(int subtractMoney)
	{
		if (Money < subtractMoney)
		{
			Debug.Log("Not enough money to buy upgrade");
		}
		else
		{
			Money -= subtractMoney;
            m_moneyText.text = "$" + Money;
		}   
	}

	public bool CanBuy(int costOfItemToBuy)
	{
		return m_money >= costOfItemToBuy;
	}
	
	// Called at the start of any day
	private void OnGameStart(BaseLevel level, bool restart)
	{
	//		m_money = m_startingMoney;
	//		m_moneyText.text = "$" + Money;
			MoneySlider.value = MoneySliderCalc();
			MoneySliderText.text = Money + "/" + Quota;
			
	}

	float MoneySliderCalc()
	{
		float progress = (float)m_money / m_quota;
		return progress;
	}

	public void ShowMoneyChange(int amount, Transform position, Color tipColor, int tip = 0)
	{
		Vector3 screenPos = m_camera.GetComponent<Camera>().WorldToScreenPoint(position.position);
		GameObject changeText = Instantiate(m_moneyChangeText, position.position, Quaternion.identity);
		changeText.transform.parent = m_gameUI.transform;
		changeText.transform.localScale = Vector3.one;

		if (tip > 0) {
			Vector3 tipPos = position.position + new Vector3 (.5f, 0f, 0f);
			GameObject tipText = Instantiate (m_moneyChangeText, tipPos, Quaternion.identity);
			tipText.transform.parent = m_gameUI.transform;
			tipText.transform.localScale = Vector3.one;
			Text tipT = tipText.GetComponent<Text> ();
			tipT.text = "+$" + tip;
			tipT.color = tipColor;
		}
		
		Text text = changeText.GetComponent<Text>();
		if (amount < 0)
		{
			text.text = "-$" + amount;
			text.color = m_minusMoneyColor;
		}
		else
		{
			text.text = "+$" + amount;
			text.color = m_plusMoneyColor;
		}
	}
	
	public void ShowMoneyChange(int amount, Vector3 position, Color tipColor, int tip = 0)
	{
		Vector3 screenPos = m_camera.GetComponent<Camera> ().WorldToScreenPoint (position);
		GameObject changeText = Instantiate (m_moneyChangeText, position, Quaternion.identity);
		changeText.transform.parent = m_gameUI.transform;
		changeText.transform.localScale = Vector3.one;

		if (tip > 0) {
			Vector3 tipPos = position + new Vector3(.5f, 0f, 0f);
			GameObject tipText = Instantiate (m_moneyChangeText, tipPos, Quaternion.identity);
			tipText.transform.parent = m_gameUI.transform;
			tipText.transform.localScale = Vector3.one;
			Text tipT = tipText.GetComponent<Text> ();
			tipT.text = "+$" + tip;
			tipT.color = tipColor;
		}

		Text text = changeText.GetComponent<Text> ();
		if (amount < 0) {
			text.text = "-$" + amount;
			text.color = m_minusMoneyColor;
		} else {
			text.text = "+$" + amount;
			text.color = m_plusMoneyColor;
		}
	}
}
