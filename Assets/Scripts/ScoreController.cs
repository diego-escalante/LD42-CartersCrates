using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour {

    public Text scoreText;
    public Text scoreDeltaText;
    public Text finalScoreText;
    public InputField inputField;
    public GameObject highScores;
    private int score = 0;
    private int scoreDelta = 0;
    private int scoreStreak = 1;
    private float scoreResetTime = 2;
    private float scoreCurrentTime = 0f;
    private HighScoreManager highScoreManager;
    public GameObject background;

    private bool gameOver = false;

    void Start() {
        highScoreManager = GetComponent<HighScoreManager>();
    }


    void OnEnable () {
        EventManager.StartListening("Score", boxScored);
        EventManager.StartListening("Start Game", startGame);
        EventManager.StartListening("Game Over", endGame);
	}

	void OnDisable () {
        EventManager.StopListening("Score", boxScored);
        EventManager.StopListening("Start Game", startGame);
        EventManager.StopListening("Game Over", endGame);
    }

    private void endGame() {
        if (gameOver) {
            return;
        }
        gameOver = true;
        scoreText.gameObject.SetActive(false);
        scoreDeltaText.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(true);
        background.SetActive(true);

        if (highScoreManager.IsHighScore(score)) {
            finalScoreText.text = "<color=yellow>Game Over!\n Final Score: " + score + "\n<size=32>Your score made it to the global leaderboards!\nSubmit your name in the field below and press Enter.\nYou can opt out and skip by leaving the field blank.</size></color>";
            inputField.gameObject.SetActive(true);
            inputField.Select();
            return;
        }

        finalScoreText.text = "Game Over!\nFinal Score: " + score + "\nPress 'R' to restart.";
    }

    private void startGame() {
        scoreText.gameObject.SetActive(true);
        scoreDeltaText.gameObject.SetActive(true);
    }

    public void Update() {
        if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.D)) {
            EventManager.TriggerEvent("Game Over");
        }

        if (!inputField.gameObject.activeSelf && Input.GetKeyDown(KeyCode.R)) {
            restart();
        }

        if (scoreResetTime <= Time.time - scoreCurrentTime) {
            scoreDeltaText.text = "";
        }

        if (highScores.activeSelf && Input.GetKeyDown(KeyCode.Return)) {
            restart();
        }

        if (inputField.gameObject.activeSelf) {
            if (Input.GetKeyDown(KeyCode.Return) && !highScores.activeSelf) {
                string name = inputField.transform.Find("Text").GetComponent<Text>().text;
                if (name != "") {
                    highScoreManager.SubmitHighScore(name, score);
                    highScores.SetActive(true);
                    finalScoreText.gameObject.SetActive(false);
                    inputField.gameObject.SetActive(false);
                } else {
                    restart();
                }
            } 
        }

        
    }

    private void restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void boxScored(){
        if (gameOver) {
            return;
        }
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
