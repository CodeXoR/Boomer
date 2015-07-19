using UnityEngine;
using System.Collections;

public class gizmo : MonoBehaviour 
{
	void OnDrawGizmos()
	{
		Gizmos.DrawSphere (transform.position, .5f);
		//Gizmos.DrawSphere (transform.position + transform.right * 3.0f, .5f);
	}
}
