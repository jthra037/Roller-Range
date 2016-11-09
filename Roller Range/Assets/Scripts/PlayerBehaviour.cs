using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    //Public links to things outside the script
    public GameObject projectile; //linked projectile prefab
    public Transform spawnPoint; //Transform of bullet spawner
    public float speed = 15;
    public Transform myCamera;

    //Public vars for camera movement
    public float rotSpeedY = 20;
    public float rotSpeedX = 40;
    public bool yInvert = false;
    public float minRot = -70;
    public float maxRot = 70;

    // Private vars for movement
    private Rigidbody rb;
    private Vector3 movement;
    private int layer;

    // Private vars for weapons
    private int wepIndex = 1;
    private float pistolRoF = 1.0f;
    private float nextShot;
    private float tmgRoF = 0.3f;
    [SerializeField]
    private float tmgAccuracy = 5.0f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        layer = gameObject.layer;
        nextShot = Time.time;
    }

	// Update is called once per frame
	void Update ()
    {
        //shoots if the player tries to shoot
		if (Input.GetButton ("Fire1")) {
            switch (wepIndex)
            {
                case 0:
                    shootPistol();
                    break;
                case 1:
                    shootTMG();
                    break;
                default:
                    Debug.Log("Somehow didn't have a weapon equipped.. Change weapons!");
                    break;
            }
		}

        //Rotation controls
        transform.Rotate(0, (Input.GetAxis("Mouse X") * rotSpeedX * Time.deltaTime), 0, Space.World); // Turn the player on x

        if (yInvert) // Change the camera on y
            myCamera.Rotate(Mathf.Clamp((Input.GetAxis("Mouse Y") * rotSpeedY * Time.deltaTime), minRot, maxRot), 0, 0, Space.Self);
        else
            myCamera.Rotate(Mathf.Clamp((Input.GetAxis("Mouse Y") * -rotSpeedY * Time.deltaTime), minRot, maxRot), 0, 0, Space.Self);

        // Move by Player forward
        movement = (transform.forward * Input.GetAxisRaw("Horizontal") * -speed) + (transform.right * Input.GetAxisRaw("Vertical") * speed);
    }

    void shootPistol()
    {
        if (Time.time >= nextShot)
        {
            GameObject thisShot;
            thisShot = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
            thisShot.layer = layer;
            nextShot = Time.time + pistolRoF;
        }
    }

    void shootTMG()
    {
        if (Time.time >= nextShot)
        {
            spawnTMGShot();
            spawnTMGShot();
            spawnTMGShot();
            //Debug.Break();
            nextShot = Time.time + tmgRoF;
        }
    }

    void spawnTMGShot()
    {
        GameObject thisShot;
        Transform thisSpawn = spawnPoint;

        // Adjust spawn transform
        
        // Spawn shot
        thisShot = Instantiate(projectile, thisSpawn.position, thisSpawn.rotation) as GameObject;
        thisShot.layer = layer;
    }

    void FixedUpdate()
    {
        rb.velocity = movement;
    }
}
