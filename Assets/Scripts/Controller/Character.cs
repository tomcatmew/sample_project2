using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleTwo
{
    /// <summary>
    /// The Character class storing variable for updating camera and weapon position/rotation.
    /// Also this class will run all character abilities in sequence.
    /// </summary>
    public class Character : MonoBehaviour
    {
        public GameObject characterCamera;
        public GameObject cameraTracker;
        public Vector3 cameraOffset = new Vector3(0,3,-8);
        public float mouseSensitivity = 200f;
        public LayerMask groundLayer;
        public Transform weapon;

        private float xRotation = 0f;
        [SerializeField]
        private CharacterAbility[] _characterAbilities;

        // Initialize the variables
        void Start()
        {
            if (characterCamera == null)
                if (Camera.main != null)
                    characterCamera = Camera.main.gameObject;
                else
                    Debug.LogError("Main camera is not found");
            if (cameraTracker == null)
            {
                cameraTracker = GameObject.Find("cameraTracker");
                if (cameraTracker == null)
                    Debug.LogError("Cannot find 'FirePosition' game object in the scene.");
            }
            if (weapon == null)
            {
                weapon = GameObject.Find("gun").transform;
                if (weapon == null)
                    Debug.LogError("Cannot find 'gun' game object in the scene.");
            }
            Cursor.lockState = CursorLockMode.Locked;
            _characterAbilities = GetComponents<CharacterAbility>();
        }

        // Update the character abilities, camera and aimming every frame
        void Update()
        {
            RunAbilities();
            UpdateCamera();
            UpdateAim();
        }

        // Find the aimming direction of the character based on the camera facing direction.
        // Rotate the gun to aim the first object which the ray hits.
        // If no object the ray hits, just let the gun aim to the position while ray travels 100 unit distance.
        private void UpdateAim()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, groundLayer))
            {
                Vector3 hitPoint = hit.point;
                Vector3 direction = hitPoint - weapon.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                weapon.rotation = Quaternion.Slerp(weapon.rotation, rotation, Time.deltaTime * 10);
            }
            else
            {
                Vector3 limitPosition = ray.origin + ray.direction * 100f;
                Vector3 direction = limitPosition - weapon.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                weapon.rotation = Quaternion.Slerp(weapon.rotation, rotation, Time.deltaTime * 10);
            }
        }

        // Update the camera position and let camera always look at the character's a little bit above position.
        // To create a third person view camera.
        // Prevent the rotation of fliping the camera.
        private void UpdateCamera()
        {
            characterCamera.transform.position = cameraTracker.transform.position + cameraOffset.z * cameraTracker.transform.forward + cameraOffset.y * Vector3.up;
            characterCamera.transform.LookAt(cameraTracker.transform);

            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);  // Limit the rotation to prevent flipping
            characterCamera.transform.RotateAround(cameraTracker.transform.position, cameraTracker.transform.right, xRotation);
        }
        // Run the character abilities such as movement and shoot in sequence.
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
