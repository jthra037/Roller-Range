using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    public int dmg = 1;

    private Rigidbody rb;
    private float speed = 50;
    private float maxDistance = 50;
    private int layer;
    private RaycastHit hit;

	// Use this for initialization
	void Start ()
    {
        layer = gameObject.layer;
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 200, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.Log(gameObject.name + " hit " + hit.collider.name);
            Destroy(gameObject);
        }
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