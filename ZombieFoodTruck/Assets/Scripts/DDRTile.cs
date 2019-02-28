using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDRTile : MonoBehaviour
{

    private string m_letter;

    private Text m_text;

    public string Letter
    {
        get { return m_letter; }
    }

    public void Init(string letter)
    {
        m_text = GetComponentInChildren<Text>();
        m_letter = letter;
        m_text.text = Letter.ToUpper();
    }
}
