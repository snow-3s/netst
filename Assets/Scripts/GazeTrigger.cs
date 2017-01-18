using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeTrigger : Photon.MonoBehaviour {

    //何人注視しているか
    int gazedCount;
    int majority;

    onGazeGather callback = null;

    public delegate void onGazeGather();

    void Start()
    {
        SetGazedAt(false);
        Participant[] participants = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().GetParticipants();
        int maxGazedCount = 0;
        foreach(Participant participant in participants)
        {
            if (participant.isSurvive())
            {
                maxGazedCount++;
            }
        }
        //過半数
        majority = maxGazedCount / 2;
        gazedCount = 0;
    }

    void Update()
    {
        if (gazedCount != 0 && gazedCount > majority)
        {
            //Callback呼び出し、自身を削除
            callback();
            PhotonNetwork.Destroy(gameObject);
        }
        if (gazedCount < 0)
            gazedCount = 0;
    }

    public void SetCallback(onGazeGather callback)
    {
        this.callback = callback;
    }

    public void SetGazedAt(bool gazedAt)
    {
        GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
    }

    public void OnGazeEnter()
    {
        SetGazedAt(true);
        photonView.RPC("PlusGazed", PhotonTargets.MasterClient);
    }

    public void OnGazeExit()
    {
        SetGazedAt(false);
        photonView.RPC("MinusGazed", PhotonTargets.MasterClient);
    }

    [PunRPC]
    void PlusGazed()
    {
        gazedCount++;
    }

    [PunRPC]
    void MinusGazed()
    {
        gazedCount--;
    }
}
