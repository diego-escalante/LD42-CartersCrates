using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDropper : MonoBehaviour {
    public int range = 8;
    public float spawnRate = 10f;
    public GameObject boxPrefab;

    private float elapsedTime = 0f;

    public void OnEnable() {
        EventManager.StartListening("Player Hit", spawnPenalty);
    }

    public void OnDisable() {
        EventManager.StopListening("Player Hit", spawnPenalty);
    }

    public void Update() {
        elapsedTime += Time.deltaTime;

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
            Debug.Log(x);
        }
    }
}
