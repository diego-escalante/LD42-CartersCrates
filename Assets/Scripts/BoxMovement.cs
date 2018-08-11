using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour {

    public float gravity = 10f;
    private float velocity = 0f;

    private BoxCollider2D coll;
    private Bounds bounds;
    private LayerMask solidMask = new LayerMask();

    public void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        bounds = coll.bounds;
        solidMask = LayerMask.GetMask("Solid");
    }

    void Update () {
        velocity -= gravity * Time.deltaTime;

        castRay();

        transform.Translate(0, velocity * Time.deltaTime, 0);
	}

    private void castRay(){
        Vector2 origin = (Vector2)transform.position + new Vector2(0, -bounds.extents.y);
        float distance = Mathf.Abs(velocity * Time.deltaTime);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, distance, solidMask);
        Debug.DrawRay(origin, Vector2.down * distance, hit.collider == null ? Color.green : Color.red);
        if (hit.collider != null && hit.collider != coll) {
            velocity = 0;
            transform.Translate(hit.distance * Vector2.down);
            Destroy(this);
        }
    }
}
