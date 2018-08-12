using UnityEngine.UI;
using UnityEngine;

public class StartScript : MonoBehaviour {

    public GameObject StartText; 

	void Update () {
        if (Input.GetKeyDown(KeyCode.Return)) {
            EventManager.TriggerEvent("Start Game");
            Destroy(StartText);
            Destroy(this);
        }
	}
}
