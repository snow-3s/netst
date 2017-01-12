using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class Voter : Photon.MonoBehaviour
{
    int maxFrame = 60 * 60 * 1;
    int frame;
    string votedRole;       //投票できる人
    string rpcCallBackName;     //投票結果を返す先
    Dictionary<int, int> votes;     //投票箱
    List<int> voters;               //投票者
    List<GameObject> selectAreas;   //選択エリア
    Participant[] participants;

    public void Run(string votedRole, string rpcCallBackName)
    {
        this.votedRole = votedRole;
        this.rpcCallBackName = rpcCallBackName;
        frame = 0;
        votes = new Dictionary<int, int>();
        participants = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().GetParticipants();

        //survivorより投票できる人を設定
        voters = new List<int>();
        foreach (Participant participant in participants)
        {
            if (participant.isSurvive())
            {
                if (!votedRole.Equals("all"))
                {
                    //動作共有及び音声共有を停止

                }

                if (participant.GetRole().Equals(votedRole) || votedRole.Equals("all"))
                {
                    voters.Add(participant.GetPlayerId());
                }
            }
        }
        //投票者のみ実行
        if (votedRole.Equals("all") || votedRole.Equals(participants.First(elem => elem.GetPlayerId() == PhotonNetwork.player.ID).GetRole()))
        {
            selectAreas = new List<GameObject>();
            GameObject prefabSelectArea = (GameObject)Resources.Load("Prefabs/SelectArea");
            //各ユーザに投票選択できるエリアを付与
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                int objOwnerId = obj.GetComponent<PhotonView>().ownerId;
                if (voters.Contains(objOwnerId) && objOwnerId != PhotonNetwork.player.ID)
                {
                    //エリア付与
                    selectAreas.Add(Instantiate(prefabSelectArea, obj.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity));
                    selectAreas.Last().GetComponent<SelectArea>().SetPlayerId(objOwnerId);
                }
            }
            //投票を促す何か

        }
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(Reception());
        }
    }

    IEnumerator Reception()
    {
        //時間経過 or 全員投票待ち
        yield return new WaitUntil(() => frame >= maxFrame || votes.Count == voters.Count);
        //投票結果から最も得票数の多い人を割り出す
        int targetPlayerId;
        if (votes.Count != 0)
            targetPlayerId = votes.GroupBy(elem => elem.Value).OrderByDescending(elem => elem.ToList().Count).First().Key;
        else
            targetPlayerId = 0;
        
        //後始末
        photonView.RPC("DestroySelectAreas", PhotonTargets.All);
        StartCoroutine(EndVoter(targetPlayerId));
    }

    IEnumerator EndVoter(int targetPlayerId)
    {
        yield return new WaitForSeconds(5);
        //投票結果を通知
        GameObject.FindGameObjectWithTag("Phase").GetComponent<PhotonView>().RPC(rpcCallBackName, PhotonTargets.All, targetPlayerId);
        PhotonNetwork.Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (frame < maxFrame)
            frame++;
    }

    //視線選択により外部から呼び出される投票メソッド
    [PunRPC]
    public void Vote(int voterPlayerId, int targetPlayerId)
    {
        votes[voterPlayerId] = targetPlayerId;
        Debug.Log("voted: " + targetPlayerId.ToString() + " - " + voterPlayerId.ToString());
    }

    //選択エリアを破棄
    [PunRPC]
    void DestroySelectAreas()
    {
        if (selectAreas != null)
        {
            foreach (GameObject obj in selectAreas)
            {
                Destroy(obj);
            }
        }
    }

}
