using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    //Public links to things outside the script
    public GameObject projectile; //linked projectile prefab
	public GameObject projWall;
	public Transform spawnPoint; //Transform of bullet spawner
    public float speed = 15;
    public Transform myCamera;
	public int health = 20;

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
	private int wepIndex = 0;
    private float pistolRoF = 1.0f;
    private float nextShot;
	private float tmgRoF = 0.15f;
	[SerializeField] 
	private float wallRoF = 0.5f;

	private int combo = 0;
	private bool runTimer = true;
	private IEnumerator coroutine;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        layer = gameObject.layer;
        nextShot = Time.time;
		coroutine = upgradeTimer ();
		StartCoroutine (coroutine);
    }

	// Update is called once per frame
	void Update ()
    {

		if (runTimer) {
			StartCoroutine (coroutine);
		}

		if (Input.GetKey (KeyCode.Alpha1)) {
			wepIndex = 0;
			Debug.Log (wepIndex);
		}
		if (Input.GetKey (KeyCode.Alpha2) && combo > 0) {
			wepIndex = 1;
			Debug.Log (wepIndex);
		}
		if (Input.GetKey (KeyCode.Alpha3) && combo > 1) {
			wepIndex = 2;
			Debug.Log (wepIndex);
		}

        //shoots if the player tries to shoot
		if (Input.GetButton ("Fire1")) {
            switch (wepIndex)
            {
                case 0:
                    shootPistol();
                    break;
                case 1:
                    shootWalls();
                    break;
				case 2:
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
			GameObject thisShot;
			thisShot = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
			thisShot.layer = layer;
            nextShot = Time.time + tmgRoF;
        }
    }

	void shootWalls()
	{
		if (Time.time >= nextShot)
		{
			RaycastHit hit;
			GameObject thisShot;
			thisShot = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
			thisShot.layer = layer;

			if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hit, Mathf.Infinity))
			{
				Instantiate (projWall, hit.point, spawnPoint.rotation);
			}

			nextShot = Time.time + wallRoF;
		}
	}

	void hit()
	{
		StopCoroutine (coroutine);
		runTimer = true;
		coroutine = upgradeTimer ();
		--health;
		--combo;
		combo = (combo < 0) ? 0 : combo;
		wepIndex = (combo < wepIndex) ? combo : wepIndex;
		Debug.Log ("Player hit");
		Debug.Log ("Player health at: " + health);
	}

    void FixedUpdate()
    {
        rb.velocity = movement;
    }

	IEnumerator upgradeTimer()
	{
		runTimer = false;
		yield return new WaitForSeconds (15);
		++combo;
		runTimer = true;
		coroutine = upgradeTimer ();
	}
}
