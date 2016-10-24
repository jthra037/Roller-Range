using UnityEngine;
using System.Collections;

public class EnemySuicideBehaviour : MonoBehaviour {

    public Collider attackVolume;
    public int health = 2;
    public int state = 1;

    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float attackDistance = 5;
    [SerializeField]
    private float attackForce = 100;

    private float distance;
    private Rigidbody rb;
    private Transform target;


	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform; // Locate the player
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
                Debug.Log("Wander isn't implemented yet!");
                break;
        }
    }

    void chase()
    {
        transform.LookAt(target); // Face the player
        distance = Vector3.Distance(transform.position, target.position); // Check the distance
        if (distance <= attackDistance)
        {
            attack();
        }
        else
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
