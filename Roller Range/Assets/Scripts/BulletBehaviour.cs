using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    private Rigidbody rb;
    private float force = 100;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(force * transform.forward, ForceMode.Impulse);
	}

	void OnTriggerEnter (Collider other)
	{
        if (!other.CompareTag("Boundary"))
        {
            Debug.Log(gameObject.name + "has had a collision with " + other);
            Destroy(gameObject);
        }
	}
}