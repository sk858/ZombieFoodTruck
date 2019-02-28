using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.UIElements;

[RequireComponent(typeof(HingeJoint2D))]
public class GrabbableObject : MonoBehaviour
{

	public class GrabEvent : UnityEvent<bool>{ };

	public class DockingEvent : UnityEvent<bool, GameObject>
	{
	};

	[SerializeField] 
	private GameObject m_emptyObject;

	[SerializeField] 
	private Vector3 m_topOffset;

	private GameObject m_currentEmptyObject;
	
	private Transform m_oldParent;

	private Quaternion m_origRotation;

	
	[SerializeField]
	private bool m_grabbed;

	[SerializeField]
	private bool m_isGrabbingDisabled;

	private DockingObject m_objectDockedTo;

	public GrabEvent OnGrab = new GrabEvent();
	
	public DockingEvent OnDock = new DockingEvent();

	public bool IsGrabbingDisabled
	{
		get
		{
			return m_isGrabbingDisabled;
		}
		set
		{
			m_isGrabbingDisabled = value;
		}
	}

	public DockingObject ObjectDockedTo
	{
		get { return m_objectDockedTo; }
		set { m_objectDockedTo = value; }
	}

	public Vector3 PivotPosition
	{
		get
		{
			return transform.position + TopOffset;
		}
	}

	public Vector3 TopOffset
	{
		get
		{
			return m_topOffset;
		}
	}

	public void OnObjectGrabbed(bool isGrabbed)
	{
		m_grabbed = isGrabbed;
		if (isGrabbed)
		{
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			m_origRotation = transform.rotation;
		}
		else
		{
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		//	transform.position = transform.position + TopOffset;
			transform.rotation = m_origRotation;
		}
		OnGrab.Invoke(isGrabbed);
	}

	public void Undock()
	{
		m_objectDockedTo.Undock();
	}
	
}
