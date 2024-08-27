using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemyObjectPool enemyPool;
    [SerializeField] List<GameObject> enemySpawnPointObject;

    private readonly int spawnBatchSize = 10;
    private readonly float spawnInterval = 1f;
    private float spawnTimer;

    void Start()
    {
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemys();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnEnemys()
    {
        for (int i = 0; i < spawnBatchSize; i++)
        {
            foreach (GameObject obj in enemySpawnPointObject)
            {
                GameObject enemy = enemyPool.GetPooledObject(1);
                enemy.transform.position = GetRandomSpawnPositionOnObject(obj);
                enemy.SetActive(true);
            }
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Implement your logic to get a random spawn position
        return new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
    }

    public Vector3 GetRandomSpawnPositionOnObject(GameObject targetObject)
    {
        Renderer targetRenderer = targetObject.GetComponent<Renderer>();  

        if (targetRenderer == null)
        {
            Debug.LogError("Target Renderer is not assigned!");
            return Vector3.zero;
        }

        // 오브젝트의 바운드를 가져옴
        Bounds bounds = targetRenderer.bounds;

        // 바운드 내에서 랜덤한 위치 계산
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        Vector3 randomPosition = new Vector3(randomX, randomY, 0);

        // 오브젝트 표면 위의 위치를 보장하려면, 레이캐스트 등 추가적인 확인이 필요할 수 있음
        // 이 예제에서는 단순히 바운드 내의 랜덤 위치를 리턴
        return randomPosition;
    }
}
