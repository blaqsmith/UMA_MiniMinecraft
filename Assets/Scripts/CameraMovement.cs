using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
	//~~~Definitions~~~
	const float m_mouseSensitivityMultiplier = 0.01f;

	//~~~Variables~~~
	//Serialized
	[SerializeField]
	private GameManager m_gameManager;
	[SerializeField, Tooltip("Look speed when using the mouse")]
	private float m_lookSpeedMouse = 4.0f;
	[SerializeField, Tooltip("Movement speed")]
	private float m_moveSpeed = 10.0f;
	[SerializeField, Tooltip("Increase to snap to final position more quickly when moving (reduce \"floatyness\"")]
	private float m_lerpMultiplier = 1.0f;
	[SerializeField, Tooltip("Activating sprint mutplies move speed by this amount")]
	private float m_sprintMultiplier = 2.0f;

	//Non-Serialized
	private Vector3 m_moveTarget;
	private Vector3 m_moveDirection;
	private float m_verticalDirection;
	private bool m_sprint;
	private Vector2 m_lookInput;

	//~~~Accessors~~~

	//~~~Unity Functions~~~

	private void Start()
	{
		//Keep player starting point where the camera has been placed in the scene
		m_moveTarget = transform.position;
	}

	private void LateUpdate()
	{
		//Look Updates
		float rotationX = transform.localEulerAngles.x;
		float newRotationY = transform.localEulerAngles.y + m_lookInput.x;

		// Weird clamping code due to weird Euler angle mapping...
		float newRotationX = (rotationX - m_lookInput.y);
		if (rotationX <= 90.0f && newRotationX >= 0.0f)
			newRotationX = Mathf.Clamp(newRotationX, 0.0f, 90.0f);
		if (rotationX >= 270.0f)
			newRotationX = Mathf.Clamp(newRotationX, 270.0f, 360.0f);

		transform.localRotation = Quaternion.Euler(newRotationX, newRotationY, transform.localEulerAngles.z);

		//Movement Updates
		//Increment the new move Target position of the camera			
		if (m_sprint)
		{
			m_moveTarget += (transform.forward * m_moveDirection.z + transform.right * m_moveDirection.x) * Time.fixedDeltaTime * m_moveSpeed * m_sprintMultiplier;
			m_moveTarget.y += (m_verticalDirection * Time.fixedDeltaTime * m_moveSpeed * m_sprintMultiplier);
		}
		else
		{
			m_moveTarget += (transform.forward * m_moveDirection.z + transform.right * m_moveDirection.x) * Time.fixedDeltaTime * m_moveSpeed;
			m_moveTarget.y += (m_verticalDirection * Time.fixedDeltaTime * m_moveSpeed);
		}


		//Lerp  the camera to a new move target position
		transform.position = Vector3.Lerp(transform.position, m_moveTarget, Time.deltaTime * m_moveSpeed * m_lerpMultiplier);
	}

	//~~~Runtime Functions~~~

	/// <summary>
	/// Obtains the Moveable object that is in the center of the viewport using camera forward vector
	/// </summary>
	/// <returns></returns>
	private Moveable GetCenterMoveable()
	{
		var cameraTransform = transform;
		Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
		RaycastHit hit;
		//Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 100, Color.red, .1f);
		if (Physics.Raycast(ray, out hit, 100, GameManager.MoveableLayerMask))
		{
			return hit.transform.GetComponentInParent<Moveable>();
		}

		return null;
	}

	//~~~Callback Functions~~~
	public void OnMove(InputAction.CallbackContext a_context)
	{
		//Read the input value that is being sent by the Input System
		var moveInput = a_context.ReadValue<Vector2>();

		//Store the value as a Vector3, making sure to move the Y input on the Z axis.
		m_moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
	}

	public void OnHeight(InputAction.CallbackContext a_context)
	{
		//Read the input value that is being sent by the Input System
		m_verticalDirection = a_context.ReadValue<float>();
	}

	public void OnLook(InputAction.CallbackContext a_context)
	{
		//Read the input value that is being sent by the Input System
		m_lookInput = a_context.ReadValue<Vector2>() * m_lookSpeedMouse * m_mouseSensitivityMultiplier;
	}

	public void OnSprint(InputAction.CallbackContext a_context)
	{
		//Read the input value that is being sent by the Input System
		if (a_context.started)
		{
			m_sprint = true;
		}
		else if (a_context.canceled)
		{
			m_sprint = false;
		}
	}

	public void OnClick(InputAction.CallbackContext context)
	{
		if (!context.started)
		{
			return;
		}

		//Debug.Log("Click: " + context.phase);
		if (m_gameManager.HeldObject != null)
		{
			m_gameManager.ReleaseHeldMoveable();
			return;
		}

		m_gameManager.GrabMoveable(GetCenterMoveable());
	}

	public void OnRotate(InputAction.CallbackContext context)
	{
		if (context.started && m_gameManager.HeldObject)
			m_gameManager.HeldObject.RotateOnY();
	}

	public void OnScrollWheel(InputAction.CallbackContext context)
	{
		var value = context.ReadValue<Vector2>();
		//Debug.Log("Scroll: " + value);
		if (value.y > 0)
		{
			m_gameManager.MovementReference.transform.Translate(Vector3.forward, m_gameManager.CameraTransform);
		}
		else if (value.y < 0)
		{
			m_gameManager.MovementReference.transform.Translate(Vector3.back, m_gameManager.CameraTransform);
		}
	}

	public void OnToggleInstructions()
	{
		m_gameManager.InstructionsPanel.SetActive(!m_gameManager.InstructionsPanel.activeSelf);
	}

	//~~~Editor Functions~~~
#if UNITY_EDITOR

#endif

}
