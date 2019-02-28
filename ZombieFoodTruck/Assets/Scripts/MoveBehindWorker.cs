using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehindWorker : MonoBehaviour
{
	private Transform m_workerBase;

	[SerializeField]
	private Transform m_refTrans;

	private Vector3 m_refPoint;

	private SpriteRenderer m_spriteRenderer;

	private int m_origOrder;

	private string m_origLayer;
	
	// Use this for initialization
	void Start ()
	{
		m_workerBase = WorkerManager.Instance.SelectedWorker.BaseTrans;
		m_refPoint = m_refTrans.position;
		m_spriteRenderer = GetComponent<SpriteRenderer>();
		m_origLayer = m_spriteRenderer.sortingLayerName;
		m_origOrder = m_spriteRenderer.sortingOrder;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_workerBase.position.y > m_refPoint.y)
		{
			m_spriteRenderer.sortingLayerName = "Default";
		}
		else
		{
			m_spriteRenderer.sortingLayerName = m_origLayer;
			m_spriteRenderer.sortingOrder = m_origOrder;

		}
	}
}
