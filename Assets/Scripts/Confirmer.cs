using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Confirmer : MonoBehaviour {

    void Awake()
    {
        Transform cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Vector3 cameraPos = cameraTransform.position;
        Vector3 cameraRotation = cameraTransform.rotation.eulerAngles;
        transform.position = cameraTransform.TransformDirection(Vector3.forward)*0.9f + cameraPos;
        transform.Rotate(new Vector3(0f, cameraRotation.y, 0f));
    }

    public void SetText(string str)
    {
        transform.Find("Image/Text").GetComponent<Text>().text = str;
    }

    public void SetCallback(ConfirmerButton.onConfirm callback)
    {
        transform.Find("Image/Button").GetComponent<ConfirmerButton>().SetCallback(callback);
    }

}
