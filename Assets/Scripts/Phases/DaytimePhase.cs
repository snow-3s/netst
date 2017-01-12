using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaytimePhase : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DaytimeEnd());
	}

    IEnumerator DaytimeEnd()
    {
        yield return new WaitForSeconds(5);
        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().EndPhase();
        PhotonNetwork.Destroy(gameObject);
    }
	
}
