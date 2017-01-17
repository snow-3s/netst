using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notifier : MonoBehaviour
{
    float time = 0;
    float timeLimit = 2.5f;

    void Awake()
    {
        Transform cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Vector3 cameraPos = cameraTransform.position;
        Vector3 cameraRotation = cameraTransform.rotation.eulerAngles;
        transform.position = cameraTransform.TransformDirection(Vector3.forward) * 0.5f + cameraPos;
        transform.Rotate(new Vector3(0f, cameraRotation.y, 0f));
    }

    void Update()
    {
        time += Time.deltaTime;
        if(time >= timeLimit)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string str)
    {
        transform.Find("Image/Text").GetComponent<Text>().text = str;
    }

}
