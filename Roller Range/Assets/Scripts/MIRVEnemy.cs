using UnityEngine;
using System.Collections;

public class MIRVEnemy : MonoBehaviour {

	// public Collider attackVolume;
	public int health = 5;
	public int state = 0;

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
	private int layer;
	private float tolerance = 0.1f;
	private float lastShot;
	private float distance;
	//private Rigidbody rb;
	private NavMeshAgent agent;
	private Transform target;


	// Use this for initialization
	void Start()
	{
		//rb = gameObject.GetComponent<Rigidbody>();
		agent = gameObject.GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Player").transform; // Locate the player
		layer = gameObject.layer;
	}

	// Update is called once per frame
	void Update()
	{
		switch (state)
		{
		case 0:
			chase();
			break;
		case 1:
			seperation();
			break;
		default:
			agent.destination = transform.position;
			Debug.Log("Wander isn't implemented yet!");
			break;
		}
	}

	void chase()
	{
		agent.destination = target.position;
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
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
		if (other.CompareTag("Bullet"))
		{
			Debug.Log("We been hit!");
			health = health - other.gameObject.GetComponent<BulletBehaviour>().dmg;
			if (health <= 0)
			{
				Destroy(gameObject);
			}
		}
	}

}
