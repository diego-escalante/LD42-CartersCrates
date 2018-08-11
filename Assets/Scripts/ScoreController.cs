using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {

    private int score = 0;

	void OnEnable () {
        EventManager.StartListening("Box Scored", boxScored);
	}

	void OnDisable () {
        EventManager.StopListening("Box Scored", boxScored);
	}

    public void boxScored(){
        score++;
        Debug.Log("Box scored! Score: " + score);
    }
}
