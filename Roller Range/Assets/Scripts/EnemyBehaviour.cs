using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    public float speed;
    public Collider attackVolume;

    private Rigidbody rb;
    private Transform target;

	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
	}

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
