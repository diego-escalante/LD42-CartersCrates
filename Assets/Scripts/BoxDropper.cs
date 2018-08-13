using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDropper : MonoBehaviour {
    public int range = 8;
    public float spawnRate = 10f;
    public GameObject boxPrefab;
    public float decayPerMinute = 0.5f;

    private float elapsedTime = 0f;

    private bool currentlySpawning = false;

    public void OnEnable() {
        EventManager.StartListening("Player Hit", spawnPenalty);
        EventManager.StartListening("Start Game", startGame);
        EventManager.StartListening("Game Over", endGame);
    }

    public void OnDisable() {
        EventManager.StopListening("Player Hit", spawnPenalty);
        EventManager.StopListening("Start Game", startGame);
        EventManager.StopListening("Game Over", endGame);
    }

    private void startGame() {
        currentlySpawning = true;
        spawnPenalty();
    }

    private void endGame() {
        currentlySpawning = false;
    }

    public void Update() {
        if (!currentlySpawning) {
            return;
        }

        elapsedTime += Time.deltaTime;

        spawnRate -= Time.deltaTime * (decayPerMinute / 60);

        if (elapsedTime >= spawnRate) {
            elapsedTime -= spawnRate;
            spawnPenaltyWithArgs(1, false);
        }
    }

    private void spawnPenalty() {
        spawnPenaltyWithArgs(3, true);
    }

    private void spawnPenaltyWithArgs(int amountOfBoxes, bool ignorePlayer) {
        int attempts = 50;
        while(amountOfBoxes > 0) {
            if (attempts < 0) {
                Debug.LogWarning("Could not spawn boxes");
                return;
            }
            attempts--;
            int i = Random.Range(-range, range + 1);

            if (ignorePlayer && Mathf.Abs(i - GameObject.FindWithTag("Player").transform.position.x) < 3) {
                continue;
            }
            if (Physics2D.OverlapPoint(new Vector2(i, 5.501f)) != null) {
                continue;
            }
            Instantiate(boxPrefab, new Vector3(i, 6, 0), Quaternion.identity);
            amountOfBoxes--;
        }
    }
}
