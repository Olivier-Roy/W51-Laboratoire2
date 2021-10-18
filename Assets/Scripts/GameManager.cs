using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int actualLevel = 0;
    private int score = 0;
    private string playerName;
    private int lives = 3;

    private bool textsNotLinked = true;

    Text playerNameText;
    Text playerScoreText;
    Text playerLivesText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        if (textsNotLinked)
        {
            textsNotLinked = false;
            if (actualLevel == 0) return;  //pas utilise sur l'écran de titre

            playerNameText = GameObject.FindGameObjectWithTag("TextName").GetComponent<Text>();
            playerNameText.text = playerName;

            playerLivesText = GameObject.FindGameObjectWithTag("TextLives").GetComponent<Text>();
            playerLivesText.text = lives.ToString();


            playerScoreText = GameObject.FindGameObjectWithTag("TextScore").GetComponent<Text>();
            playerScoreText.text = score.ToString();
        }
    }

    public void RestartLevel(float delay)
    {
        StartCoroutine(RestartLevelDelay(delay, actualLevel));
    }


    public void StartNextlevel(float delay)
    {
        StartCoroutine(RestartLevelDelay(delay, GetNextLevel()));
    }

    private IEnumerator RestartLevelDelay(float delay, int level)
    {

        yield return new WaitForSeconds(delay);
        textsNotLinked = true;

        if (lives == 0)
            SceneManager.LoadScene("SceneGameOver");
        else if (level == 2)
            SceneManager.LoadScene("Scene2");
        else if (level == 3)
            SceneManager.LoadScene("Scene3");
        else
            SceneManager.LoadScene("Scene1");
    }

    public void ResetGame()
    {
        lives = 3;
        actualLevel = 0;
        score = 0;
        SceneManager.LoadScene("Scene0");
    }

    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    private int GetNextLevel()
    {
        actualLevel++;
        if (actualLevel == 4)
            actualLevel = 1;

        return actualLevel;
    }

    //---------------------------------------------------------------
    //Role "traditionnel" du Game Manager: petit donc on le garde ici
    //---------------------------------------------------------------
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        playerScoreText.text = score.ToString();
    }

    public void PlayerDie()
    {
        lives--;
        playerLivesText.text = lives.ToString();
    }

}