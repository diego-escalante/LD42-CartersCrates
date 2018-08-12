using UnityEngine.UI;
using UnityEngine;

public class StartScript : MonoBehaviour {

    public GameObject StartText;
    public GameObject HighScores;
    public GameObject background;

	void Update () {
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (StartText.activeSelf) {
                EventManager.TriggerEvent("Start Game");
                Destroy(StartText);
                background.SetActive(false);
                Destroy(this);
            } else {
                HighScores.SetActive(false);
                StartText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            StartText.SetActive(false);
            HighScores.SetActive(true);
        }
	}
}
