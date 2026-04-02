using UnityEngine;
using System.Collections;
public class Gun : MonoBehaviour
{
    public float fireRate = 0.1f;
    public float damage = 10f;
    public float range = 100f;
    public Transform firePoint;
    public float lineDisplayTime = 0.05f; // 궤적 표시 시간
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
        lineRenderer.enabled = false; 
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (playerInput.Fire && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (firesound != null)
            audioSource.PlayOneShot(firesound);

        Vector3 shootDirection = transform.root.forward;  
        Ray ray = new Ray(firePoint.position, shootDirection);
        Vector3 endPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("맞음: " + hit.collider.name);
            endPoint = hit.point;

            var enemy = hit.collider.GetComponentInParent<EnemyAI>();
            if (enemy != null)
                enemy.TakeDamage(damage, hit.point, hit.normal);
        }
        else
        {
            endPoint = firePoint.position + shootDirection * range;  
        }

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