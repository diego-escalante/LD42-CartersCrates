using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningBehavior : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private float delay = 0.25f;
    private float timeElapsed = 0;

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        Collider2D coll = Physics2D.OverlapPoint(transform.position);
        if (coll != null) {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > delay) {
                spriteRenderer.enabled = true;
            }
        } else {
            spriteRenderer.enabled = false;
            timeElapsed = 0;
        }
    }

}
