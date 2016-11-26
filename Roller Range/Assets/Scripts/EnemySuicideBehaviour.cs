using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySuicideBehaviour : MonoBehaviour {

    public Collider attackVolume;
    public int health = 2;
    public int state = 1;

    /*[SerializeField]
    private float attackDistance = 5;
    [SerializeField]
    private float attackForce = 100;*/
	[SerializeField]
	private float chaseSpeed = 16f;
	[SerializeField]
	private float wanderCircPos = 10f;
	[SerializeField]
	private float wanderCircRad = 3f;
	[SerializeField]
	private float wanderSpeed = 5f;


    //private int layer;
    private float distance;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private Transform target;
    private GameController GC;

    private Vector3 myOffset;
    private Vector3 myDestination;

	Dictionary<int, System.Action> actions = new Dictionary<int, System.Action> ();

	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform; // Locate the player
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        //layer = gameObject.layer;

		actions.Add (0, chase);
		actions.Add (2, wander);
		actions.Add (3, nothing);
	}

    // Update is called once per frame
    void Update()
    {
		if ((transform.position - target.position).magnitude < 3.0f) {
			gotEem ();
		}

		actions [state] ();
    }

    void chase()
    {
		agent.speed = chaseSpeed;
        agent.destination = target.position;
        distance = Vector3.Distance(transform.position, target.position); // Check the distance
    }

	void wander()
	{
		agent.speed = wanderSpeed;
		Vector3 offset = new Vector3 (Random.value - 0.5f, 0, Random.value - 0.5f);
		offset = offset.normalized * wanderCircRad;
        myOffset = offset;/// FOR DEBUGGING
        agent.destination = transform.position + (transform.forward * wanderCircPos) + offset;
        myDestination = agent.destination; ///ALSO FOR DEBUGGING
	}

	void nothing()
	{
		agent.destination = transform.position;
	}

	void helpChase()
	{
		state = 0;
	}

    void hit()
    {
        health = health - 1;
        if (health <= 0)
        {
            GC.iDied(1);
            Destroy(gameObject);
        }
    }

	void gotEem()
	{
		target.gameObject.SendMessage ("hit");
		DestroyImmediate(gameObject);
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
