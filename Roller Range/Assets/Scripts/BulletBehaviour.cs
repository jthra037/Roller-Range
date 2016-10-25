using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    public int dmg = 1;

    private Rigidbody rb;
    private float speed = 50;
    private float maxDistance = 50;
    private int layerMask;
    private RaycastHit hit;

	// Use this for initialization
	void Start ()
    {
        layerMask = 1 << gameObject.layer;
        rb = gameObject.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 200, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, rb.velocity.magnitude))
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log(gameObject.name + " hit " + hit.collider.name);
            Debug.Log(hitObject.tag);
            if (hitObject.CompareTag("Enemy") || hitObject.CompareTag("Player"))
            {
                Debug.Log("Calling Hit");
                hitObject.SendMessage("hit");
                Destroy(gameObject);
            }
        }
    }


	void OnTriggerEnter (Collider other)
	{
        if (!other.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
	}
}