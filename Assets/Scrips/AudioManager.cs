using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;   // 배경음 (BGM)
    public AudioSource effectsSource; // 효과음 (SFX)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // 배경음 볼륨 조절 (0.0 ~ 1.0)
    public void SetMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
    }

    // 효과음 볼륨 조절 (0.0 ~ 1.0)
    public void SetEffectsVolume(float value)
    {
        if (effectsSource != null)
            effectsSource.volume = value;
    }

    // 전체 음소거 Toggle
    public void SetSoundOnOff(bool isOn)
    {
        // 전체 소리를 끄고 켜는 가장 확실한 방법
        AudioListener.pause = !isOn;
    }
}