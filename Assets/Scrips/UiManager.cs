using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    // --- 싱글톤 설정 ---
    private static UIManager instance;
    public static UIManager Instance => instance;

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;

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
        // 싱글톤 초기화
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        UpdateScore(0);

        // 이벤트 리스너 연결
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        if (effectsSlider != null) effectsSlider.onValueChanged.AddListener(OnEffectsSliderChanged);
        if (soundToggle != null) soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);

        if (resumeButton != null) resumeButton.onClick.AddListener(Resume);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);

        // 초기 볼륨 설정 반영
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

    // 점수 업데이트
    public void UpdateScore(int score)
    {
        if (scoreText != null) scoreText.text = $"SCORE: {score}";
    }

    // 일시정지
    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        // 마우스 커서 활성화 (메뉴 클릭용)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 플레이어 조작 차단 (총 발사 및 회전 멈춤)
        SetPlayerControl(false);
    }

    // 재개
    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        // 마우스 커서 다시 숨기기 (게임 플레이용)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 플레이어 조작 다시 활성화
        SetPlayerControl(true);
    }

    // 플레이어 스크립트 제어 함수
    private void SetPlayerControl(bool state)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // 1. 입력 스크립트 차단 (클릭 시 총 발사 버그 수정)
            var input = player.GetComponent<PlayerInput>();
            if (input != null) input.enabled = state;

            // 2. 이동 및 회전 스크립트 차단 (마우스 회전 멈춤)
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

    internal void UpdateHealth(float health, float startingHealth)
    {
        throw new NotImplementedException();
    }
}