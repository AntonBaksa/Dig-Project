using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int scorePlayer1 = 0;
    public static int scorePlayer2 = 0;
    public static int winningScore = 3;

    public TMP_Text scoreText;

    void Start()
    {
        UpdateScoreText();
    }

    public void AwardPoint(int playerID)
    {
        if (playerID == 1)
            scorePlayer2++;
        else
            scorePlayer1++;

        UpdateScoreText();
        CheckForWinner();
    }

    private void CheckForWinner()
    {
        if (scorePlayer1 >= winningScore)
        {
            Debug.Log("Player 1 Wins!");
            ResetGame();
        }
        else if (scorePlayer2 >= winningScore)
        {
            Debug.Log("Player 2 Wins!");
            ResetGame();
        }
    }

    private void ResetGame()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Player 1: " + scorePlayer1 + " - Player 2: " + scorePlayer2;
        }
    }
}
