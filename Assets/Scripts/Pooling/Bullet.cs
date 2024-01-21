using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float life = 5f;

    private Vector3 _velocity;
    private void OnEnable()
    {
        Invoke("DeactiveSelf", life);
    }
    public void Initialize(Vector3 direction)
    {
        _velocity = direction.normalized * speed;
    }
    private void Update()
    {
        transform.position += _velocity * Time.deltaTime;
    }
    private void DeactiveSelf()
    {
        BulletPoolManager.sharedInstance.ReturnToPool(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DeactiveSelf();
    }
}
