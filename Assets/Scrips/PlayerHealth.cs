using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : LivingEntity
{
    public GameObject gameOverUI;  // 게임 오버 UI 패널
    public AudioClip deathSound;
    private AudioSource audioSource;
    protected override void OnEnable()
    {
        base.OnEnable();
        startingHealth = 100f;
        audioSource = GetComponent<AudioSource>();
    }

    public override void Die()
    {
        base.Die();
        var animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Die");

        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);

        // 게임 오버 UI 표시
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // 플레이어 조작 비활성화
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerInput>().enabled = false;

        Time.timeScale = 0f; // 게임 일시정지
    }
}