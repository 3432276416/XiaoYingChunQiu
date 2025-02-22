using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolTool : MonoBehaviour
{
    public GameObject prefab;
    ObjectPool<GameObject> pool;

    private void Awake() {
        //初始化对象池
        pool = new ObjectPool<GameObject>(
            createFunc:()=>Instantiate(prefab,transform),
            actionOnGet:(obj)=>obj.SetActive(true),
            actionOnRelease:(obj)=>obj.SetActive(false),
            actionOnDestroy:(obj)=>Destroy(obj),
            collectionCheck:false,
            defaultCapacity:10,
            maxSize:100
        );
        PreFillPool(7);
    }

/// <summary>
/// 初始化预先生成物体到对象池
/// </summary>
/// <param name="count">初始数量</param>
    void PreFillPool(int count)
    {
        var preFillArray = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = pool.Get();
        }

        foreach (var item in preFillArray)
        {
            pool.Release(item);
        }
    }

/// <summary>
/// 从对象池获取对象
/// </summary>
/// <returns>GameObject对象</returns>
    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }

/// <summary>
/// 把对象放回对象池
/// </summary>
/// <param name="obj">GameObject对象</param>
    public void ReturnObjectFromPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
