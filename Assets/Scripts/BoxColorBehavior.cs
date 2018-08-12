using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColorBehavior : MonoBehaviour {
    public string boxColor;
    public Sprite redBox;
    public Sprite blueBox;
    public Sprite greenBox;

    private GameObject desiredGoal;

    // Would have been nice to have an enumerator.
    private string[] availableColors = new string[] {"red", "green", "blue"};

    public void Start() {
        boxColor = availableColors[Random.Range(0, availableColors.Length)];
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (boxColor == "red") {
            spriteRenderer.sprite = redBox;
        } else if (boxColor == "green") {
            spriteRenderer.sprite = greenBox;
        } else {
            spriteRenderer.sprite = blueBox;
        }

        desiredGoal = GameObject.FindWithTag("Goal" + boxColor);
    }

    public GameObject getDesiredGoal(){
        return desiredGoal;
    }
}
