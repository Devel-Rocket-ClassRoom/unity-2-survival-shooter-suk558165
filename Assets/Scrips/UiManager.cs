using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class UiManager : MonoBehaviour
{
    private static UiManager instance;
    public static UiManager Instance => instance;

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;
    private int currentScore = 0; // 점수를 저장할 변수

    [Header("Health UI")]
    public Slider healthSlider;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    [Header("Pause UI Elements")]
    public GameObject pausePanel;
    public Slider musicSlider;
    public Slider effectsSlider;
    public Toggle soundToggle;
    public Button resumeButton;
    public Button quitButton;

    private bool isPaused = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        currentScore = 0;
        UpdateScore(currentScore);

        if (musicSlider != null) musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        if (effectsSlider != null) effectsSlider.onValueChanged.AddListener(OnEffectsSliderChanged);
        if (soundToggle != null) soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);

        if (resumeButton != null) resumeButton.onClick.AddListener(Resume);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);

        if (AudioManager.Instance != null)
        {
            if (musicSlider != null) musicSlider.value = AudioManager.Instance.musicSource.volume;
            if (effectsSlider != null) effectsSlider.value = AudioManager.Instance.effectsSource.volume;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    // 외부에서 점수를 더할 때 호출하는 함수
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScore(currentScore);
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null) scoreText.text = $"SCORE: {score}";
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SetPlayerControl(false);
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        // 마우스 잠금을 해제하여 일시정지 후 회전 고정 문제 해결
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SetPlayerControl(true);
    }

    private void SetPlayerControl(bool state)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var input = player.GetComponent<PlayerInput>();
            if (input != null) input.enabled = state;

            var movement = player.GetComponent<PlayerMovement>();
            if (movement != null) movement.enabled = state;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnMusicSliderChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
    }

    private void OnEffectsSliderChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetEffectsVolume(value);
    }

    private void OnSoundToggleChanged(bool isOn)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSoundOnOff(isOn);
    }

    public void UpdateHealth(float health, float startingHealth)
    {
        if (healthSlider != null)
            healthSlider.value = health / startingHealth;
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}