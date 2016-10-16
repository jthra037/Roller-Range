using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    public GameObject projectile; //linked projectile prefab
    public Transform spawnPoint; //Transform of bullet spawner
    
	// Update is called once per frame
	void Update ()
    {
        //shoots if the player tries to shoot
		if (Input.GetButtonDown ("Fire1")) {
			GameObject thisShot;
			thisShot = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
			thisShot.tag = gameObject.tag;
		}
	}
}
