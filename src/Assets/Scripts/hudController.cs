using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hudController : MonoBehaviour
{
    public string state = "Running";
    public int generation = 0;
    public float maxHeight = 0f;
    public float magnitudeModifier = 0f;
    public float directionModifier = 0f;

    private TextMeshProUGUI hudText;

    void Start()
    {
        hudText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        hudText.text = (
            "State: " + state + "\n" +
            "Generation: " + generation + "\n" +
            "Max Height: " + maxHeight + "\n" +
            "Magnitude Modifier: " + magnitudeModifier + "\n" +
            "Direction Modifier: " + directionModifier
        );
    }
}


