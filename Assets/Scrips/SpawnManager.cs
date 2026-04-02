using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    public GameObject[] enemyPrefabs;     // ZomBear, Zombunny
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public int minSpawnCount = 1;
    public int maxSpawnCount = 2;

   
    public GameObject hellephantPrefab;   // Hellephant 프리팹
    public int killsToSpawnBoss = 5;     // 몇 마리 잡으면 보스 등장

    private float nextSpawnTime;
    public static int totalKills = 0;     // 총 처치 수
    private bool bossSpawned = false;

    private void Update()
    {
        // 보스 스폰 체크
        if (!bossSpawned && totalKills >= killsToSpawnBoss)
        {
            SpawnBoss();
            bossSpawned = true;
        }

        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnInterval;
            Spawn();
        }
    }

    private void Spawn()
    {
        int count = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < count; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject spawnedEnemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);

            EnemyAI enemyAI = spawnedEnemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
                enemyAI.target = GameObject.FindWithTag("Player").transform;
        }
    }

    private void SpawnBoss()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        
        GameObject boss = Instantiate(hellephantPrefab, spawnPoint.position, spawnPoint.rotation);

        EnemyAI enemyAI = boss.GetComponent<EnemyAI>();
        if (enemyAI != null)
            enemyAI.target = GameObject.FindWithTag("Player").transform;
    }
}