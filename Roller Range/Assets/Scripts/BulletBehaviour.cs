using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    public int dmg = 1;

    private Rigidbody rb;
    private RaycastHit hit;

	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 200, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, rb.velocity.magnitude))
        {
            GameObject hitObject = hit.collider.gameObject;
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