using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleTwo
{
    public class Character : MonoBehaviour
    {
        public GameObject characterCamera;
        public float rotationSpeed = 2f;
        public Vector3 cameraOffset = new Vector3(0,3,-8);
        [SerializeField]
        private CharacterAbility[] _characterAbilities;
        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = false;
            _characterAbilities = GetComponents<CharacterAbility>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = !Cursor.visible;
            }
            RunAbilities();
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            characterCamera.transform.position = transform.position + cameraOffset.z * transform.forward + cameraOffset.y * Vector3.up;
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            transform.Rotate(0, horizontal, 0);
            characterCamera.transform.LookAt(transform);
        }
        private void RunAbilities()
        {
            foreach (CharacterAbility ability in _characterAbilities)
            {
                if (ability.enableAbility && ability.IsInitialized())
                    ability.RunAbility();
            }
        }
    }
}
