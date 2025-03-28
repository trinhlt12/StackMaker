using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly GameObject        prefab;
    private readonly Queue<GameObject> pool = new();

    public ObjectPool(GameObject prefab, int initialSize)
    {
        this.prefab = prefab;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Object.Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Object.Instantiate(prefab);
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    public void ReturnAll()
    {
        foreach (var obj in pool)
        {
            obj.SetActive(false);
        }
    }
}