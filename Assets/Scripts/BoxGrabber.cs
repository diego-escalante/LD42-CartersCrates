using UnityEngine;

public class BoxGrabber : MonoBehaviour {

    public int boxTowerLimit = 3;
    private PlayerMovement playerMovement;
    private GameObject carriedBox = null;
    private Collider2D coll = null;
    private LayerMask boxMask = new LayerMask();
    private const float MARGIN = 0.01f;


    void Start () {
        playerMovement = GetComponent<PlayerMovement>();
        coll = GetComponent<Collider2D>();
        boxMask = LayerMask.GetMask("Boxes");
    }
	
	void Update () {
		if (Input.GetButtonDown("Action")) {
            doAction();
        }
	}

    private void doAction() {
        bool isFacingRight = playerMovement.getFacingRight();
        float reach = 1f;

     
        if (carriedBox == null)
        {
            // Picking up a box.
            Vector2 origin = new Vector2(transform.position.x + (coll.bounds.extents.x * (isFacingRight ? 1 : -1)), transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * (isFacingRight ? 1 : -1), reach, boxMask);
            if (hit.collider != null && canPickupBoxes(hit.collider.transform)) {
                carriedBox = hit.collider.gameObject;
                carriedBox.transform.SetParent(this.transform);
                carriedBox.transform.localPosition = new Vector3(0, 1, 0);
                setBoxTowerEnabledValue(carriedBox.transform, false);
            }
        } else {
            // Dropping a box.
            float edgeOfPlayer = transform.position.x + ((coll.bounds.extents.x - MARGIN) * (isFacingRight ? 1 : -1));
            float x;
            if (isFacingRight) {
                x = Mathf.Round(edgeOfPlayer) + 1;
            } else {
                x = Mathf.Round(edgeOfPlayer) - 1;
            }

            //if (Mathf.Abs(edgeOfPlayer - x) > 0.5f) {
            //    return;
            //}

            Vector2 pointToCheck = new Vector2(x, Mathf.RoundToInt(transform.position.y));
            Collider2D other = Physics2D.OverlapPoint(pointToCheck);
            if (other == null) {
                carriedBox.transform.SetParent(null);
                carriedBox.transform.position = new Vector3(pointToCheck.x, pointToCheck.y, carriedBox.transform.position.z);
                setBoxTowerEnabledValue(carriedBox.transform, true);
                carriedBox = null;
            }
        }
    }

    private void setBoxTowerEnabledValue(Transform box, bool enabled) {
        while (box != null) {
            box.GetComponent<BoxMovement>().enabled = enabled;
            if (box.childCount > 0) {
                box = box.GetChild(0);
            } else {
                box = null;
            }
        }
    }

    private bool canPickupBoxes(Transform box) {
        int count = 0;
        while (box != null) {
            if (++count > boxTowerLimit) {
                return false;
            }
            if (box.childCount > 0) {
                box = box.GetChild(0);
            }
            else {
                box = null;
            }
        }
        return true;
    }
}
