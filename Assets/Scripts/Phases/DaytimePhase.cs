using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaytimePhase : Photon.MonoBehaviour {

    bool isInterrupted = false;
    int frame = 0;
    int maxFrame = 60 * 60 * 5;

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
        frame++;
    }

    IEnumerator DaytimeEnd()
    {
        yield return new WaitUntil(() => frame >= maxFrame || isInterrupted);
        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().EndPhase();
        if (!isInterrupted)
        {
            PhotonNetwork.Destroy(GameObject.FindGameObjectWithTag("GazeTrigger"));
        }
        PhotonNetwork.Destroy(gameObject);
    }
	
}
