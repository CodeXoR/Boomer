using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _NetCharacter : Photon.MonoBehaviour 
{
	public CharacterControl cc;

	public bool isAlive;

	void Awake()
	{
		isAlive = true;

		if (photonView.isMine) {
			cc.enabled = true;
			transform.name = "ME";
		}

		else{
			transform.name = "Other";
		}
	}
}
