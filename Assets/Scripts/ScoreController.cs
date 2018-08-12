using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {

    private int score = 0;

	void OnEnable () {
        EventManager.StartListening("Score", boxScored);
	}

	void OnDisable () {
        EventManager.StopListening("Score", boxScored);
	}

    public void boxScored(){
        score++;
        Debug.Log("Box scored! Score: " + score);
    }
}
