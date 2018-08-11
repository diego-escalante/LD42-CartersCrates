using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour {

    public float gravity = 10f;
    private float velocity = 0f;

    private BoxCollider2D coll;
    private Bounds bounds;
    private LayerMask solidMask = new LayerMask();
    private bool done = false;

    public void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        bounds = coll.bounds;
        solidMask = LayerMask.GetMask("Solid");
    }

    void Update () {
        if(done) {
            Destroy(this);
            return;
        }
        velocity -= gravity * Time.deltaTime;

        castRay();

        transform.Translate(0, velocity * Time.deltaTime, 0);
	}

    private void castRay(){
        Vector2 origin = (Vector2)transform.position + new Vector2(0, -bounds.extents.y);
        float distance = Mathf.Abs(velocity * Time.deltaTime);
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.down, distance, solidMask);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider != coll) {
                velocity = 0;
                transform.Translate(hit.distance * Vector2.down);
                done = true;
                return;
            }
        }
    }
}
