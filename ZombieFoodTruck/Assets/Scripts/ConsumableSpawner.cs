using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableSpawner : MonoBehaviourSingleton<ConsumableSpawner> {
	
	private Dictionary<Customer.CustomerType, GameObject> m_consumableMap;

	[SerializeField] private GameObject m_patienceBuff;
	[SerializeField] private GameObject m_walletDrop;

	[SerializeField] private GameObject m_consumableAlert;
	[SerializeField] private GameObject m_consumableAlertText;

	[SerializeField] private Transform m_deadBodyTransform;

	private Vector3 m_deadBodyOffset = new Vector3(.5f, 1, 0);


	private void DisplayAlert(string msg)
	{
		m_consumableAlertText.GetComponent<Text>().text = msg;
		LeanTween.moveX(m_consumableAlert, 400f, .4f).setEase(LeanTweenType.easeInOutQuad);
		LeanTween.moveX(m_consumableAlert, 1200f, .4f).setEase(LeanTweenType.easeInOutQuad).setDelay(2f).setOnComplete(
			() => m_consumableAlert.transform.position = new Vector3(-800f, 300f, 0)
		);
	}
	
	public void OnBodyCleaned(Customer.CustomerType bodyType)
	{
		if (m_consumableMap.ContainsKey(bodyType))
		{
			GameObject consumableDrop = Instantiate(m_consumableMap[bodyType]);
//			DisplayAlert(consumableDrop.GetComponent<BaseConsumable>().GetAlertMessage());
			MoneyManager.Instance.ShowMoneyChange(m_walletDrop.GetComponent<WalletDrop>().GetMoneyAward(), m_deadBodyTransform.position + m_deadBodyOffset, Color.clear);
		}
		EatingMechanicManager.Instance.OnBodyCleaned(bodyType);
	}

	// Use this for initialization
	void Start () {
//		m_consumableMap = new Dictionary<Customer.CustomerType, GameObject>
//		{
//			{Customer.CustomerType.Normal, (GameObject) Resources.Load(ConsumablePrefabPath + "PatienceBuff")},
//			{Customer.CustomerType.Rich, (GameObject) Resources.Load(ConsumablePrefabPath + "PatienceBuff")},
//			{Customer.CustomerType.Smelly, (GameObject) Resources.Load(ConsumablePrefabPath + "PatienceBuff")},
//			{Customer.CustomerType.ZombieHunter, (GameObject) Resources.Load(ConsumablePrefabPath + "PatienceBuff")},
//		};
		m_consumableMap = new Dictionary<Customer.CustomerType, GameObject>
		{
//			{Customer.CustomerType.Normal, m_patienceBuff},
			{Customer.CustomerType.Rich, m_walletDrop},
//			{Customer.CustomerType.Smelly, m_patienceBuff},
//			{Customer.CustomerType.ZombieHunter, m_patienceBuff},
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
