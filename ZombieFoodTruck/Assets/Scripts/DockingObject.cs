using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DockingObject : MonoBehaviour
{	
	[SerializeField]
	private Vector3 m_offset;

	private GameObject m_objectToDock;
	
	private GameObject m_dockedObject;

	private bool m_hasObject;
	
	public GameObject DockedObject
	{
		get
		{
			return m_dockedObject;
		}
		set
		{
			m_dockedObject = value;
		}
	}

	private void Dock()
	{
		Debug.Log("Docking object");
		m_hasObject = true;
		DockedObject = m_objectToDock;
		m_objectToDock = null;
		Vector3 newPos = transform.position + m_offset;
		newPos.z = DockedObject.transform.position.z;
		DockedObject.transform.position = newPos;
		m_hasObject = true;
		GrabbableObject grabbable = DockedObject.GetComponent<GrabbableObject>();
		grabbable.OnDock.Invoke(true, this.gameObject);
		grabbable.ObjectDockedTo = this;

	}

	public void Undock()
	{
		Debug.Log("Undocking object");
		m_hasObject = false;
		GrabbableObject grabbable = DockedObject.GetComponent<GrabbableObject>();
		grabbable.OnGrab.RemoveListener(OnDockedObjectGrabChanged);
		grabbable.OnDock.Invoke(false, this.gameObject);
		grabbable.ObjectDockedTo = null;
		DockedObject = null;
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Grabbable") && !m_hasObject)
		{
			m_objectToDock = other.gameObject;
			m_objectToDock.GetComponent<GrabbableObject>().OnGrab.AddListener(OnDockedObjectGrabChanged);
		}
	}

	private void OnDockedObjectGrabChanged(bool isGrabbed)
	{
		Debug.Log("Grabbed object changed");
		if (isGrabbed)
		{
			Undock();
		}
		else
		{
			Dock();
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("Grabbable") && !m_hasObject && m_objectToDock != null)
		{
			m_objectToDock.GetComponent<GrabbableObject>().OnGrab.RemoveListener(OnDockedObjectGrabChanged);
			m_objectToDock = null;
		}
	}
}
