using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmerButton : MonoBehaviour {

    onConfirm callback = null;
    
    public delegate void onConfirm();

    public void SetCallback(onConfirm callback)
    {
        this.callback = callback;
    }

    public void OnGazeEnter()
    {
        if (callback != null)
        {
            callback();
        }
        Destroy(transform.root.root.gameObject);
    }

}
