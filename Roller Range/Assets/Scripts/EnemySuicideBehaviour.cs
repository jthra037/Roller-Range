using UnityEngine;
using System.Collections;

public class EnemySuicideBehaviour : MonoBehaviour {

    public Collider attackVolume;
    public int health = 2;
    public int state = 1;

    [SerializeField]
    private float attackDistance = 5;
    [SerializeField]
    private float attackForce = 100;

    //private int layer;
    private float distance;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private Transform target;


	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform; // Locate the player
        //layer = gameObject.layer;
	}

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                chase();
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
        if (distance <= attackDistance)
        {
            attack();
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
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("We been hit!");
            health = health - other.gameObject.GetComponent<BulletBehaviour>().dmg;
            if (health <= 0)
            {
                Debug.Log("We dead");
                Destroy(gameObject);
            }
        }
    }
}
