using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleTwo
{
    /// <summary>
    /// Pooling Manager class that will manage the initializing and spawning of bullet objects to pool
    /// </summary>
    public class BulletPoolManager : MonoBehaviour
    {
        public static BulletPoolManager sharedInstance;
        public GameObject bulletPrefab;
        public int poolSize = 100;

        private Queue<GameObject> bulletPool = new Queue<GameObject>();
        private void Awake()
        {
            sharedInstance = this;
            if (bulletPrefab != null)
                InitializePool();
            else
                Debug.LogError("Bullet prefab is not assigned.");
        }

        private void InitializePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.SetParent(transform);
                bullet.SetActive(false);
                bulletPool.Enqueue(bullet);
            }
        }

        // Take the buttet reference from pool
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

        // Return the bullet to pool
        public void ReturnToPool(GameObject bullet)
        {
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }
}
