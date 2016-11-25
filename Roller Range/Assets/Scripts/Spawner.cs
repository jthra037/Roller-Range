using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	private Transform target;

	void Start()
	{
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update()
	{
		transform.LookAt (target.position);
	}

	public void spawn(GameObject prefab, Vector3 localOffset)
	{
		Instantiate (prefab, transform.position + localOffset, transform.rotation);
	}
}
