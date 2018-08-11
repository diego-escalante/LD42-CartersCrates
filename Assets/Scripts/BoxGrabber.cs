using UnityEngine;

public class BoxGrabber : MonoBehaviour {

    public int boxTowerLimit = 3;
    private PlayerMovement playerMovement;
    private GameObject carriedBox = null;

    void Start () {
        playerMovement = GetComponent<PlayerMovement>();
    }
	
	void Update () {
		if (Input.GetButtonDown("Action")) {
            bool isFacingRight = playerMovement.getFacingRight();
            Vector2 pointToCheck = new Vector2(isFacingRight ? Mathf.CeilToInt(transform.position.x + 1) : Mathf.FloorToInt(transform.position.x - 1), 
                                               Mathf.RoundToInt(transform.position.y));
            Collider2D coll = Physics2D.OverlapPoint(pointToCheck);
            if (carriedBox != null) {
                if (coll == null) {
                    carriedBox.transform.SetParent(null);
                    carriedBox.transform.position = new Vector3(pointToCheck.x, pointToCheck.y, carriedBox.transform.position.z);
                    setBoxTowerEnabledValue(carriedBox.transform, true);
                    carriedBox = null;
                }
            } else {
                if (coll != null && coll.tag == "Box" && canPickupBoxes(coll.transform)) {
                    carriedBox = coll.gameObject;
                    carriedBox.transform.SetParent(this.transform);
                    carriedBox.transform.localPosition = new Vector3(0, 1, 0);
                    setBoxTowerEnabledValue(carriedBox.transform, false);
                }
            }
        }
	}

    private void setBoxTowerEnabledValue(Transform box, bool enabled) {
        while (box != null) {
            box.GetComponent<BoxMovement>().enabled = enabled;
            //box.GetComponent<BoxCollider2D>().enabled = enabled;
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
