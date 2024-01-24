using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleTwo
{
    /// <summary>
    /// Bullet is a pooling object, after being taken from pool
    /// The initial direction will be set and shoot out.
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        public float speed = 20f;
        public float life = 5f;

        // _velocity defines the moving mangitude and direction of the bullet
        private Vector3 _velocity;
        private void OnEnable()
        {
            // deactive the bullet after passing period of time
            Invoke("DeactiveSelf", life);
        }
        public void Initialize(Vector3 direction)
        {
            _velocity = direction.normalized * speed;
        }
        // simple bullet will fire as a straight line without affection of gravity
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
}