﻿using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    public float speed = 10;
    public Collider attackVolume;
    public float attackDistance = 5;
    public float attackForce = 100;

    private Rigidbody rb;
    private Transform target;
    private float distance;

	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform; // Locate the player
	}

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target); // Face the player
        distance = Vector3.Distance(transform.position, target.position); // Check the distance
    }

    void FixedUpdate()
    {
        if (distance <= attackDistance)
        {
            attack();
        } else
        {
            rb.velocity = transform.forward * speed;
        }
    }

    void attack()
    {
        rb.AddForce(transform.forward * attackForce, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Got 'eem!");
            Destroy(gameObject);
        }
    }
}
