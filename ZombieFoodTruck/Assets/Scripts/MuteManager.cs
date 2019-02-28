using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteManager : MonoBehaviour
{

	private bool m_isMuted;

	public bool IsMuted
	{
		get
		{
			return m_isMuted;
		}
		set
		{
			m_isMuted = value;
			if (!value)
			{
				AudioListener.volume = 1;
			}
			else
			{
				AudioListener.volume = 0;
			}
		}
	}

	public void ToggleMute()
	{
		IsMuted = !IsMuted;
	}
}
