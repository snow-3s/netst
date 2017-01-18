using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaytimePhase : Photon.MonoBehaviour {

    bool isInterrupted = false;
    float time = 0;
    float timeLimit = 60 * 5;

    // Use this for initialization
    void Start () {
        StartCoroutine(DaytimeEnd());
        if (PhotonNetwork.isMasterClient)
        {
            GameObject obj = PhotonNetwork.InstantiateSceneObject("Prefabs/GazeTrigger", new Vector3(0, 3f, 0), Quaternion.identity, 0, null);
            obj.GetComponent<GazeTrigger>().SetCallback(()=> { isInterrupted = true; });
        }
    }

    void Update()
    {
        time += Time.deltaTime;
    }

    IEnumerator DaytimeEnd()
    {
        yield return new WaitUntil(() => time >= timeLimit || isInterrupted);
        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().EndPhase();
        if (!isInterrupted)
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("GazeTrigger"));
        }
        PhotonNetwork.Destroy(gameObject);
    }
	
}
