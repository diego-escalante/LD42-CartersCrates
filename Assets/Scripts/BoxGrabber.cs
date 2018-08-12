using UnityEngine;

public class BoxGrabber : MonoBehaviour {

    public Sprite normalSprite;
    public Sprite heavySprite;

    public int boxTowerLimit = 3;
    private PlayerMovement playerMovement;
    private GameObject carriedBox = null;
    private Collider2D coll = null;
    private LayerMask boxMask = new LayerMask();
    private const float MARGIN = 0.01f;

    private Transform crosshairs;
    private SpriteRenderer crosshairsRenderer;


    void Start () {
        playerMovement = GetComponent<PlayerMovement>();
        coll = GetComponent<Collider2D>();
        boxMask = LayerMask.GetMask("Boxes");
        crosshairs = GameObject.FindWithTag("Crosshairs").transform;
        crosshairsRenderer = crosshairs.GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        doAction();
	}

    private void doAction() {
        bool isFacingRight = playerMovement.getFacingRight();
        float reach = 1f;

     
        if (carriedBox == null)
        {
            //If pressing down, pick up box from underneath.
            if (Input.GetButton("Down")) {
                Vector2 pointToCheckBelow = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y - 1));
                Collider2D otherBelow = Physics2D.OverlapPoint(pointToCheckBelow);
                if(otherBelow != null && otherBelow.tag == "Box") {
                    if (Input.GetButtonDown("Action")) {
                        carriedBox = otherBelow.gameObject;
                        carriedBox.transform.SetParent(this.transform);
                        carriedBox.transform.localPosition = new Vector3(0, 1, 0);
                        setBoxTowerEnabledValue(carriedBox.transform, false);
                    }
                    crosshairsRenderer.enabled = true;
                    crosshairsRenderer.sprite = normalSprite;
                    crosshairs.position = pointToCheckBelow;
                } else {
                    crosshairsRenderer.enabled = false;
                }
            } else {
                // Picking up a box.
                Vector2 origin = new Vector2(transform.position.x + (coll.bounds.extents.x * (isFacingRight ? 1 : -1)), transform.position.y);
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * (isFacingRight ? 1 : -1), reach, boxMask);
                float x;
                float edgeOfPlayer = transform.position.x + ((coll.bounds.extents.x - MARGIN) * (playerMovement.getFacingRight() ? 1 : -1));
                if (playerMovement.getFacingRight()) {
                    x = Mathf.Round(edgeOfPlayer) + 1;
                } else {
                    x = Mathf.Round(edgeOfPlayer) - 1;
                }
                Vector3 pointToCheckSide = new Vector3(x, Mathf.RoundToInt(transform.position.y), -5);

                if (hit.collider != null && canPickupBoxes(hit.collider.transform)) {
                    if (Input.GetButtonDown("Action")) {
                        carriedBox = hit.collider.gameObject;
                        carriedBox.transform.SetParent(this.transform);
                        carriedBox.transform.localPosition = new Vector3(0, 1, 0);
                        setBoxTowerEnabledValue(carriedBox.transform, false);
                        EventManager.TriggerEvent("Pick Up");
                    }
                    
                    crosshairsRenderer.enabled = true;
                    crosshairs.position = pointToCheckSide;
                    crosshairsRenderer.sprite = normalSprite;
                } else if (hit.collider != null && !canPickupBoxes(hit.collider.transform)) {
                    crosshairsRenderer.enabled = true;
                    crosshairs.position = pointToCheckSide;
                    crosshairsRenderer.sprite = heavySprite;
                    if (Input.GetButtonDown("Action")) {
                        EventManager.TriggerEvent("Error");
                    }
                } else {
                    crosshairsRenderer.enabled = false;
                }
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

            Vector2 pointToCheck = new Vector2(x, Mathf.RoundToInt(transform.position.y));
            Collider2D other = Physics2D.OverlapPoint(pointToCheck);
            if (other == null) {
                if (Input.GetButtonDown("Action")) {
                    EventManager.TriggerEvent("Drop");
                    carriedBox.transform.SetParent(null);
                    carriedBox.transform.position = new Vector3(pointToCheck.x, pointToCheck.y, carriedBox.transform.position.z);
                    setBoxTowerEnabledValue(carriedBox.transform, true);
                    carriedBox = null;
                }
                crosshairsRenderer.enabled = true;
                crosshairsRenderer.sprite = normalSprite;
                crosshairs.position = pointToCheck;
            } else {
                crosshairsRenderer.enabled = false;
            }
        }
    }

    private void setBoxTowerEnabledValue(Transform box, bool enabled) {
        while (box != null) {
            box.GetComponent<BoxMovement>().enabled = enabled;
            box.GetComponent<BoxCollider2D>().size = enabled ? Vector2.one : new Vector2(0.725f, 1);
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

    public bool isCarrying() {
        return carriedBox != null;
    }
}
