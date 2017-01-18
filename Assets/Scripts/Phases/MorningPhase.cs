using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningPhase : MonoBehaviour {

	// Use this for initialization
	void Start () {
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
        int telleePlayerId = 0;
        foreach(Participant participant in participants.Values)
        {
            if (participant.GetRole().Equals("fortuneTeller"))
            {
                telleePlayerId = participant.GetPlayerId();
                break;
            }
        }
        GameMaster gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

        playerId = 1;
		if (playerId != 0)
		{
			if (participants[PhotonNetwork.player.ID].isSurvive ()) {
			    if (PhotonNetwork.player.ID == telleePlayerId) {
                    GameObject prefabConfirmer = (GameObject)Resources.Load("Prefabs/Confirmer");
                    GameObject confirmer = Instantiate(prefabConfirmer, new Vector3(), Quaternion.identity);

                    if (participants [playerId].GetRole ().Equals ("warewolf")) {    //白黒通知
						confirmer.GetComponent<Confirmer> ().SetText ("Player" + playerId + ": " + "black");
						confirmer.GetComponent<Confirmer> ().SetCallback (() => {
							Debug.Log (participants [playerId].GetRole ());
						});
					} else {
						confirmer.GetComponent<Confirmer> ().SetText ("Player" + playerId + ": " + "white");
						confirmer.GetComponent<Confirmer> ().SetCallback (() => {
							Debug.Log (participants [playerId].GetRole ());
						});
					}
				}
			}
		}
		gameMaster.EndPhase();
		PhotonNetwork.Destroy(gameObject);
	}



}