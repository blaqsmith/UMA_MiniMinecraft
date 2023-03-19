using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Moveable : MonoBehaviour
{
	//~~~Definitions~~~

	//~~~Variables~~~
	//Serialized
	[SerializeField]
	private List<Cubit> cubits;

	//Non-Serialized

	//~~~Accessors~~~

	//~~~Unity Functions~~~

	void Start()
	{
		
	}

	//~~~Runtime Functions~~~

	public void UpdatePosition(Vector3 a_newPosition)
	{
		var pos = a_newPosition;
		pos.x = Mathf.RoundToInt(pos.x);
		pos.y = Mathf.Max(0f, Mathf.RoundToInt(pos.y));
		pos.z = Mathf.RoundToInt(pos.z);
		transform.SetPositionAndRotation(pos, transform.rotation);
	}

	public void RotateOnY()
	{
		transform.Rotate(Vector3.up, 90);
	}

	public bool HasCollision()
	{
		foreach (var cubit in cubits)
		{
			if (cubit.HasCollision)
			{
				return true;
			}
		}

		return false;
	}

	//~~~Callback Functions~~~

	//~~~Editor Functions~~~
#if UNITY_EDITOR

	private void OnValidate()
	{
		cubits = GetComponentsInChildren<Cubit>().ToList<Cubit>();
	}

#endif

}
