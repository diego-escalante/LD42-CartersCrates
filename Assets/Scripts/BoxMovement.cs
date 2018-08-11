using UnityEngine;

public class BoxMovement : MonoBehaviour {

    public float gravity = 10f;
    private float velocity = 0f;

    private BoxCollider2D coll;
    private Bounds bounds;
    private const float MARGIN = 0.01f;

    private BoxColorBehavior boxColorBehavior;

    public void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        bounds = coll.bounds;
        boxColorBehavior = GetComponent<BoxColorBehavior>();
    }

    void Update () {
        velocity -= gravity * Time.deltaTime;

        collisionCheck();

        transform.Translate(0, velocity * Time.deltaTime, 0);

        checkIfPastGoal();
	}

    private void collisionCheck(){
        float distance = Mathf.Abs(velocity * Time.deltaTime);
        Vector2 origin = (Vector2)transform.position + new Vector2(-bounds.extents.x + MARGIN, -bounds.extents.y);
        if (castVerticalRay(origin, distance)){
            return;
        }
        origin = (Vector2)transform.position + new Vector2(bounds.extents.x - MARGIN, -bounds.extents.y);
        if (castVerticalRay(origin, distance)){
            return;
        }
        origin = (Vector2)transform.position + new Vector2(0, -bounds.extents.y);
        castVerticalRay(origin, distance);
    }

    private bool castVerticalRay(Vector2 origin, float distance)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.down, distance);
        foreach(RaycastHit2D hit in hits) {
            if (hit.collider == coll){
                continue;
            }
            if (hit.collider.tag == "Player" || hit.collider.transform.root.tag == "Player") {
                EventManager.TriggerEvent("Player Hit");
                Destroy(gameObject);
                return true;
            } if (hit.collider.gameObject == boxColorBehavior.getDesiredGoal()) {
                if (transform.childCount > 0) {
                    transform.GetChild(0).transform.SetParent(null);
                }
                continue;
            } else {
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

    private void checkIfPastGoal() {
        if (transform.position.y < -6) {
            EventManager.TriggerEvent("Box Scored");
            Destroy(gameObject);
        }
    }
}
