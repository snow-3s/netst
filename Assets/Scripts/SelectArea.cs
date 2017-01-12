using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectArea : MonoBehaviour {
    int targetPlayerId;
    int voterPlayerId;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPlayerId(int playerId)
    {
        targetPlayerId = playerId;
        voterPlayerId = PhotonNetwork.player.ID;
    }

    public void OnGazeEnter()
    {
        //投票
        GameObject.FindGameObjectWithTag("Voter").GetComponent<PhotonView>().RPC("Vote", PhotonTargets.MasterClient, voterPlayerId, targetPlayerId);
    }

    public void OnGazeExit()
    {

    }
}
