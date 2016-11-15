using UnityEngine;
using System.Collections;

public class AvatarRotationUpdater : Photon.MonoBehaviour {

    GameObject camera;

	// Use this for initialization
	void Start () {
        if (photonView.isMine)
        {
            camera = GameObject.FindWithTag("MainCamera");
        }
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (photonView.isMine)
        {
            this.transform.rotation = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
        }
	}

}
