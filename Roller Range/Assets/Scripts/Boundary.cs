using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {
    
    void OnTriggerExit(Collider other)
    {
        Debug.Log(gameObject.name + " caught " + other.name + " trying to leave.");
        Destroy(other.gameObject);
    }
}
