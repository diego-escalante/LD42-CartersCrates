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
            Instantiate(boxPrefab, new Vector3(Random.Range(-range, range+1), 6, 0), Quaternion.identity);
        }
    }

    private void spawnPenalty() {
        float xPlayerPos = GameObject.FindWithTag("Player").transform.position.x;

        // Create an arraylist with all options.
        List<int> possibleNumbers = new List<int>();
        for (int i = -range; i <= range; i++) {
            if (Mathf.Abs(i - xPlayerPos) < 3) {
                continue;
            }
            possibleNumbers.Add(i);
        }

        int amountOfBoxes = 3;

        for (; amountOfBoxes > 0 || possibleNumbers.Count == 0; amountOfBoxes--) {
            int i = Random.Range(0, possibleNumbers.Count);
            int x = possibleNumbers[i];
            possibleNumbers.RemoveAt(i);

            Collider2D coll = Physics2D.OverlapBox(new Vector2(x, 6), Vector2.one, 0);
            if (coll != null) {
                amountOfBoxes++;
                continue;
            }

            Instantiate(boxPrefab, new Vector3(x, 6, 0), Quaternion.identity);
        }
    }
}
