using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EveningPhase : Photon.MonoBehaviour {

    int hangedPlayerId;

	// Use this for initialization
	void Start () {
        hangedPlayerId = 0;
        GameObject prefabNotifier = (GameObject)Resources.Load("Prefabs/Notifier");
        GameObject notifier = Instantiate(prefabNotifier, new Vector3(), Quaternion.identity);
        notifier.GetComponent<Notifier>().SetText("吊りターン");
        PhotonNetwork.InstantiateSceneObject("Prefabs/Voter", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
        //投票開始
        GameObject.FindGameObjectWithTag("Voter").GetComponent<Voter>().Run("all", "HangmanNotify");
    }

    // Update is called once per frame
    void Update () {
	}

    [PunRPC]
    public void HangmanNotify(int playerId)
    {
        Debug.Log("Hang: " + playerId.ToString());
        hangedPlayerId = playerId;
        GameMaster gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

        //投票が無効でなければ
        if (playerId != 0)
        {
            gameMaster.KillPlayer(hangedPlayerId);
            //退場演出
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (obj.GetComponent<PhotonView>().ownerId == hangedPlayerId)
                {
                    obj.GetComponent<Rigidbody>().useGravity = false;
                    obj.transform.position += new Vector3(0, 5.0f, 0);
                }
            }
        }
        gameMaster.EndPhase();
        PhotonNetwork.Destroy(gameObject);
    }
}
