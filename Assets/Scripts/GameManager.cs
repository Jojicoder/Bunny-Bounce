using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text gameOverText;
    public GameObject replayButton;
    public GameObject menuButton;

    [Header("Time Limit")]
    public float timeLimit = 60f;

    [Header("BGM")]
    public AudioSource bgmSource;

    [Header("Game Sounds")]
    public AudioClip gameOverSound;
    public AudioClip timeUpSound;

    [Range(0f, 1f)]
    public float soundVolume = 0.5f;

    [Header("Button Sound")]
    public AudioSource uiAudioSource;
    public AudioClip clickSound;

    [Range(0f, 1f)]
    public float clickVolume = 1f;

    public float sceneChangeDelay = 0.2f;

    private int score;
    private float remainingTime;
    private bool isGameOver;
    private bool isChangingScene;

    private void Start()
    {
        Time.timeScale = 1f;

        score = 0;
        remainingTime = timeLimit;
        isGameOver = false;
        isChangingScene = false;

        UpdateScoreText();
        UpdateTimerText();

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        if (replayButton != null)
        {
            replayButton.SetActive(false);
        }

        if (menuButton != null)
        {
            menuButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;

            UpdateTimerText();
            GameOver("TIME UP!");
            return;
        }

        UpdateTimerText();
    }

    public void AddScore(int amount)
    {
        if (isGameOver)
        {
            return;
        }

        score += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text =
                "Time: " + Mathf.CeilToInt(remainingTime);
        }
    }

    public void GameOver(string message)
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        if (bgmSource != null)
        {
            bgmSource.Stop();
        }

        if (message == "TIME UP!")
        {
            if (timeUpSound != null && Camera.main != null)
            {
                AudioSource.PlayClipAtPoint(
                    timeUpSound,
                    Camera.main.transform.position,
                    soundVolume
                );
            }

            if (gameOverText != null)
            {
                gameOverText.text =
                    "<color=#FFD700>TIME UP!</color>\n\n" +
                    "<color=white>Score: " + score + "</color>";
            }
        }
        else
        {
            if (gameOverSound != null && Camera.main != null)
            {
                AudioSource.PlayClipAtPoint(
                    gameOverSound,
                    Camera.main.transform.position,
                    soundVolume
                );
            }

            if (gameOverText != null)
            {
                gameOverText.text =
                    "<color=#FF3333>GAME OVER!!</color>\n\n" +
                    "<color=white>Score: " + score + "</color>";
            }
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
        }

        if (replayButton != null)
        {
            replayButton.SetActive(true);
        }

        if (menuButton != null)
        {
            menuButton.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void ReplayGame()
    {
        if (isChangingScene)
        {
            return;
        }

        StartCoroutine(ReplayGameCoroutine());
    }

    private IEnumerator ReplayGameCoroutine()
    {
        isChangingScene = true;

        PlayClickSound();

        yield return new WaitForSecondsRealtime(sceneChangeDelay);

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ReturnToMenu()
    {
        if (isChangingScene)
        {
            return;
        }

        StartCoroutine(ReturnToMenuCoroutine());
    }

    private IEnumerator ReturnToMenuCoroutine()
    {
        isChangingScene = true;

        PlayClickSound();

        yield return new WaitForSecondsRealtime(sceneChangeDelay);

        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    private void PlayClickSound()
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(
                clickSound,
                clickVolume
            );
        }
    }
}