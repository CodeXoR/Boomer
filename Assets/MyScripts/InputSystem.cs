using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputSystem : MonoBehaviour 
{
	const float referenceScreenWidth = 800;
	const float referenceScreenHeight = 400;
#region ENUMS

	// InputStates Enum
	public enum InputStates { Standby = 0, Moving = 1, Shoot = 2 }
	public InputStates inputState;

#endregion

#region PUBLIC VARIABLES 

	// Character Gun Projectile Reference
	public GameObject projectile;

	/** GUI Transform References **/
	// Shooting Cursor For Touch
	public Transform shootCursorL;
	public Transform shootCursorR;
	public Transform activeCursor;

	public Transform shootGuide;
	
	public Transform gun;

	/** Shooting Variables **/
	// Shoot Power
	public float shootPower;

	// screen aspect ratio
	public float aspectRatio;

	public Vector3 originalRot;

	public GameObject moveUI;

#endregion

#region PRIVATE VARIABLES 

	// InputState
	InputStates state;



#endregion

#region PRIVATE FUNCTIONS

	void Start()
	{
		activeCursor = transform.parent.eulerAngles.y <= 180 ? shootCursorR : shootCursorL;
		aspectRatio = (referenceScreenWidth / Screen.width) + (referenceScreenHeight/Screen.height);
		activeCursor.position = shootGuide.position;
		state = InputStates.Standby;
		originalRot = gun.right;
	}
	
	void Update ()
	{
		TouchPlayer ();
		gun.position = transform.parent.position + transform.parent.right*3.65f + transform.parent.up*3.85f;
		shootGuide.position = gun.position + (gun.right*0.5f);
		shootGuide.position = new Vector3 (shootGuide.position.x, shootGuide.position.y, 0);
		shootGuide.up = gun.right;
		activeCursor = transform.parent.eulerAngles.y <= 180 ? 
					   shootCursorR : shootCursorL;
		switch(LevelManager.Get().state)
		{
			case LevelManager.LevelStates.Standby:
				break;
			case LevelManager.LevelStates.OnGoing:
				ApplyTouchInput();
				break;
			case LevelManager.LevelStates.Restarting:
				break;
			default:
				break;
		}
	}

	void ApplyTouchInput()
	{
		switch(state)
		{
			case InputStates.Standby:
				GunAlignStart();
				break;
			case InputStates.Moving:
				if (Input.touchCount == 1) {
					Vector3 touchPos = Input.GetTouch(0).position;
					Vector3 offset = touchPos - activeCursor.position;
					activeCursor.position = Camera.main.WorldToScreenPoint(gun.position) +
					Vector3.ClampMagnitude(offset, 185f/aspectRatio);
					
					float d = Vector3.Distance (Camera.main.ScreenToWorldPoint (activeCursor.position),
				                            	shootGuide.position);
					GunAlignToCursor();
					ArrowStretch(d);
					shootPower = d * 3f;
				}
				else{
					state = InputStates.Standby;
					transform.parent.GetComponent<CharacterControl>().state = CharacterControl.MoveStates.Idle;
				}
				break;
			case InputStates.Shoot:
				ShootProjectile();
				shootPower = 0.1f;
			    shootGuide.localScale = new Vector3(1.5f,1.5f,1f);     
				transform.parent.GetComponent<CharacterControl>().state = CharacterControl.MoveStates.Idle;
				state = InputStates.Standby;
				break;
			default:
				break;
		}
	}

	void ShootProjectile()
	{
		GameObject bullet = (GameObject)Instantiate (projectile, gun.position + gun.right * 3.0f, gun.rotation);
		transform.parent.GetComponent<CharacterControl> ().projectileShot = bullet;
		bullet.GetComponent<Rigidbody2D> ().AddForce (gun.right * shootPower, ForceMode2D.Impulse);
	}

	void ArrowStretch(float dist)
	{
		Vector3 stretchScale = new Vector3 (shootGuide.localScale.x, 
		                                    Mathf.Clamp(dist/shootGuide.localScale.y, 1.5f, 3.5f), 
		                                    shootGuide.localScale.z);

		shootGuide.localScale = Vector3.Lerp(shootGuide.localScale,stretchScale, Time.deltaTime*8.0f);
	}
	
	void TouchPlayer()
	{
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 1000.0f);
			if(hits.Length > 0){
				if(hits[0].transform.gameObject == transform.parent.gameObject ){
					shootGuide.localScale = new Vector3(1.5f,1.5f,1f);   
					transform.parent.GetComponent<CharacterControl>().ShowShootUI();
				}
			}
		}
	}
	
#endregion

#region PUBLIC FUNCTIONS
	public void MoveToTouch(){
		state = InputStates.Moving;
	}

	public void Shoot(){
		state = InputStates.Shoot;
	}

	public void GunAlignStart(){
		gun.right = Vector3.Lerp (gun.right, originalRot, Time.deltaTime*8.0f);
	}

	public void GunAlignToCursor()
	{
		Vector3 guideCoords = Camera.main.ScreenToWorldPoint (activeCursor.position);
		guideCoords = new Vector3 (guideCoords.x, guideCoords.y, 0);
		Vector3 target = gun.position - guideCoords;
		gun.right = Vector3.Lerp (gun.right, target, Time.deltaTime*3.0f);
	}

#endregion

#region UNUSED CODE FOR REFERENCE
	
	//	public Transform shootBand1;
	//	public Transform shootBand2;

	//shootBand1.position = RectTransformUtility.WorldToScreenPoint (Camera.main, 
	//                                                               gun.position + gun.right*9.85f + gun.up*12.95f);
	//shootBand2.position = RectTransformUtility.WorldToScreenPoint (Camera.main, 
	//                                                               gun.position + gun.right*10.15f - gun.up*12.95f);

	//	void BandAlignAndStretch()
	//	{
	//		shootBand1.up = (activeCursor.position)-shootBand1.position;
	//		shootBand2.up = (activeCursor.position)-shootBand2.position;
	//		RectTransform rt1 = shootBand1.GetComponent<RectTransform> ();
	//		rt1.sizeDelta = new Vector2 (100, 
	//		                             aspectRatio*Vector3.Distance (activeCursor.position, shootBand1.position));
	//		RectTransform rt2 = shootBand2.GetComponent<RectTransform> ();
	//		rt2.sizeDelta = new Vector2 (100, 
	//		                             aspectRatio*Vector3.Distance (activeCursor.position, shootBand2.position));
	//	}
#endregion

}
