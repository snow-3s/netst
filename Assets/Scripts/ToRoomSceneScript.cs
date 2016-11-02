using UnityEngine;
using System.Collections;

public class ToRoomSceneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void ButtonOnClick () {
		Debug.Log("hello world");
		Application.LoadLevel("room");
	}

}
