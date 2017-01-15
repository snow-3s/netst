using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameMaster : Photon.MonoBehaviour
{
    public enum Phase
    {
        START, MORNING, DAYTIME, EVENING, NIGHT, HUMAN_VICTORY, WERWOLF_VICTORY, END
    };
    Phase currentPhase, nextPhase;

    //参加者リスト
    Dictionary<int, Participant> participants;
    void Start()
    {
        //参加者確定、これ以上人が入れないように

        //参加者リストを取得
        PhotonPlayer[] players = PhotonNetwork.playerList;
        participants = new Dictionary<int, Participant>();

        currentPhase = Phase.END;
        nextPhase = Phase.END;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5);
        //投票結果を通知
        if (PhotonNetwork.isMasterClient)
        {
            //役職抽選
            participants = RoleSelecter.Run(PhotonNetwork.playerList);
            //役職通知&共有
            foreach (Participant participant in participants.Values)
            {
                photonView.RPC("NotifyRole", PhotonTargets.All, participant);
            }
        }
        nextPhase = Phase.START;
    }

    void Update()
    {
        if (currentPhase != nextPhase)
        {
            currentPhase = nextPhase;
            switch (currentPhase)
            {
                case Phase.START:
                    //開始時の演出
                    PhotonNetwork.InstantiateSceneObject("Prefabs/Phases/DaytimePhase", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
                    break;

                case Phase.MORNING:
                    PhotonNetwork.InstantiateSceneObject("Prefabs/Phases/DaytimePhase", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
                    break;

                case Phase.DAYTIME:
                    PhotonNetwork.InstantiateSceneObject("Prefabs/Phases/DaytimePhase", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
                    break;

                case Phase.EVENING:
                    PhotonNetwork.InstantiateSceneObject("Prefabs/Phases/EveningPhase", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
                    break;

                case Phase.NIGHT:
                    PhotonNetwork.InstantiateSceneObject("Prefabs/Phases/NightPhase", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
                    break;

                case Phase.HUMAN_VICTORY:
                    PhotonNetwork.InstantiateSceneObject("Prefabs/Phases/DaytimePhase", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
                    break;

                case Phase.WERWOLF_VICTORY:
                    PhotonNetwork.InstantiateSceneObject("Prefabs/Phases/DaytimePhase", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
                    break;

                case Phase.END:
                    //会話グループを初期化
                    PhotonVoiceNetwork.Client.ChangeAudioGroups(new byte[] { }, new byte[] { 0 });
                    PhotonVoiceNetwork.Client.GlobalAudioGroup = 0;
                    //ゲームマスター削除,Starter 生成
                    PhotonNetwork.InstantiateSceneObject("Prefabs/GameStarter", new Vector3(0, 2.5f, 0), Quaternion.identity, 0, null);
                    PhotonNetwork.Destroy(gameObject);
                    break;
            }
        }

    }

    public Participant[] GetParticipants()
    {
        return participants.Values.ToArray();
    }

    public Dictionary<int,Participant> GetParticipantsDictionary()
    {
        return participants;
    }

    public void KillPlayer(int playerId)
    {
        participants[playerId].die();
        if (PhotonNetwork.player.ID == playerId)
        {
            //死人のみと会話できるように
            PhotonVoiceNetwork.Client.ChangeAudioGroups(null, new byte[] { 1 });
            PhotonVoiceNetwork.Client.GlobalAudioGroup = 1;
        }
    }

    //役職の通知&共有
    [PunRPC]
    void NotifyRole(Participant participant)
    {
        if(!PhotonNetwork.isMasterClient)
            participants[participant.GetPlayerId()] = participant;
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (participant.GetPlayerId() == gameObject.GetComponent<PhotonView>().ownerId)
            {
                gameObject.GetComponent<PlayerParameter>().SetRole(participant.GetRole());
            }
        }
    }

    //Phase 更新
    public void EndPhase()
    {
        switch (currentPhase)
        {
            case Phase.START:
                nextPhase = Phase.DAYTIME;
                break;

            case Phase.MORNING:
                nextPhase = Phase.DAYTIME;
                break;

            case Phase.DAYTIME:
                nextPhase = Phase.EVENING;
                break;

            case Phase.EVENING:
                nextPhase = Phase.NIGHT;
                break;

            case Phase.NIGHT:
                //勝敗判定
                if (true)
                    nextPhase = Phase.HUMAN_VICTORY;
                else
                    nextPhase = Phase.MORNING;
                break;

            case Phase.HUMAN_VICTORY:
                nextPhase = Phase.END;
                break;

            case Phase.WERWOLF_VICTORY:
                nextPhase = Phase.END;
                break;
        }
    }

}
