using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleTwo
{
    public class CharacterState
    {
        public enum CharacterMoveState
        {
            Idle,
            Walk,
            Falling,
            Run,
            Jump
        }

        public enum WeaponState
        {
            WeaponIdle,
            WeaponInUse
        }
    }
}
