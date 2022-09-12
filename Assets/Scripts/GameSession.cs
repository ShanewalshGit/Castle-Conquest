using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3, score = 0;

    [SerializeField] Text scoreText, livesText;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void AddtoScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();

    }

    public void AddtoLives(int value)
    {
        playerLives += value;
        livesText.text = playerLives.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGame();
        }
    }

    public void TakeLife()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(4);
        Destroy(gameObject);
    }
}
