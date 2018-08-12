using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairBehavior : MonoBehaviour {

    public float scaleMax = 1.1f;
    public float scaleMin = 0.9f;
    public float pulseTime = 0.5f;

    private bool growing = false;
    private float elapsedTime = 0;

	void Update () {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > pulseTime) {
            elapsedTime -= pulseTime;
            growing = !growing;
        }

        float currentScale = Mathf.Lerp(growing ? scaleMax : scaleMin, growing ? scaleMin : scaleMax, elapsedTime / pulseTime);
        transform.localScale = Vector3.one * currentScale;
	}
}
