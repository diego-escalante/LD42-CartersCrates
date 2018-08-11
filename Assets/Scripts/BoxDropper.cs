using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDropper : MonoBehaviour {
    public int range = 8;
    public float spawnRate = 10f;
    public GameObject boxPrefab;

    private float elapsedTime = 0f;

    public void Update() {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= spawnRate) {
            elapsedTime -= spawnRate;
            Instantiate(boxPrefab, new Vector3(Random.Range(-range, range+1), 6, 0), Quaternion.identity);
        }
    }
}
