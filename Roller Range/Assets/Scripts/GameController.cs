using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public bool running = true;
	public float spawnRate = 3f;

	private Spawner[] spawnPoints;
	private int spawnIndex;
	private Transform player;
	private int level = 1;

	[SerializeField]
	private List<GameObject> creepPrefabs = new List<GameObject>(3);

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		spawnPoints = transform.GetComponentsInChildren<Spawner> ();

		StartCoroutine (levelTimer ());
		StartCoroutine (spawning ());
	}

	int getPrefabIndex()
	{
		int tempIndex = (int)Random.value * 100;
		tempIndex = (tempIndex % level) % creepPrefabs.Count;

		return tempIndex;
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
				
			prefabIndex = getPrefabIndex();
			float prefabHeight = creepPrefabs [prefabIndex].transform.localScale.y;
			Vector3 offset = new Vector3 (0, prefabHeight / 2, 0);

			spawnPoints [spawnIndex].spawn (creepPrefabs [prefabIndex], offset);
			yield return new WaitForSeconds(spawnRate);
		}
	}
}
