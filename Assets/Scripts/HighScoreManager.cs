using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{

    private const string privateCode = "PASTE YOUR CODE HERE";
    private const string publicCode = "5b707e04191a8b0bccbf5a23";
    private const string webURL = "http://dreamlo.com/lb/";

    public GameObject userGO;
    public GameObject scoreGO;

    private string nameList;
    private string scoreList;
    private int lowestScore;
    private bool loaded;

    public void Start()
    {
        downloadHighScores();
    }

    public bool IsHighScore(int score)
    {
        if (!loaded)
        {
            return false;
        }
        return score > lowestScore;
    }

    public void SubmitHighScore(string name, int score)
    {
        StartCoroutine(UploadNewHighScore(name, score));
    }

    private IEnumerator UploadNewHighScore(string name, int score)
    {
        Debug.Log("Uploading high score...");
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(name) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            downloadHighScores();
        }
        Debug.Log("Uploading high score!");
    }

    // Use this for initialization
    private void downloadHighScores()
    {
        StartCoroutine("DownloadHighScoresFromDatabase");
    }

    private IEnumerator DownloadHighScoresFromDatabase()
    {
        Debug.Log("Getting scores!");

        WWW www = new WWW(webURL + publicCode + "/pipe/20");
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Failed to get scores. Retrying in the background.");
            Invoke("downloadHighScores", 30);
            yield break;
        }

        Highscore[] scores = FormatHighscores(www.text);

        nameList = "";
        scoreList = "";

        foreach (Highscore score in scores)
        {
            nameList += score.username + "\n";
            scoreList += score.score + "\n";
        }

        for (int i = 0; i < 20 - scores.Length; i++)
        {
            nameList += "???\n";
            scoreList += "0\n";
        }

        userGO.GetComponent<Text>().text = nameList;
        scoreGO.GetComponent<Text>().text = scoreList;
        loaded = true;
        Debug.Log("Got scores!");
    }

    private Highscore[] FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        Highscore[] highscoresList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highscoresList[i] = new Highscore(username, score);
        }
        lowestScore = entries.Length < 20 ? 0 : highscoresList[entries.Length - 1].score;
        return highscoresList;
    }
}

public struct Highscore
{
    public string username;
    public int score;

    public Highscore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}