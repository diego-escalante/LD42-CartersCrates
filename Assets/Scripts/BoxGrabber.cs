using UnityEngine;

public class BoxGrabber : MonoBehaviour {

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
                    carriedBox.GetComponent<BoxMovement>().enabled = true;
                    carriedBox.GetComponent<BoxCollider2D>().enabled = true;
                    carriedBox = null;
                }
            } else {
                if (coll != null && coll.tag == "Box")
                {
                    carriedBox = coll.gameObject;
                    carriedBox.transform.SetParent(this.transform);
                    carriedBox.transform.localPosition = new Vector3(0, 1, 0);
                    carriedBox.GetComponent<BoxMovement>().enabled = false;
                    carriedBox.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
	}
}
