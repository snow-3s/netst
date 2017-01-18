using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningPhase : MonoBehaviour {

	// Use this for initialization
	void Start () {
		telleePlayerId = 0;
		PhotonNetwork.InstantiateSceneObject("Prefabs/Voter", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
		GameObject.FindGameObjectWithTag("Voter").GetComponent<Voter>().Run("fortuneTeller", "TelleeRoleNotify");
	}

	// Update is called once per frame
	void Update () {
	}

	[PunRPC]
	public void TelleeRoleNotify(int playerId)
	{
		Dictionary<int, Participant> participants = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().GetParticipantsDictionary();
		GameMaster gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
		GameObject prefabConfirmer = (GameObject)Resources.Load("Prefabs/Confirmer");
		GameObject confirmer = Instantiate(prefabConfirmer, new Vector3(), Quaternion.identity);

		if (playerId != 0)
		{
			if (GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameMaster> ().GetParticipantsDictionary () [PhotonNetwork.player.ID].isSurvive ()) {
				foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player")) {
					if (PhotonNetwork.player.GetRole () == "fortuneTeller") {
						if (Participants [playerId].GetRole ().Equals ("warewolf")) {    //白黒通知
							confirmer.GetComponent<Confirmer> ().SetText ("Player" + playerId + ": " + "black");
							confirmer.GetComponent<Confirmer> ().SetCallback (() => {
								Debug.Log (Participants [playerId].GetRole ());
							});
						} else {
							confirmer.GetComponent<Confirmer> ().SetText ("Player" + playerId + ": " + "white");
							confirmer.GetComponent<Confirmer> ().SetCallback (() => {
								Debug.Log (Participants [playerId].GetRole ());
							});
						}
					}
				}
			}
		}
		gameMaster.EndPhase();
		PhotonNetwork.Destroy(gameObject);
	}



}