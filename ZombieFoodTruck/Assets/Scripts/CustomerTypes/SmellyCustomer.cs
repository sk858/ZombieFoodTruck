using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellyCustomer : Customer {

    [SerializeField]
    private float m_patienceModifier;

    private static float DEFAULT_PATIENCE_MOD = 1;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //TODO make it so that smelly effects stack (or so that it chooses the maximum of all smelly sources)
        //CustomerManager.Instance.SetSmellState(BodySmellManager.Smell.Yellow, m_patienceModifier);
    }

    public override void OnExit()
    {
        base.OnExit();
        //CustomerManager.Instance.SetSmellState(BodySmellManager.Smell.Clean, DEFAULT_PATIENCE_MOD);
    }
}
