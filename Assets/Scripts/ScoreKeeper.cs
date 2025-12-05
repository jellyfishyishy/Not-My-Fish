using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Net;

public class ScoreKeeper : MonoBehaviour
{
    public GameObject GameOverPanel;
    public GameObject Button;
    public int MaxWeight;
    private static float score;  // everyone has the same score
    private static Text scoreText; // everyone has the same text
    public AudioSource applause;
    Vector2 pos;

    // Use this for initialization
    internal void Start()
    {
        score = MaxWeight;
        applause = GetComponent<AudioSource>();
        GameOverPanel.SetActive(false);
        Button.SetActive(false);
        pos = transform.position;
        scoreText = GetComponent<Text>();
        UpdateText();
    }

    public static void AddToScore(float points)
    {
        score -= points;
        UpdateText();
    }

    private static void UpdateText()
    {
        scoreText.text = String.Format("Weight Remaining: {0} lbs", score);
        // scoreText.alignment = TextAnchor.UpperRight;
    }

    private void Update()
    {
        if (score <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (score == 0)
        {
            applause.PlayOneShot(applause.clip);
        }

        GameOverPanel.SetActive(true);
        Button.SetActive(true);
        scoreText.text = String.Format("Game Over\nScore of: {0}", score);
        scoreText.alignment = TextAnchor.MiddleCenter;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
