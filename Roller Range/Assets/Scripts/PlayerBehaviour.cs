﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    //Public links to things outside the script
    public GameObject projectile; //linked projectile prefab
	public GameObject projWall;
	public Transform spawnPoint; //Transform of bullet spawner
    public float speed = 15;
    public Transform myCamera;
	private int health = 20;
    public Image healthBar;
    public Image wepBar;
    public Text scoreTxt;
    public Text wepText;

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

    // Stuff for the UI
    private int maxHealth;
    private float maxWidth;
    public int score = 0;
    private int upgradeTime;
    public bool dead = false;

	// Make the game run
    private GameController GC;

	// Don't cheat!
	private float maxHeight;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        layer = gameObject.layer;
        nextShot = Time.time;
        upgradeTime = 15;
		coroutine = upgradeTimer ();
		StartCoroutine (coroutine);

        maxHealth = health;
		maxHeight = transform.position.y + 1f;
    }

	// Update is called once per frame
	void Update ()
    {
		if (dead) {
			rb.velocity = Vector3.zero;
			return;
		}

		if (runTimer) {
			StartCoroutine (coroutine);
		}

		if (Input.GetKey (KeyCode.Alpha1)) {
			wepIndex = 0;
            wepText.text = "Weapon Equipped: " + (wepIndex + 1).ToString();
        }
		if (Input.GetKey (KeyCode.Alpha2) && combo > 0) {
			wepIndex = 1;
            wepText.text = "Weapon Equipped: " + (wepIndex + 1).ToString();
        }
		if (Input.GetKey (KeyCode.Alpha3) && combo > 1) {
			wepIndex = 2;
            wepText.text = "Weapon Equipped: " + (wepIndex + 1).ToString();
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
        scoreTxt.text = combo.ToString();
        wepIndex = (combo < wepIndex) ? combo : wepIndex;
        wepText.text = "Weapon Equipped: " + (wepIndex + 1).ToString();
        //float newWidth = ((float)health / maxHealth) * maxWidth;
        float newWidth = ((float)health / maxHealth);
        healthBar.fillAmount = newWidth;
        if (health <= 0)
            dead = true;
    }

    void FixedUpdate()
    {
        rb.velocity = movement;
    }

	IEnumerator upgradeTimer()
	{
        Vector3 startPos = transform.position;
		runTimer = false;
        for (int i = 0; i < upgradeTime; ++i)
        {
            wepBar.fillAmount = (float)i / upgradeTime;
            yield return new WaitForSeconds(1);
        }
        if (((transform.position - startPos).magnitude < 1) && (rb.velocity.magnitude < 0.2))
        {
            Debug.Log("Are you cheating?");
            yield break;
        }
		if (transform.position.y > maxHeight) {
			dead = true;
			yield break;
		}
        ++combo;
        GC.gotCombo();
        wepText.text = "Weapon Equipped: " + (wepIndex + 1).ToString();
        scoreTxt.text = combo.ToString();
		runTimer = true;
		coroutine = upgradeTimer ();
	}
}
