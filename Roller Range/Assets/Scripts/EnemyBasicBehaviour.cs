﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBasicBehaviour : MonoBehaviour {

    // public Collider attackVolume;
    public int health = 1;
    public int state = 0;

    [SerializeField]
    private float attackDistance = 10;
    [SerializeField]
    private float fireRate = 2;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform spawnPoint;
	[SerializeField]
	private float chaseSpeed = 10f;
	[SerializeField]
	private float wanderCircPos = 10f;
	[SerializeField]
	private float wanderCircRad = 3f;
	[SerializeField]
	private float wanderSpeed = 5f;

    private int layer;
    private float tolerance = 0.1f;
    private float lastShot;
    private float distance;
    //private Rigidbody rb;
    private NavMeshAgent agent;
    private Transform target;
    private GameController GC;

	Dictionary<int, System.Action> actions = new Dictionary<int, System.Action> ();


    // Use this for initialization
    void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform; // Locate the player
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        layer = gameObject.layer;

		actions.Add (0, chase);
		actions.Add (1, seperation);
		actions.Add (2, wander);
		actions.Add (3, nothing);
    }

    // Update is called once per frame
    void Update()
    {
		actions [state] ();
    }

    void chase()
    {
        agent.destination = target.position;
		agent.speed = chaseSpeed;
        distance = Vector3.Distance(transform.position, target.position); // Check the distance
        if (distance < attackDistance)
        {
            state = 1;
        }
    }

    void seperation()
    {
        distance = Vector3.Distance(transform.position, target.position); // Check the distance

        if (distance > attackDistance)
        {
            state = 0;
        } else if (distance - tolerance > (attackDistance / 1.5))
        {
            agent.destination = target.position;
        } else
        {
            agent.destination = transform.position;
        }

        transform.LookAt(target);
        attack();
    }

    void attack()
    {
        if (Time.time > fireRate + lastShot)
        {
            GameObject thisShot;
            thisShot = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
            thisShot.layer = layer;
            lastShot = Time.time;
        }
    }

    void hit()
    {
        health = health - 1;
        if (health <= 0)
        {
            GC.iDied(3);
            Destroy(gameObject);
        }

        state = 0;
    }

	void wander()
	{
		agent.speed = wanderSpeed;
		Vector3 offset = new Vector3 (Random.value - 0.5f, 0, Random.value - 0.5f);
		offset = offset.normalized * wanderCircRad;
		agent.destination = transform.position + (transform.forward * wanderCircPos) + offset;
	}

	void nothing()
	{
		agent.destination = transform.position;
	}

	void helpChase()
	{
		state = 0;
	}

    void OnTriggerEnter(Collider other)
    {
		if ((state != 2) && (state != 3) && other.CompareTag ("Enemy")) {
			other.SendMessage ("helpChase");
		}

		if ((state == 2) && other.CompareTag ("Player")) {
			state = 0;
		}
    }
}
