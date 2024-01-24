using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleTwo
{
    /// <summary>
    /// The main character shoot class to manage the character shooting ability.
    /// This class is derived from character ability class.
    /// Left click to shoot bullet
    /// </summary>
    public class CharacterShoot : CharacterAbility
    {
        public Transform firePosition;

        protected CharacterState.WeaponState _currentWeaponState;
        protected override void Init()
        { 
            base.Init();

            if (firePosition == null)
            {
                firePosition = GameObject.Find("FirePosition").transform;
                if (firePosition == null)
                    Debug.LogError("Cannot find 'FirePosition' game object in the scene.");
            }
            _currentWeaponState = CharacterState.WeaponState.WeaponIdle;
        }

        void Update()
        {
            // Left click to shoot bullet
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
            else
            {
                _currentWeaponState = CharacterState.WeaponState.WeaponIdle;
            }
        }

        public void Shoot()
        {
            // Pooling manager is a singleton object
            GameObject bulletObject = BulletPoolManager.sharedInstance.TakeFromPool();
            bulletObject.transform.position = firePosition.position;
            bulletObject.transform.rotation = firePosition.rotation;
            Bullet bulletComponent = bulletObject.GetComponent<Bullet>();
            // Fire the bullet by giving the initial direction
            bulletComponent.Initialize(firePosition.forward);
            _currentWeaponState = CharacterState.WeaponState.WeaponInUse;
        }
        void OnDrawGizmos()
        {
            // Debug purpose, show the shooting direction
            Gizmos.color = Color.red;
            Vector3 endPoint = firePosition.transform.position + firePosition.forward * 25f; // Calculate the end point based on the direction and length
            Gizmos.DrawLine(firePosition.transform.position, endPoint);
        }
        
    }
}
