using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public bool running = true;
	public float spawnRate = 3f;

	private Spawner[] spawnPoints;
	private int spawnIndex;
	private Transform player;
	private int level = 0;

	[SerializeField]
	private List<GameObject> creepPrefabs = new List<GameObject>(3);

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		spawnPoints = transform.GetComponentsInChildren<Spawner> ();

		StartCoroutine (levelTimer ());
		StartCoroutine (spawning ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	int getPrefab()
	{
		return 0;
	}

	IEnumerator levelTimer()
	{
		while (running) {
			yield return new WaitForSeconds (15);
			++level;
		}
	}

	IEnumerator spawning()
	{
		int prefabIndex;

		while (running) {
			spawnIndex = ++spawnIndex % spawnPoints.Length;
			while (spawnPoints [spawnIndex].canSeePlayer) {
				spawnIndex = ++spawnIndex % spawnPoints.Length;
			}


			prefabIndex = getPrefab();
			yield return new WaitForSeconds(spawnRate);
		}
	}
}
