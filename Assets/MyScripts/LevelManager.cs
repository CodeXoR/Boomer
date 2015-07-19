using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour 
{
	public enum LevelStates { Standby = 0, OnGoing = 1, Restarting = 2 }
	public LevelStates state = LevelStates.OnGoing;
	static LevelManager levelManager;
	public static LevelManager Get() { return levelManager; }

	void Awake()
	{
		levelManager = this;
	}

	void Update()
	{
		switch(state)
		{
			case LevelStates.Standby:
				break;
			case LevelStates.OnGoing:
				break;
			case LevelStates.Restarting:
				Application.LoadLevel (Application.loadedLevelName);
				break;
			default:
				break;
		}
	}

	public void Restart(){
		state = LevelStates.Restarting;
	}
}
