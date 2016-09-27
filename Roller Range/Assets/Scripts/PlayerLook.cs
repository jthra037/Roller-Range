using UnityEngine;
using System.Collections;

public class PlayerLook : MonoBehaviour {

    public float rotSpeed = 5;
    public bool yInvert = false;
    public float minRot;
    public float maxRot;

    void Update()
    {
        if (yInvert)
            transform.Rotate(Mathf.Clamp((Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime), minRot, maxRot), 0, 0, Space.Self);
        else
            transform.Rotate(Mathf.Clamp((Input.GetAxis("Mouse Y") * -rotSpeed * Time.deltaTime), minRot, maxRot), 0, 0, Space.Self);

        transform.Rotate(0, (Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime), 0, Space.World);
    }
}
