using UnityEngine;
using System.Collections;

public class InstructionsToggle : MonoBehaviour {
    [SerializeField]
    private GameObject instructions;
    [SerializeField]
    private GameObject title;

    void instructionsToggle()
    {
        instructions.SetActive(!instructions.activeSelf);
        title.SetActive(!instructions.activeSelf);
    }
}
