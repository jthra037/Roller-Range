using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    Rigidbody rb;
    float force = 100;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(force * transform.forward, ForceMode.Impulse);
	}

	void OnTriggerEnter (Collider other)
	{
        Debug.Log(gameObject.name + "has had a collision with " + other);
	    Destroy (gameObject);
	}
}