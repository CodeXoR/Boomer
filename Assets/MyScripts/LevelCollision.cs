using UnityEngine;
using System.Collections;

public class LevelCollision : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
		{
			Destroy(other.gameObject);
		}
	}
}
