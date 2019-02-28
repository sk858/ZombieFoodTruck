using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelSelectPanel : MonoBehaviour {

	[SerializeField] private Dropdown m_levelDropdown;

	[SerializeField] private string m_truckScreen; // the truck scene where the game happens

	private void Start() 
	{
		int numLevels = GameManagerUser.Instance.NumberLevels;
		List<string> levelList = new List<string> ();
		for (int i = 0; i < numLevels; i++) {
			string l = "" + i;
			levelList.Add(l);
		}
		m_levelDropdown.AddOptions (levelList);
	}

	public void SelectLevel()
	{
		int level = m_levelDropdown.value;
		PlayerStatistics.Instance.CurrentLevel = level;
		SceneManager.LoadScene (m_truckScreen);
	}
}
