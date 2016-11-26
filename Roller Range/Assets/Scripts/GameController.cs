using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public bool running = true;
	public float spawnRate = 3f;
    public Image levelBar;
    public Text levelText;
    private int gameScore = 0;
    public Text finalScore;
    public int level = 1;

    private Spawner[] spawnPoints;
	private int spawnIndex;
	private Transform player;
    private bool playerExists = true;

    private int levelTime = 15;

	[SerializeField]
	private List<GameObject> creepPrefabs = new List<GameObject>(3);

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		spawnPoints = transform.GetComponentsInChildren<Spawner> ();

		StartCoroutine (levelTimer ());
		StartCoroutine (spawning ());
	}

    void Update()
    {
        if (playerExists && player.gameObject.GetComponent<PlayerBehaviour>().dead)
        {
            int score = player.gameObject.GetComponent<PlayerBehaviour>().score;
            score *= level;
            finalScore.text = "Final score: " + gameScore.ToString();
            StartCoroutine(gameOver());
            playerExists = false;
            Destroy(player.gameObject.GetComponent<PlayerBehaviour>());
        }
    }

	int getPrefabIndex()
	{
		int tempIndex = (int)(Random.value * 100);
		tempIndex = (tempIndex % level) % creepPrefabs.Count;

		return tempIndex;
	}

    public void gotCombo()
    {
        gameScore += 100 * level;
    }

    public void iDied(int points)
    {
        gameScore += points * level;
    }

	IEnumerator levelTimer()
	{
		while (running) {
            for (int i = 0; i < levelTime; ++i)
            {
                levelBar.fillAmount = (float)i / levelTime;
                yield return new WaitForSeconds(1);
            }
            ++level;
            levelText.text = "Current level: " + level.ToString();
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

    IEnumerator gameOver()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("GameOver");
    }
}
