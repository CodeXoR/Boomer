using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray(transform.position, transform.up);
		RaycastHit hit;
		if (Physics.SphereCast (ray, 4.0f, out hit, 100f)) {
			Debug.Log("hit" + hit.transform.name);
		}
	}
}
