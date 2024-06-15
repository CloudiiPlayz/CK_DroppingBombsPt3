using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject title;
    private Spawner spawner;
    private Vector2 screenBounds;
    public GameObject splash;

    [Header("Player")]
    public GameObject playerPrefab;
    private GameObject player;
    private bool gameStarted = false;

    [Header("Score")]
    public TMP_Text scoreText;
    public int pointsWorth = 1;
    private int score;

    private int bestScore = 0;
    public TMP_Text bestScoreText;
    private bool beatBestScore;

    public Color normalColor;
    public Color bestScoreColor;

    private bool smokeCleared = true;
    // Start is called before the first frame update

    private void Awake()
    {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        scoreText.enabled = false;

        bestScoreText.enabled = false;
    }


    void Start()
    {
        spawner.active = false;
        title.SetActive(true); 
        splash.SetActive(false);

        bestScore = PlayerPrefs.GetInt("BestScore");
        bestScoreText.text = "Best Score: " + bestScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
        {
            if (Input.anyKeyDown && smokeCleared)
            {
                smokeCleared = false;
                ResetGame();
            }
        }
        else
        {
            if (!player)
            {
                OnPlayerKilled();
            }
        }

        

        var nextBomb = GameObject.FindGameObjectsWithTag("Bomb");

        foreach (GameObject bombObject in nextBomb)
        {
            if (bombObject.transform.position.y < (-screenBounds.y - 12))
            {
                if (gameStarted)
                {
                    score += pointsWorth;
                    scoreText.text = "Score: " + score.ToString();
                }
                    
                    Destroy(bombObject);
            }
            
        }
    }

    void OnPlayerKilled()
    {
        spawner.active = false;
        gameStarted = false;

        Invoke("SplashScreen", 2);

        if (score > bestScore)
        {
            bestScoreText.color = bestScoreColor;

            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            beatBestScore = true;
            bestScoreText.text = "Best Score: " + bestScore.ToString();
        }
    }

    void ResetGame()
    {
        bestScoreText.color = bestScoreColor;

        spawner.active = true;
        title.SetActive(false);
        splash.SetActive(false);

        scoreText.enabled = true;
        score = 0;

        beatBestScore = false;
        bestScoreText.enabled = true;

        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), playerPrefab.transform.rotation);
        gameStarted = true;
    }

    void SplashScreen()
    {
        smokeCleared = true;
        splash.SetActive(true);
    }
}
