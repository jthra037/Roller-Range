using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

    private Transform playerCam;

	// Use this for initialization
	void Start () {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerCam = player.GetComponentInChildren<Camera>().GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(playerCam.position);
	}
}
