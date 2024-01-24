using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleTwo
{
    /// <summary>
    /// State machine, could be useful if we want to efficently manage the animation system later
    /// </summary>
    public class CharacterState
    {
        public enum CharacterMoveState
        {
            Idle,
            Walk,
            Falling,
            Jump
        }

        public enum WeaponState
        {
            WeaponIdle,
            WeaponInUse
        }
    }
}
