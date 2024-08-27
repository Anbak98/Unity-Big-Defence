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

        // ������Ʈ�� �ٿ�带 ������
        Bounds bounds = targetRenderer.bounds;

        // �ٿ�� ������ ������ ��ġ ���
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        Vector3 randomPosition = new Vector3(randomX, randomY, 0);

        // ������Ʈ ǥ�� ���� ��ġ�� �����Ϸ���, ����ĳ��Ʈ �� �߰����� Ȯ���� �ʿ��� �� ����
        // �� ���������� �ܼ��� �ٿ�� ���� ���� ��ġ�� ����
        return randomPosition;
    }
}
