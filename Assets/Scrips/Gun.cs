using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float fireRate = 0.1f;
    public float damage = 10f;
    public float range = 100f;
    public Transform firePoint; // 할당하신 파이어포인트
    public float lineDisplayTime = 0.05f;
    public ParticleSystem muzzleFlash;
    public AudioClip firesound;

    private float nextFireTime = 0f;
    private PlayerInput playerInput;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        // 궤적이 캐릭터를 따라 휘지 않도록 월드 좌표계 사용
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if (playerInput != null && playerInput.Fire && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (muzzleFlash != null) muzzleFlash.Play();

        if (firesound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.effectsSource.PlayOneShot(firesound);
        }

        // --- 수정된 로직: firePoint 기준 발사 ---

        // 1. 레이의 시작점은 firePoint, 방향은 총구가 바라보는 앞방향(forward)
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hit;
        Vector3 endPoint;

        // 2. 레이캐스트 실행 (플레이어 자신은 무시하도록 레이어 설정 추천)
        if (Physics.Raycast(ray, out hit, range))
        {
            endPoint = hit.point;

            var enemy = hit.collider.GetComponentInParent<EnemyAI>();
            if (enemy != null)
                enemy.TakeDamage(damage, hit.point, hit.normal);
        }
        else
        {
            // 맞은 게 없으면 총구 정면으로 range만큼 떨어진 지점이 끝점
            endPoint = firePoint.position + firePoint.forward * range;
        }

        // 3. 궤적 그리기
        StopAllCoroutines();
        StartCoroutine(ShowLine(firePoint.position, endPoint));
    }

    private IEnumerator ShowLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(lineDisplayTime);

        lineRenderer.enabled = false;
    }
}