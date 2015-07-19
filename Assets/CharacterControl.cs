using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterControl : Photon.MonoBehaviour 
{
	const float MAX_SPEED = 20.0f;
	
	#region PUBLIC VARIABLES
	
	// MoveStates Enum
	public enum MoveStates { Idle = 0, MoveRight = 1, MoveLeft = 2, ShootReady = 3 }
	public MoveStates state = MoveStates.Idle;
	
	public float speed;
	public GameObject projectileShot = null;
	
	#endregion
	
	#region PRIVATE VARIABLES
	
	// Character Gun Reference
	GameObject gun;
	
	// Rigidbody Affected By Physics
	Rigidbody2D body;
	
	// Character Sprite Rotation Variable
	Vector3 orientation;
	
	// Animation Components
	Animator anim;
	
	// InputSystem
	InputSystem iSystem;
	
	// Children
	List<Transform> children = new List<Transform>();
	
	#endregion
	
	#region PRIVATE FUNCTIONS
	
	void Start()
	{
		// reference to children
		foreach (Transform child in transform) {
			children.Add(child);
		}
		
		//Transform UICanvas = GameObject.Find ("UICanvas").transform;
		// Each character has children at start
		// 1st child - input system
		// 2nd child - gun or weapon
		// 3rd child - gun or weapon shoot guide
		// 4th child - GUI Canvas
		// 5th child - Camera
		
		iSystem = children [0].GetComponent<InputSystem> ();
		
		// reparent all other children
		children [1].SetParent(null);
		children [2].SetParent(null);
		children [3].SetParent(null);

		children [4].GetComponent<FollowCam> ().target = GetComponent<_NetCharacter> ();
		children [4].SetParent(null);
		
		gun = children [1].gameObject;
		
		// rigidbody component
		body = GetComponent<Rigidbody2D> ();
		// animator component
		anim = GetComponent<Animator> ();
		
		// rotation variable
		orientation = transform.eulerAngles;
		
		// enable or disable components
		iSystem.enabled = true;
		iSystem.shootGuide.gameObject.SetActive(true);
		children [3].gameObject.SetActive(true);
		children [4].gameObject.SetActive(true);
		children [4].GetComponent<FollowCam> ().enabled = true;
		
		// clear list
		children.Clear ();
	}
	
	void Update()
	{
		switch (LevelManager.Get ().state) {
		case LevelManager.LevelStates.Standby:
			break;
		case LevelManager.LevelStates.OnGoing:
			ApplyMove ();
			break;
		case LevelManager.LevelStates.Restarting:
			break;
		default:
			break;
		}
	}
	
	void ApplyMove()
	{
		switch(state)
		{
		case MoveStates.Idle:
			gun.GetComponent<SpriteRenderer>().enabled = true;
			body.velocity = new Vector2(0, body.velocity.y);
			iSystem.activeCursor.position = RectTransformUtility.WorldToScreenPoint(Camera.main, 
			                                                                        iSystem.shootGuide.position);
			ActDeactShootUI(false);
			iSystem.moveUI.SetActive(true);
			break;
		case MoveStates.ShootReady:
			iSystem.activeCursor.position = RectTransformUtility.WorldToScreenPoint(Camera.main, 
			                                                                        iSystem.shootGuide.position);
			ActDeactShootUI(true);
			iSystem.moveUI.SetActive(false);
			break;
		case MoveStates.MoveRight:
			gun.GetComponent<SpriteRenderer>().enabled = false;
			iSystem.originalRot = gun.transform.right;
			ActDeactShootUI(false);
			if(transform.eulerAngles.y >= 180f){
				SetOrientation(0f);
			}
			body.AddForce((Vector2)transform.right * speed, ForceMode2D.Force);
			break;
		case MoveStates.MoveLeft:
			gun.GetComponent<SpriteRenderer>().enabled = false;
			iSystem.originalRot = gun.transform.right;
			ActDeactShootUI(false);
			if(transform.eulerAngles.y < 180f){
				SetOrientation(180f);
			}
			body.AddForce((Vector2)transform.right * speed, ForceMode2D.Force);
			break;
		default:
			break;
		}
		Vector2 vel = body.velocity;
		vel.x = Mathf.Clamp(vel.x, -MAX_SPEED, MAX_SPEED);
		body.velocity = vel;
		anim.SetFloat("Velocity", Mathf.Abs(body.velocity.x));
	}
	
	void ActDeactShootUI(bool flag)
	{
		iSystem.shootGuide.gameObject.SetActive(flag);
		iSystem.activeCursor.gameObject.SetActive(flag);
	}
	
	void SetOrientation(float angle)
	{
		// character orientation
		orientation = transform.eulerAngles;
		orientation.y = angle;
		orientation.z = -orientation.z;
		transform.eulerAngles = orientation;
		
		// gun orientation
		gun.transform.eulerAngles = transform.eulerAngles;
	}
	#endregion 
	
	#region PUBLIC FUNCTIONS
	
	public void ShowShootUI(){
		state = state == MoveStates.ShootReady ? MoveStates.Idle : MoveStates.ShootReady;
	}
	
	public void MoveRight(){
		state = MoveStates.MoveRight;
	}
	
	public void MoveLeft(){
		state = MoveStates.MoveLeft;
	}
	
	public void Stop(){
		state = MoveStates.Idle;
	}
	
	#endregion
}
