using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleTwo
{
    public class CharacterShoot : CharacterAbility
    {
        public Transform firePosition;

        protected CharacterState.WeaponState _currentWeaponState;
        protected override void Init()
        { 
            base.Init();
            _currentWeaponState = CharacterState.WeaponState.WeaponIdle;
        }

        // Update is called once per frame
        void Update()
        {
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
            GameObject bulletObject = BulletPoolManager.sharedInstance.TakeFromPool();
            bulletObject.transform.position = firePosition.position;
            bulletObject.transform.rotation = firePosition.rotation;
            Bullet bulletComponent = bulletObject.GetComponent<Bullet>();
            bulletComponent.Initialize(transform.forward);
            _currentWeaponState = CharacterState.WeaponState.WeaponInUse;

        }
    }
}
