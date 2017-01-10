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
        
    }

    void Update()
    {

    }
}
