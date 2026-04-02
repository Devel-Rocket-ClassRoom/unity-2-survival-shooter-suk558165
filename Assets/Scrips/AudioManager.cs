using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource effectsSource; // 효과음 재생 전용 소스

    [Header("SFX Clips Array")]
    // 인스펙터에서 효과음 파일들을 배열로 넣으세요.
    public AudioClip[] sfxClips;

    // 이름으로 빨리 찾기 위한 딕셔너리
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitDictionary();
        }
        else Destroy(gameObject);
    }

    private void InitDictionary()
    {
        foreach (AudioClip clip in sfxClips)
        {
            if (clip != null && !sfxDictionary.ContainsKey(clip.name))
                sfxDictionary.Add(clip.name, clip);
        }
    }

    // 배경음 볼륨 조절
    public void SetMusicVolume(float value)
    {
        if (musicSource != null) musicSource.volume = value;
    }

    // 효과음 볼륨 조절 (이 값이 바뀌면 PlayOneShot으로 재생되는 모든 소리에 적용됨)
    public void SetEffectsVolume(float value)
    {
        if (effectsSource != null) effectsSource.volume = value;
    }

    public void SetSoundOnOff(bool isOn)
    {
        AudioListener.pause = !isOn;
    }

    // 효과음 재생 함수 (이름으로 재생)
    public void PlaySFX(string clipName)
    {
        if (sfxDictionary.ContainsKey(clipName))
        {
            effectsSource.PlayOneShot(sfxDictionary[clipName]);
        }
        else
        {
            Debug.LogWarning($"효과음 {clipName}을 찾을 수 없습니다.");
        }
    }
}