using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public bool canSeePlayer = false;

	private Transform target;
    private GameController GC;

	void Start()
	{
		target = GameObject.FindGameObjectWithTag ("Player").transform;
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	void Update()
	{
		transform.LookAt (target.position);
	}

	void FixedUpdate()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
		{
			GameObject hitObject = hit.collider.gameObject;
			if (hitObject.CompareTag ("Player")) {
				canSeePlayer = true;
			} else {
				canSeePlayer = false;
			}
		}
	}

	public void spawn(GameObject prefab, Vector3 localOffset)
	{
        Instantiate(prefab, transform.position + localOffset, transform.rotation);
	}
}
