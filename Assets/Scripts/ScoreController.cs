using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    public Text scoreText;
    public Text scoreDeltaText;
    private int score = 0;
    private int scoreDelta = 0;
    private int scoreStreak = 1;
    private float scoreResetTime = 2;
    private float scoreCurrentTime = 0f;


    void OnEnable () {
        EventManager.StartListening("Score", boxScored);
        EventManager.StartListening("Start Game", startGame);
	}

	void OnDisable () {
        EventManager.StopListening("Score", boxScored);
        EventManager.StopListening("Start Game", startGame);
    }

    private void startGame() {
        scoreText.gameObject.SetActive(true);
        scoreDeltaText.gameObject.SetActive(true);
    }

    public void Update() {
        if (scoreResetTime <= Time.time - scoreCurrentTime) {
            scoreDeltaText.text = "";
        }
    }

    public void boxScored(){
        if (scoreResetTime > Time.time - scoreCurrentTime) {
            scoreStreak++;
        } else {
            scoreStreak = 1;
            scoreDelta = 0;
        }
        scoreCurrentTime = Time.time;

        scoreDelta += (int)Mathf.Pow(scoreStreak, 2);
        score += (int)Mathf.Pow(scoreStreak, 2);

        scoreText.text = "Score: " + score;
        scoreDeltaText.text = "+" + scoreDelta;
    }
}
