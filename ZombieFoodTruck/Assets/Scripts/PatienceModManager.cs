using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatienceModManager : MonoBehaviourSingleton<PatienceModManager> {

    private float m_totalPatienceMod = 1;

    private Dictionary<string, float> m_modifiers = new Dictionary<string, float>();
	

    public void AddPatienceModifier(string id, float value)
    {
	    RemovePatienceModifier(id);
        m_modifiers.Add(id, value);
        m_totalPatienceMod *= value;
	    CustomerManager.Instance.SetPatienceDepletionMod(m_totalPatienceMod);
    }

    public void RemovePatienceModifier(string id)
    {
        float value;
        if (m_modifiers.TryGetValue(id, out value))
        {
            m_modifiers.Remove(id);
            m_totalPatienceMod /= value;
	        CustomerManager.Instance.SetPatienceDepletionMod(m_totalPatienceMod);
        }
    }

	// Use this for initialization
	void Start ()
    {
	    
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
