using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour 
{
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;
	public _NetCharacter target;
	
	void Update () 
	{
		switch(LevelManager.Get().state)
		{
			case LevelManager.LevelStates.Standby:
				break;
			case LevelManager.LevelStates.OnGoing:
				GameObject focus = target.gameObject;
				if(target.cc.projectileShot)
					focus = target.cc.projectileShot;
				Vector3 newPos = focus.transform.position;
				newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
				newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
				newPos.z = transform.position.z;
				transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime*3.0f);
				break;
			case LevelManager.LevelStates.Restarting:
				break;
			default:
				break;
		}
	}
}
