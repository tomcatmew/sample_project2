using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager sharedInstance;
    public GameObject bulletPrefab;
    public int poolSize = 20;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    // Start is called before the first frame update
    private void Awake()
    {
        sharedInstance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for(int i = 0; i < poolSize; i ++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.SetParent(transform);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public GameObject TakeFromPool()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.SetParent(transform);
            return bullet;
        }
    }

    public void ReturnToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
