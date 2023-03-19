using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//~~~Definitions~~~

	//~~~Variables~~~
	//Serialized
	[SerializeField]
	private Transform m_cameraTransform;
	[SerializeField]
	private Transform m_movementReference;
	[SerializeField]
	private GameObject m_instructionsPanel;

	//Non-Serialized
	public static int MoveableLayerMask { get; private set; }


	//~~~Accessors~~~
	public Moveable HeldObject { get; private set; }
	public Transform CameraTransform { get => m_cameraTransform; }
	public Transform MovementReference { get => m_movementReference; }
	public GameObject InstructionsPanel { get => m_instructionsPanel; }


	//~~~Unity Functions~~~

	void Start()
	{
		MoveableLayerMask = LayerMask.GetMask("Moveable");
		Cursor.lockState = CursorLockMode.Locked;
	}
	void Update()
	{
		if (HeldObject != null)
		{
			HeldObject.UpdatePosition(m_movementReference.position);
		}
	}

	//~~~Runtime Functions~~~

	public void ReleaseHeldMoveable()
	{
		if (HeldObject == null || HeldObject.HasCollision())
			return;

		//Debug.Log("Letting go of " + HeldObject.name);
		HeldObject.transform.SetParent(null, true);
		HeldObject = null;
	}

	public void GrabMoveable(Moveable a_moveable)
	{
		if (HeldObject != null)
		{
			Debug.LogError("We're already holding on to something, can't grab anything else");
			return;			
		}

		if (a_moveable == null)
			return;

		//Grab the focused object
		HeldObject = a_moveable;
		m_movementReference.position = HeldObject.transform.position;
		//Debug.Log("Grabbing " + HeldObject.name);
	}

	//~~~Callback Functions~~~

	//~~~Editor Functions~~~
#if UNITY_EDITOR

#endif

}
