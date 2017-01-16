﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightPhase : Photon.MonoBehaviour
{

    int victimPlayerId;

    // Use this for initialization
    void Start()
    {
        victimPlayerId = 0;
        PhotonNetwork.InstantiateSceneObject("Prefabs/Voter", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
        GameObject.FindGameObjectWithTag("Voter").GetComponent<Voter>().Run("werewolf", "VictimNotify");
        Dictionary<int, Participant> participants = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().GetParticipantsDictionary();
        if (participants[PhotonNetwork.player.ID].isSurvive())
        {
            if (participants[PhotonNetwork.player.ID].GetRole().Equals("werewolf"))
            {
                //狼会話
                PhotonVoiceNetwork.Client.GlobalAudioGroup = 2;
            }
            else
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (PhotonNetwork.player.ID == obj.GetComponent<PhotonView>().ownerId)
                    {
                        //音声を送信しない
                        obj.GetComponent<PhotonVoiceRecorder>().Transmit = false;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    [PunRPC]
    public void VictimNotify(int playerId)
    {
        if (GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().GetParticipantsDictionary()[PhotonNetwork.player.ID].isSurvive()) {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (PhotonNetwork.player.ID == obj.GetComponent<PhotonView>().ownerId)
                {
                    //音声を再生できるように
                    obj.GetComponent<PhotonVoiceRecorder>().Transmit = true;
                }
            }
            //全体の会話グループに
            PhotonVoiceNetwork.Client.GlobalAudioGroup = 0;
        }

        victimPlayerId = playerId;
        GameMaster gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        if (playerId != 0)
        {
            gameMaster.KillPlayer(victimPlayerId);
            //演出
        }
        gameMaster.EndPhase();
        PhotonNetwork.Destroy(gameObject);
    }
}