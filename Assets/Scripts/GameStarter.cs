using UnityEngine;
using System.Collections;

public class GameStarter : Photon.MonoBehaviour {

    //何人注視しているか
    int gazedCount;

	// Use this for initialization
	void Start () {
		SetGazedAt(false);
        gazedCount = 0;
	}

	// Update is called once per frame
	void Update () {
        if(gazedCount != 0 && gazedCount == PhotonNetwork.playerList.Length)
        {
            //ゲームの進行役を生成、自身を削除
            PhotonNetwork.InstantiateSceneObject("Prefabs/GameMaster", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
            PhotonNetwork.Destroy(gameObject);
        }
        if (gazedCount < 0)
            gazedCount = 0;
    }

	public void SetGazedAt(bool gazedAt) {
		GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
	}

	public void OnGazeEnter() {
        SetGazedAt(true);
        photonView.RPC("PlusGazed", PhotonTargets.MasterClient);
	}

	public void OnGazeExit() {
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
