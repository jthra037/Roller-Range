using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {
    
    void OnTriggerExit(Collider other)
	{
		if (other.CompareTag ("Player")) {
			other.GetComponent<PlayerBehaviour> ().dead = true;
		} else {
			Debug.Log (gameObject.name + " caught " + other.name + " trying to leave.");
			Destroy (other.gameObject);
    
		}
	}
}
