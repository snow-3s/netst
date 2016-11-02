using UnityEngine;
using System.Collections;

public class MobileGyro : MonoBehaviour {
    Quaternion attitude;
    void Start () {
        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 0.01F;
        attitude = new Quaternion();
    }
	
	void Update () {
        if (Input.gyro.enabled)
        {
            //ジャイロを前方基準に
            attitude = Quaternion.AngleAxis(90.0f, Vector3.right) * Input.gyro.attitude * Quaternion.AngleAxis(180.0f, Vector3.forward);
        }
	}

    public Quaternion GetAttitude()
    {
        return attitude;
    }
}
