using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightPhase : Photon.MonoBehaviour
{

    int victimPlayerId;

    // Use this for initialization
    void Start()
    {
        victimPlayerId = 0;
        GameObject prefabNotifier = (GameObject)Resources.Load("Prefabs/Notifier");
        GameObject notifier = Instantiate(prefabNotifier, new Vector3(), Quaternion.identity);
        notifier.GetComponent<Notifier>().SetText("人狼ターン");
        PhotonNetwork.InstantiateSceneObject("Prefabs/Voter", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
        StartCoroutine(DelayExec());
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator DelayExec() {
        yield return new WaitForSeconds(3);
        GameObject.FindGameObjectWithTag("Voter").GetComponent<Voter>().Run("werewolf", "VictimNotify");
        Dictionary<int, Participant> participants = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().GetParticipantsDictionary();
        if (participants[PhotonNetwork.player.ID].isSurvive())
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (PhotonNetwork.player.ID == obj.GetComponent<PhotonView>().ownerId)
                {
                    if (participants[PhotonNetwork.player.ID].GetRole().Equals("werewolf"))
                    {
                        //狼会話
                        PhotonVoiceNetwork.Client.GlobalAudioGroup = 2;
                    }
                    else
                    {
                        //音声を送信しない
                        obj.GetComponent<PhotonVoiceRecorder>().Transmit = false;
                    }
                    //アバターの動きを共有しないように
                    obj.GetComponent<PhotonView>().synchronization = ViewSynchronization.Off;
                }
            }
        }
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
                    //アバターの動きを共有するように
                    obj.GetComponent<PhotonView>().synchronization = ViewSynchronization.UnreliableOnChange;
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
