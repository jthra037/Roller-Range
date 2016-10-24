using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    public GameObject projectile; //linked projectile prefab
    public Transform spawnPoint; //Transform of bullet spawner
    public float speed = 15;
    public Transform myCamera;

    public float rotSpeedY = 20;
    public float rotSpeedX = 40;
    public bool yInvert = false;
    public float minRot = -70;
    public float maxRot = 70;

    [SerializeField]
    private float fireRate = 1f;

    private float lastShot;
    private Rigidbody rb;
    private Vector3 movement;
    private int layer;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        layer = gameObject.layer;
        lastShot = Time.time;
    }

	// Update is called once per frame
	void Update ()
    {
        //shoots if the player tries to shoot
        if (Input.GetButton ("Fire1") && (Time.time > fireRate + lastShot)) {
			GameObject thisShot;
			thisShot = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
            thisShot.layer = layer;
            lastShot = Time.time;
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

    void FixedUpdate()
    {
        rb.velocity = movement;
    }
}
