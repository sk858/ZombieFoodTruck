using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    
    private float m_value;

    private int m_discreteValue;

    private int m_size;
    
    [SerializeField]
    private GameObject[] m_pieces;
    
    [SerializeField]
    private float m_timeBtwDecAnim;
    
    [SerializeField] 
    private float m_timeBtwIncAnim;

    public float Value
    {
        get { return m_value; }
        set
        {
            m_value = value; 
            SetValue(value * m_size);
        }
    }

    private void Awake()
    {
        m_size = m_pieces.Length;
        for (int i = 0; i < m_pieces.Length; i++)
        {
            m_pieces[i].SetActive(false);
        }
    }
    
    public void SetValue(float newVal)
    {
        if ((int)newVal != m_discreteValue)
        {
            m_discreteValue = (int) newVal;
            for (int i = 0; i < m_pieces.Length; i++)
            {
                if (i >= m_discreteValue)
                {
                    m_pieces[i].SetActive(false);
                }
                else
                {
                    m_pieces[i].SetActive(true);
                }
            }		   
        }
        m_value = newVal;
    }
}
