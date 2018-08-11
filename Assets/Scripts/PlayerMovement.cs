using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float runSpeed;
    public float gravity;
    public float jumpStrength;

    private bool facingRight;

    private Vector2 velocity = Vector2.zero;
    private bool grounded = false;
    private BoxCollider2D coll;
    private Bounds bounds;
    private SpriteRenderer spriteRenderer;
    private LayerMask solidMask = new LayerMask();

    private const float MARGIN = 0.01f;
    private const int RAY_NUMBER = 2;

    public void Start() {
        coll = GetComponent<BoxCollider2D>();
        bounds = coll.bounds;
        solidMask = LayerMask.GetMask("Solid") | LayerMask.GetMask("Boxes");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        // Moving Horizontally.
        velocity.x = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (velocity.x > 0){
            facingRight = true;
        } else if (velocity.x < 0) {
            facingRight = false;
        }

        spriteRenderer.flipX = !facingRight;

        // Falling and Jumping.
        if(!grounded) {
            velocity.y -= gravity * Time.deltaTime;
        } else if (Input.GetButtonDown("Jump")) {
            velocity.y = jumpStrength;
            grounded = false;
        }

        //Collision Checks.
        verticalCollisionCheck();
        horizontalCollisionCheck();
   
        // Translate.
        transform.Translate((Vector3)velocity * Time.deltaTime);
    }

    private void horizontalCollisionCheck() {
        // Return early if we are not moving. Why bother?
        if (velocity.x == 0) {
            return;
        }

        // Set the direction of the check.
        Vector2 direction = velocity.x > 0 ? Vector2.right : Vector2.left;

        // Calculate the distance.
        float distance = Mathf.Abs(velocity.x * Time.deltaTime);

        // Calculate the orgin of rays and shooooooot!
        Vector2 origin;
        origin = (Vector2)transform.position + new Vector2(bounds.extents.x * direction.x, -bounds.extents.y + MARGIN);
        if (castHorizontalRay(origin, direction, distance)) {
            return;
        }
        origin = (Vector2)transform.position + new Vector2(bounds.extents.x * direction.x, bounds.extents.y - MARGIN);
        castHorizontalRay(origin, direction, distance);
    }

    private bool castHorizontalRay(Vector2 origin, Vector2 direction, float distance) {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, solidMask);
        Debug.DrawRay(origin, direction * distance, hit.collider == null ? Color.green : Color.red);

        if (hit.collider != null) {
            velocity.x = 0;
            transform.Translate(direction * hit.distance);
            return true;
        }
        return false;
    }

    private void verticalCollisionCheck() {
        if (velocity.y > 0) {
            return;
        }

        // Set the direction of the check.
        Vector2 direction = Vector2.down;
        if (velocity.y > 0) {
            direction = Vector2.up;
        }

        // Assume we are not grounded.
        grounded = false;

        // Calculate the distance that will be traveled. (If the velocity is 0, check just a little bit anyway, in case we fall down a cliff or something.)
        float distance = velocity.y == 0 ? MARGIN : Mathf.Abs(velocity.y * Time.deltaTime);

        // Calculate origin of ray and shoot.
        Vector2 origin;
        origin = (Vector2)transform.position + new Vector2(-bounds.extents.x + MARGIN, bounds.extents.y * direction.y + coll.offset.y);
        if (castVerticalRay(origin, direction, distance)) {
            return;
        }
        origin = (Vector2)transform.position + new Vector2(bounds.extents.x - MARGIN, bounds.extents.y * direction.y + coll.offset.y);
        castVerticalRay(origin, direction, distance);
    }

    private bool castVerticalRay(Vector2 origin, Vector2 direction, float distance) {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, solidMask);
        Debug.DrawRay(origin, direction * distance, hit.collider == null ? Color.green : Color.red);

        if (hit.collider != null) {
            velocity.y = 0;
            transform.Translate(direction * hit.distance);
            if (direction == Vector2.down)
            {
                grounded = true;
            }
            return true;
        }
        return false;
    }

    public bool getFacingRight() {
        return facingRight;
    }
}