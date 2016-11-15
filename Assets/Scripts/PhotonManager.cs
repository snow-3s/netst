using UnityEngine;
using System.Collections;

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
        GameObject prefabCamera = (GameObject)Resources.Load("Prefabs/vrcamera");
        GameObject player = PhotonNetwork.Instantiate("Prefabs/Avatars/unitychan", new Vector3(Random.Range(-4.0f,4.0f), 0.1f, Random.Range(-4.5f, 4.5f)), Quaternion.identity, 0);
        GameObject head = player.transform.FindChild("head").gameObject;
        GameObject camera = (GameObject)Instantiate(prefabCamera, head.transform.position, Quaternion.identity);
        camera.transform.parent = head.transform;
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.Log("PhotonManager OnDisconnectedFromPhoton");
    }
}
