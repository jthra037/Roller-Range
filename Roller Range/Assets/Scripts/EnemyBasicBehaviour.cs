using UnityEngine;
using System.Collections;

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
        Debug.Log("My state is: " + state);
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
        Debug.Log(distance - tolerance + " > " + attackDistance/1.5 + " = " + (distance - tolerance > (attackDistance / 1.5)));

        if (distance > attackDistance)
        {
            state = 0;           
        } else if (distance - tolerance > (attackDistance/1.5))
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
