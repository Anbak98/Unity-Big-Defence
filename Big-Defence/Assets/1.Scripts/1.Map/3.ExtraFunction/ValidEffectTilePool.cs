using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidEffectTilePool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int poolSize = 10;

    private List<GameObject> pool;

    private GameObject parentObject;

    void Awake()
    {
        pool = new List<GameObject>();
        parentObject = new("ValidEffectGroup");
        parentObject.transform.parent = transform;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.name = $"validEffectTile({i})";
            obj.SetActive(false);
            obj.transform.parent = parentObject.transform;
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        GameObject newObj = Instantiate(objectPrefab);
        newObj.SetActive(false);
        newObj.transform.parent = parentObject.transform;
        newObj.name = $"validEffectTile({pool.Count + 1})";
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
