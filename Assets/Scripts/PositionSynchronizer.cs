using UnityEngine;
using System.Collections;

public class PositionSynchronizer : MonoBehaviour {

    GameObject target = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (target != null)
        {
            this.transform.position = target.transform.position;
        }
	}

    public void SetSyncTarget(GameObject target)
    {
        this.target = target;
    }
}
