using UnityEngine;
using System.Collections;
using System;

public class PhotonManager : Photon.MonoBehaviour
{
	
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("v0.1");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnJoinedLobby()
    {
        Debug.Log("PhotonManager OnJoinedLobby");
        //join room
        RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom("room1", roomOptions, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        Debug.Log("PhotonManager OnJoinedRoom");
        //player and vrcamera generate
//		int count = GameObject.FindGameObjectsWithTag("Player").Length;
		int count = PhotonNetwork.playerList.Length;
		int angle = 30;
        GameObject prefabCamera = (GameObject)Resources.Load("Prefabs/vrcamera");
        GameObject camera = (GameObject)Instantiate(prefabCamera, new Vector3(), Quaternion.identity);
		GameObject player = PhotonNetwork.Instantiate("Prefabs/Avatars/unitychan", new Vector3(3 * Mathf.Sin(angle * count * (Mathf.PI / 180)), 0.1f, 3 * Mathf.Cos(angle * count * (Mathf.PI / 180))), Quaternion.identity, 0);
        GameObject head = player.transform.FindChild("head").gameObject;
        camera.GetComponent<PositionSynchronizer>().SetSyncTarget(head);
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.Log("PhotonManager OnDisconnectedFromPhoton");
    }
}
