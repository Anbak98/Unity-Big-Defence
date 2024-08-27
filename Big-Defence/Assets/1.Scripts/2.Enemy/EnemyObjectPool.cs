using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject enemyData;

    private int poolSize = 100;

    private List<List<GameObject>> pool;

    void Start()
    {
        pool = new List<List<GameObject>>();

        for (int i = 0; i < enemyData.PrefabList.Count - 1; i++)
        {
            pool.Add(new List<GameObject>());
            for(int j = 0; j < poolSize; j++)
            {
                GameObject obj = Instantiate(enemyData.PrefabList[i+1].Prefab);
                obj.name = $"{enemyData.PrefabList[i+1].Name}({j})";
                obj.SetActive(false);
                obj.transform.parent = transform;
                pool[i].Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(int enemyCode)
    {
        if(enemyCode >= enemyData.PrefabList.Count)
        {
            Debug.LogError("Enemy spawn index was out of range.");
        }
        foreach (GameObject obj in pool[enemyCode])
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        GameObject newObj = Instantiate(enemyData.PrefabList[enemyCode].Prefab);
        newObj.SetActive(false);
        newObj.transform.parent = transform;
        pool[enemyCode].Add(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void SetActiveOffAllObject()
    {
        for(int i = 0; i < enemyData.PrefabList.Count; i++)
        {
            foreach (GameObject obj in pool[i])
            {
                obj.SetActive(false);
            }
        }
    }
}
