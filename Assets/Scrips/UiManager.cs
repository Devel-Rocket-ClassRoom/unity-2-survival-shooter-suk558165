using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // --- 싱글톤 설정 ---
    private static UIManager instance;
    public static UIManager Instance => instance;

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;

    [Header("Health UI")]
    public Slider healthSlider; // 플레이어 체력바 (추가됨)

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel; // 게임 오버 패널 (추가됨)

    [Header("Pause UI Elements")]
    public Slider musicSlider;
    public Slider effectsSlider;
    public Toggle soundToggle;
    public Button resumeButton;
    public Button quitButton;

    private bool isPaused = false;

    private void Awake()
    {
        // 싱글톤 초기화
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        UpdateScore(0);

        // 이벤트 리스너 연결
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        if (effectsSlider != null) effectsSlider.onValueChanged.AddListener(OnEffectsSliderChanged);
        if (soundToggle != null) soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);

        if (resumeButton != null) resumeButton.onClick.AddListener(Resume);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);

        // 초기 볼륨값 반영
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

    // --- PlayerHealth 및 SpawnManager에서 호출하는 함수들 ---

    public void UpdateScore(int score)
    {
        if (scoreText != null) scoreText.text = $"SCORE: {score}";
    }

    // 플레이어 체력 UI 업데이트 (추가됨)
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }

    // 게임 오버 UI 표시 (추가됨)
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame() // 재시작 버튼용 (필요 시 UI 버튼에 연결)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // --- 사운드 조절 로직 ---
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
}