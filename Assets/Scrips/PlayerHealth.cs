using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : LivingEntity
{
    public AudioClip deathSound;
    public AudioClip hitSound;        // 피격 효과음
    public Image damageEffect;        // 빨간 이미지 슬롯

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // ← Awake에서 초기화!
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        startingHealth = 100f;
       
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
        UIManager.Instance.UpdateHealth(health, startingHealth);

        // 피격 효과음
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        // 빨간 이펙트
        if (damageEffect != null)
            StartCoroutine(FlashDamage());
    }

    private IEnumerator FlashDamage()
    {
        // 빨갛게
        damageEffect.color = new Color(1f, 0f, 0f, 0.4f);

        // 서서히 사라짐
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0.4f, 0f, elapsed / duration);
            damageEffect.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        damageEffect.color = new Color(1f, 0f, 0f, 0f);
    }

    public override void Die()
    {
        base.Die();

        var animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Die");

        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerInput>().enabled = false;
        GetComponent<PlayerShooter>().enabled = false;

        var rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        UIManager.Instance.ShowGameOver();
    }
}