using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColorBehavior : MonoBehaviour {
    public string boxColor;

    private GameObject desiredGoal;

    // Would have been nice to have an enumerator.
    private string[] availableColors = new string[] {"red", "green", "blue"};

    public void Start() {
        boxColor = availableColors[Random.Range(0, availableColors.Length)];
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (boxColor == "red") {
            spriteRenderer.color = Color.red;
        } else if (boxColor == "green") {
            spriteRenderer.color = Color.green;
        } else {
            spriteRenderer.color = Color.blue;
        }

        desiredGoal = GameObject.FindWithTag("Goal" + boxColor);
    }

    public GameObject getDesiredGoal(){
        return desiredGoal;
    }
}
