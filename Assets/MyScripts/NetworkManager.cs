using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	public Text connectionState;
	public Transform spawnPoint;
	
	void Start () {
		PhotonNetwork.ConnectUsingSettings("v0.1");
		//PhotonNetwork.logLevel = PhotonLogLevel.Full;
	}

	void OnJoinedLobby()
	{
		RoomOptions roomOptions = new RoomOptions () { isVisible = false, maxPlayers = 4 };
		PhotonNetwork.JoinOrCreateRoom("velvetRoom", roomOptions, TypedLobby.Default);
	}

//	void OnPhotonRandomJoinFailed()
//	{
//		Debug.Log("Can't join random room!");
//		PhotonNetwork.CreateRoom(null);
//		Debug.Log ("room created!");
//	}

	void OnJoinedRoom()
	{
		connectionState.gameObject.SetActive (false);
		GameObject character = PhotonNetwork.Instantiate ("PandaRobo", spawnPoint.position, spawnPoint.rotation, 0);
	}

	void OnGUI(){
		connectionState.text = PhotonNetwork.connectionStateDetailed.ToString();
	}
}
