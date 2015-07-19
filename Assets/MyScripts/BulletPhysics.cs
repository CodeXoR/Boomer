using UnityEngine;
using System.Collections;

public class BulletPhysics : MonoBehaviour 
{
	public Rigidbody2D body;

	void Start() {
		body = GetComponent<Rigidbody2D>();
	}
	void Update() {
		switch(LevelManager.Get().state)
		{
			case LevelManager.LevelStates.Standby:
				break;
			case LevelManager.LevelStates.OnGoing:
				Vector2 v = body.velocity;
				float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				break;
			case LevelManager.LevelStates.Restarting:
				break;
			default:
				break;
		}
	}
}
