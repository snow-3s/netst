using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectArea : MonoBehaviour {
    float gazedTime = 0;
    bool isGazed = false;
    int targetPlayerId;
    int voterPlayerId;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isGazed)
        {
            gazedTime += Time.deltaTime;
            if(gazedTime > 4f)
            {
                //投票
                GameObject.FindGameObjectWithTag("Voter").GetComponent<PhotonView>().RPC("Vote", PhotonTargets.MasterClient, voterPlayerId, targetPlayerId);
                GameObject prefabConfirmer = (GameObject)Resources.Load("Prefabs/Confirmer");
                GameObject confirmer = Instantiate(prefabConfirmer, new Vector3(), Quaternion.identity);
                confirmer.GetComponent<Confirmer>().SetText("投票しました");
                isGazed = false;
                gazedTime = 0;
            }
        }
		
	}

    public void SetPlayerId(int playerId)
    {
        targetPlayerId = playerId;
        voterPlayerId = PhotonNetwork.player.ID;
    }

    public void OnGazeEnter()
    {
        isGazed = true;
    }

    public void OnGazeExit()
    {
        isGazed = false;
        gazedTime = 0;
    }
}
