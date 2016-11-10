using UnityEngine;
using System.Collections;

public class WallThoughts : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (KillMe ());
	}

	IEnumerator KillMe()
	{
		yield return new WaitForSeconds (15f);
		Destroy (gameObject);
	}
}
