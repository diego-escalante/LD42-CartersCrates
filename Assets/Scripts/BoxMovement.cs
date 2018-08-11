using UnityEngine;

public class BoxMovement : MonoBehaviour {

    public float gravity = 10f;
    private float velocity = 0f;

    private BoxCollider2D coll;
    private Bounds bounds;
    private const float MARGIN = 0.01f;

    public void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        bounds = coll.bounds;
    }

    void Update () {
        velocity -= gravity * Time.deltaTime;

        collisionCheck();

        transform.Translate(0, velocity * Time.deltaTime, 0);
	}

    private void collisionCheck(){
        float distance = Mathf.Abs(velocity * Time.deltaTime);
        Vector2 origin = (Vector2)transform.position + new Vector2(-bounds.extents.x + MARGIN, -bounds.extents.y);
        if (castVerticalRay(origin, distance)){
            return;
        }
        origin = (Vector2)transform.position + new Vector2(bounds.extents.x - MARGIN, -bounds.extents.y);
        castVerticalRay(origin, distance);
    }

    private bool castVerticalRay(Vector2 origin, float distance)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.down, distance);
        foreach(RaycastHit2D hit in hits) {
            if (hit.collider == coll){
                continue;
            }
            if (hit.collider.tag == "Player") {
                Debug.Log("Player hit! Ouch!");
                Destroy(gameObject);
                return true;
            }
            else {
                velocity = 0;
                transform.Translate(Vector2.down * hit.distance);
                if (hit.collider.tag == "Box") {
                    transform.SetParent(hit.collider.transform);
                }
                return true;
            }
        }
        return false;
    }
}
