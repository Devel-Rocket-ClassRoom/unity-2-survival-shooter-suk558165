using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float traceDistance = 15f;
    public float attackDistance = 1.5f;
    public float attackInterval = 0.5f;
    public AmenyData data;
    public float collisionDamage = 10f;
    public float collisionInterval = 1f;
    public ParticleSystem hitParticle;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public float maxHealth = 100f;

    private float currentHealth;
    private NavMeshAgent agent;
    private Animator animator;
    private float lastCollisionTime;
    private float lastAttackTime;
    private enum Status { Idle, Trace, Attack, Die }
    private Status currentStatus;
    private bool isDead;
    private AudioSource audioSource;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentStatus = Status.Idle;
    }

    private void Update()
    {
        if (isDead) return;

        switch (currentStatus)
        {
            case Status.Idle: UpdateIdle(); break;
            case Status.Trace: UpdateTrace(); break;
            case Status.Attack: UpdateAttack(); break;
        }
    }

    private void UpdateIdle()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= traceDistance)
        {
            currentStatus = Status.Trace;
            SetAnimatorBool("HasTarget", true);
            agent.isStopped = false;
        }
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (isDead) return;

        // 체력 감소
        currentHealth -= damage;

        // 피격 파티클
        if (hitParticle != null)
        {
            hitParticle.transform.position = hitPoint;
            hitParticle.transform.forward = hitNormal;
            hitParticle.Play();
        }

        // 피격 효과음 (수정: AudioManager의 effectsSource 사용)
        if (hitSound != null && AudioManager.Instance != null && AudioManager.Instance.effectsSource != null)
            AudioManager.Instance.effectsSource.PlayOneShot(hitSound);

        // 체력 0 이하면 사망
        if (currentHealth <= 0)
            Die();
    }

    private void UpdateTrace()
    {
        if (target == null)
        {
            currentStatus = Status.Idle;
            SetAnimatorBool("HasTarget", false);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackDistance)
        {
            currentStatus = Status.Attack;
            agent.isStopped = true;
            SetAnimatorBool("HasTarget", false);
            return;
        }

        if (distance > traceDistance)
        {
            currentStatus = Status.Idle;
            agent.isStopped = true;
            SetAnimatorBool("HasTarget", false);
            return;
        }

        agent.SetDestination(target.position);
    }

    private void UpdateAttack()
    {
        if (target == null)
        {
            currentStatus = Status.Idle;
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            currentStatus = Status.Trace;
            agent.isStopped = false;
            SetAnimatorBool("HasTarget", true);
            return;
        }

        Vector3 lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        if (Time.time > lastAttackTime + attackInterval)
        {
            lastAttackTime = Time.time;
            var livingEntity = target.GetComponent<LivingEntity>();
            if (livingEntity != null && !livingEntity.Isdead)
                livingEntity.OnDamage(10f, transform.position, -transform.forward);
        }
    }

    // 충돌 데미지
    private void OnCollisionStay(Collision collision)
    {
        if (Time.time < lastCollisionTime + collisionInterval) return;

        var player = collision.gameObject.GetComponent<PlayerHealth>();
        if (player != null && !player.Isdead)
        {
            lastCollisionTime = Time.time;
            player.OnDamage(collisionDamage,
                collision.contacts[0].point,
                collision.contacts[0].normal);
        }
    }

    private void SetAnimatorBool(string name, bool value)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetBool(name, value);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetTrigger("Die");

        // 사망 효과음 (수정: AudioManager의 effectsSource 사용)
        if (deathSound != null && AudioManager.Instance != null && AudioManager.Instance.effectsSource != null)
            AudioManager.Instance.effectsSource.PlayOneShot(deathSound);

        agent.isStopped = true;
        agent.enabled = false; // ← NavMesh 오류 방지
        SpawnManager.totalKills++; // SpawnManager 변수에 맞춰 수정

    }

    public void StartSinking()
    {
        // 콜라이더 끄기 (플레이어와 충돌 방지)
        GetComponent<Collider>().enabled = false;
        isSinking = true;
    }

    private bool isSinking = false;
    public float sinkSpeed = 1f;

    private void LateUpdate()
    {
        if (isSinking)
        {
            transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime);
            Destroy(gameObject, 2f);
        }
    }
}