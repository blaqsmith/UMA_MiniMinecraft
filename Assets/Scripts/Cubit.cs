using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cubit : MonoBehaviour
{
	//~~~Definitions~~~

	//~~~Variables~~~
	//Serialized
	[SerializeField]
	private Renderer m_renderer;
	[SerializeField]
	private Material m_defaultMaterial;
	[SerializeField]
	private Material m_warningMaterial;

	//Non-Serialized

	//~~~Accessors~~~

	public bool HasCollision { get; private set; } = false;

	//~~~Unity Functions~~~

	private void OnTriggerEnter(Collider a_other)
	{
		if ((GameManager.MoveableLayerMask & (1 << a_other.gameObject.layer))  == 0)
		{
			return;
		}

		m_renderer.material = m_warningMaterial;
		HasCollision = true;
	}

	private void OnTriggerExit(Collider a_other)
	{
		if ((GameManager.MoveableLayerMask & (1 << a_other.gameObject.layer)) == 0)
		{
			return;
		}

		m_renderer.material = m_defaultMaterial;
		HasCollision = false;
	}

	//~~~Runtime Functions~~~

	//~~~Callback Functions~~~

	//~~~Editor Functions~~~
#if UNITY_EDITOR

#endif

}
