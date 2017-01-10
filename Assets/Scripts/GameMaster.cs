using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : Photon.MonoBehaviour
{
    //参加者リスト
    Dictionary<int, Participant> participants;

    void Start()
    {
        //参加者確定、これ以上人が入れないように

        //参加者リストを取得
        PhotonPlayer[] players = PhotonNetwork.playerList;
        //参加者抽選
        participants = RoleSelecter.Run(players);
        //役職通知
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            int playerId = gameObject.GetComponent<PhotonView>().ownerId;
            gameObject.GetComponent<PlayerParameter>().SetRole(participants[playerId].GetRole());
        }
        //ゲームマスター削除,Starter 生成
        PhotonNetwork.InstantiateSceneObject("Prefabs/GameStarter", new Vector3(0, 2.5f, 0), Quaternion.identity, 0, null);
        PhotonNetwork.Destroy(gameObject);
    }

    void Update()
    {

    }
}
