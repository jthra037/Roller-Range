using UnityEngine;
using System.Collections;

public class PlayerLook : MonoBehaviour {

    public float rotSpeedY = 5;
	public float rotSpeedX = 10;
    public bool yInvert = false;
    public float minRot;
    public float maxRot;

    void Update()
    {
        if (yInvert)
            transform.Rotate(Mathf.Clamp((Input.GetAxis("Mouse Y") * rotSpeedY * Time.deltaTime), minRot, maxRot), 0, 0, Space.Self);
        else
            transform.Rotate(Mathf.Clamp((Input.GetAxis("Mouse Y") * -rotSpeedY * Time.deltaTime), minRot, maxRot), 0, 0, Space.Self);

        transform.Rotate(0, (Input.GetAxis("Mouse X") * rotSpeedX * Time.deltaTime), 0, Space.World);
    }
}
