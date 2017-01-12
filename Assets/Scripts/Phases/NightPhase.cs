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
        PhotonNetwork.InstantiateSceneObject("Prefabs/Voter", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
        GameObject.FindGameObjectWithTag("Voter").GetComponent<Voter>().Run("werewolf", "VictimNotify");
    }

    // Update is called once per frame
    void Update()
    {
    }

    [PunRPC]
    public void VictimNotify(int playerId)
    {
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
