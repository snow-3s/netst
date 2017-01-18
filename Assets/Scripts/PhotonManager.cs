using UnityEngine;
using System.Collections;
using System;

public class PhotonManager : Photon.MonoBehaviour
{
	
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("v0.1");
        Application.targetFrameRate = 60;
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
		int count = PhotonNetwork.playerList.Length;
		int angle = 40;
        GameObject prefabCamera = (GameObject)Resources.Load("Prefabs/vrcamera");
        GameObject camera = (GameObject)Instantiate(prefabCamera, new Vector3(), Quaternion.identity);
        string[] avatarArray = { "man", "old_man", "old_man2", "old_woman" };
        string playerAvatarName = avatarArray[UnityEngine.Random.Range(0, avatarArray.Length)];
		GameObject player = PhotonNetwork.Instantiate("Prefabs/Avatars/"+playerAvatarName, new Vector3(3.5f * Mathf.Sin(angle * count * (Mathf.PI / 180)), 0.1f, 3.5f * Mathf.Cos(angle * count * (Mathf.PI / 180))), Quaternion.identity, 0);
        GameObject head = player.transform.FindChild("head").gameObject;
        camera.GetComponent<PositionSynchronizer>().SetSyncTarget(head);
        //GameStarter generate as SceneObject
        PhotonNetwork.InstantiateSceneObject("Prefabs/GameStarter", new Vector3(0, 2.5f, 0), Quaternion.identity, 0, null);
        ParticipantSerializer.Register();
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.Log("PhotonManager OnDisconnectedFromPhoton");
    }
}
