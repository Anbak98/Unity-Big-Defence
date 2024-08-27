using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidEffectTilePool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int poolSize = 10;

    private List<GameObject> pool;

    private GameObject parentObject;

    private void Awake()
    {
        pool = new List<GameObject>();
        parentObject = new("InvalidEffectGroup");
        parentObject.transform.parent = transform;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.name = $"invalidEffectTile({i})";
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
        newObj.name = $"invalidEffectTile({pool.Count + 1})";
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
