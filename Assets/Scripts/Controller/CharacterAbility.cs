using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleTwo
{
    public class CharacterAbility : MonoBehaviour
    {
        /// <summary>
        /// Gets and stores components for further use
        /// </summary>
        public bool enableAbility = true;

        protected bool _initialized = false;
        protected Character _character;
        protected CharacterController _characterController;
        protected virtual void Awake()
        {
            _character = GetComponent<Character>();
            _characterController = GetComponent<CharacterController>();
        }
        protected virtual void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            _initialized = true;
        }
        public virtual void RunAbility()
        {

        }
        public void EnableAbility(bool enable)
        {
            enableAbility = enable;
        }
        public bool IsInitialized()
        {
            return _initialized;
        }
    }
}