using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MIRVEnemy : MonoBehaviour {

	// public Collider attackVolume;
	public int health = 5;
	public int state = 0;
	public GameObject child;
	public int numChildren = 4;
	[SerializeField]
	private float splitForce = 3f;

	[SerializeField]
	private float attackDistance = 15f;
	[SerializeField]
	private float fireRate = 1.5f;
	[SerializeField]
	private GameObject projectile;
	[SerializeField]
	private Transform spawnPoint;
	[SerializeField]
	private Transform spawnPoint2;
	[SerializeField]
	private float chaseSpeed = 8f;
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

	void attack()
	{
		if (Time.time > fireRate + lastShot)
		{
			StartCoroutine (shootBoth ());
		}
	}

	IEnumerator shootBoth()
	{
		GameObject thisShot;
		lastShot = Time.time;
		thisShot = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
		thisShot.layer = layer;
		yield return new WaitForSeconds (0.5f);
		thisShot = Instantiate(projectile, spawnPoint2.position, spawnPoint2.rotation) as GameObject;
		thisShot.layer = layer;
	}

	void hit()
	{
		health = health - 1;
		if (health <= 0)
		{
			for (int i = 0; i < numChildren; ++i) {
				GameObject thisChild = Instantiate (child, transform.position, transform.rotation) as GameObject;

				Rigidbody childRB = thisChild.GetComponent<Rigidbody> ();

				childRB.AddForce (new Vector3 (Random.value * splitForce, splitForce, Random.value * splitForce), ForceMode.Impulse);
			}

            GC.iDied(5);
			Destroy (gameObject);
		}
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
