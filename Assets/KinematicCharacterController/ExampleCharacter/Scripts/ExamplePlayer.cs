using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
    public enum status
    {
        Small,
        Medium,
        Large,
        ExtraLarge
    }
namespace KinematicCharacterController.Examples
{

    public class ExamplePlayer : MonoBehaviour
    {
        public ExampleCharacterController Character;
        public ExampleCharacterCamera CharacterCamera;
        public KinematicCharacterMotor CM;
        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        public Animator animator;
        [SerializeField] private status CharacterStatus;
        private bool canJump = false;
        private bool canGrapple = false;
        public GameObject grappleGO;
        public GameObject characterModel;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
            ApplyState(CharacterStatus);
            
            CM.SetCapsuleDimensions(CM.Capsule.radius, CM.Capsule.height ,CM.GetYoff());
            
            
        
        }
        public void ApplyState(status status)
        {
            CharacterStatus = status;
            switch (status)
            {
                case status.Small:
                    canJump = false;
                    break;
                case status.Medium:
                    canJump = true;
                    characterModel.transform.localScale = characterModel.transform.localScale * 3f;
                    CM.SetCapsuleDimensions(CM.Capsule.radius*3, CM.Capsule.height*3 ,CM.GetYoff()*3f);
                    break;
                case status.Large:
                    canJump = true;
                    canGrapple = true;
                    CM.SetCapsuleDimensions(CM.Capsule.radius*5, CM.Capsule.height*5 ,CM.GetYoff()*5f);
                    characterModel.transform.localScale = characterModel.transform.localScale * 5f;
                    activeGrapple();
                    break;
                default:
                    break;
            }
        }
        
        void activeGrapple()
        {
            grappleGO.SetActive(true);
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Attack");
            }

            if (Input.GetAxisRaw(HorizontalInput) != 0 || Input.GetAxisRaw(VerticalInput) != 0)
            {

                animator.SetBool("Walking", true);
            }
            else
            {
                animator.SetBool("Walking", false);
            }
            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
            float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            // if (Input.GetMouseButtonDown(1))
            // {
            //     CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
            // }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            if(canJump) characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
            characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
            characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
}