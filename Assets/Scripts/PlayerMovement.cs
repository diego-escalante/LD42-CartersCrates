using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float runSpeed;
    public float gravity;
    public float jumpStrength;

    private Vector2 velocity = Vector2.zero;
    private bool grounded = false;
    private BoxCollider2D coll;
    private Bounds bounds;
    private LayerMask solidMask = new LayerMask();

    private const float MARGIN = 0.01f;
    private const int RAY_NUMBER = 2;

    public void Start() {
        coll = GetComponent<BoxCollider2D>();
        bounds = coll.bounds;
        solidMask = LayerMask.GetMask("Solid");
    }

    public void Update() {
        // Moving Horizontally.
        velocity.x = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Falling and Jumping.
        if(!grounded) {
            velocity.y -= gravity * Time.deltaTime;
        } else if (Input.GetButtonDown("Jump")) {
            velocity.y = jumpStrength;
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
        origin = (Vector2)transform.position + new Vector2(-bounds.extents.x + MARGIN, bounds.extents.y * direction.y);
        if (castVerticalRay(origin, direction, distance)) {
            return;
        }
        origin = (Vector2)transform.position + new Vector2(bounds.extents.x - MARGIN    , bounds.extents.y * direction.y);
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
}

//public float gravity = -9.8f;
//public Vector2 maxVelocity = new Vector2(10f, 20f);

//public float jumpSpeed = 1f;

//public float runningAcceleration = 100f;

//private Vector2 velocity = Vector2.zero;
//private BoxCollider2D coll;
//private Bounds bounds;
//private LayerMask solidMask = new LayerMask();
//private const int RAY_NUMBER = 2;
//private bool grounded = false;

//private const float MARGIN = 0.01f;


//void Start()
//{
//    if (maxVelocity.x < 0 || maxVelocity.y < 0)
//    {
//        Debug.LogError("maxVelocity has negative values!");
//    }

//    coll = GetComponent<BoxCollider2D>();
//    bounds = coll.bounds;
//    solidMask = LayerMask.GetMask("Solid");
//}

//void Update()
//{
//    applyGravity();
//    doJump();
//    doMove();
//    doVerticalCollisionCheck();
//    doHorizontalCollisionCheck();

//    transform.position += (Vector3)velocity * Time.deltaTime;
//}

//private void doVerticalCollisionCheck()
//{
//    // No need to do a vertical collision check if we are not moving and there's no gravity.
//    if (gravity == 0 && velocity.y == 0)
//    {
//        return;
//    }

//    // Assume we are not grounded, so that we can fall if we move out of solid ground.
//    grounded = false;

//    // If we are not moving, make the direction to check for collisions point to the direction of gravity.
//    Vector2 direction = velocity.y == 0 ? Mathf.Sign(gravity) * Vector2.up : Mathf.Sign(velocity.y) * Vector2.up;
//    float distance = velocity.y == 0 ? MARGIN : Mathf.Abs(velocity.y) * Time.deltaTime;

//    Vector2 originStart = (Vector2)transform.position + (Vector2)bounds.center + new Vector2(-bounds.extents.x + MARGIN, bounds.extents.y * direction.y);
//    Vector2 originFinish = (Vector2)transform.position + (Vector2)bounds.center + new Vector2(bounds.extents.x - MARGIN, bounds.extents.y * direction.y);

//    RaycastHit2D hit = castRays(direction, distance, originStart, originFinish);

//    if (hit.collider != null)
//    {
//        velocity.y = 0;
//        transform.Translate(direction * hit.distance);
//        //TODO - is this the correct logic for setting grounded?
//        grounded = direction.y == Mathf.Sign(gravity);
//    }
//}

//private void doHorizontalCollisionCheck()
//{
//    // No need to do a horizonal collision check if we are not moving.
//    if (velocity.x == 0)
//    {
//        return;
//    }

//    Vector2 direction = Mathf.Sign(velocity.x) * Vector2.right;
//    float distance = Mathf.Abs(velocity.x) * Time.deltaTime;

//    Vector2 originStart = (Vector2)transform.position + (Vector2)bounds.center + new Vector2(bounds.extents.x * direction.x, -bounds.extents.y + MARGIN);
//    Vector2 originFinish = (Vector2)transform.position + (Vector2)bounds.center + new Vector2(bounds.extents.x * direction.x, bounds.extents.y - MARGIN);

//    RaycastHit2D hit = castRays(direction, distance, originStart, originFinish);

//    if (hit.collider != null)
//    {
//        velocity.x = 0;
//        transform.Translate(direction * hit.distance);
//    }
//}

//private void applyGravity()
//{
//    if (!grounded)
//    {
//        velocity.y = Mathf.Clamp(velocity.y + gravity * Time.deltaTime, -maxVelocity.y, maxVelocity.y);
//    }
//}

//private void doJump()
//{
//    if (grounded && Input.GetButtonDown("Jump"))
//    {
//        velocity.y = jumpSpeed * -Mathf.Sign(gravity);
//        grounded = false;
//    }
//}

//private void doMove()
//{
//    float targetSpeed = Input.GetAxisRaw("Horizontal") * maxVelocity.x;
//    if (velocity.x == targetSpeed)
//    {
//        return;
//    }
//    velocity.x = velocity.x > targetSpeed ? Mathf.Max(targetSpeed, velocity.x - runningAcceleration * Time.deltaTime) : Mathf.Min(targetSpeed, velocity.x + runningAcceleration * Time.deltaTime);
//}

//private RaycastHit2D castRays(Vector2 direction, float distance, Vector2 originStart, Vector2 originFinish)
//{
//    for (int i = 0; i < RAY_NUMBER; i++)
//    {
//        float t = i / (float)(RAY_NUMBER - 1);
//        Vector2 currentOrigin = Vector2.Lerp(originStart, originFinish, t);
//        RaycastHit2D hit = Physics2D.Raycast(currentOrigin, direction, distance, solidMask);
//        Debug.DrawRay(currentOrigin, direction * distance, hit.collider == null ? Color.green : Color.red);
//        if (hit.collider != null)
//        {
//            return hit;
//        }
//    }
//    return new RaycastHit2D();
//}
