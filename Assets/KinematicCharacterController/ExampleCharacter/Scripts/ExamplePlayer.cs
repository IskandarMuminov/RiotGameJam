using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
    // public enum status
    // {
    //     Small,
    //     Medium,
    //     Large,
    //     ExtraLarge
    // }
namespace KinematicCharacterController.Examples
{

    public class ExamplePlayer : MonoBehaviour
    {
        public ExampleCharacterController Character;
        public ExampleCharacterCamera CharacterCamera;
        public KinematicCharacterMotor CM;
        [SerializeField] private float seedlingSize = 1f;
        [SerializeField] private float saplingSize = 3f;
        [SerializeField] private float youngSize = 5f;
        [SerializeField] private float matureSize = 10f;
        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        public Animator animator;
        [SerializeField] private Player_State CharacterStatus;
        [SerializeField] private bool canJump = false;
        [SerializeField] private bool canGrapple = false;
        public GameObject grappleGO;
        public GameObject characterModel;
        private float radius, heights, offset;
        private Vector3 initialSizeModel;

        void OnEnable()
        {
            ResourceManager.StateChanged += ApplyState; 
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());

            ApplyState(CharacterStatus);


        }
        void Awake()
        {
            radius = CM.Capsule.radius;
            heights = CM.Capsule.height;
            offset = CM.GetYoff();
            initialSizeModel = characterModel.transform.localScale;
        }
        public void ApplyState(Player_State status)
        {
            CharacterStatus = status;
            switch (status)
            {
                case Player_State.Seedling:
                    canJump = false;
                    break;
                case Player_State.Sapling:
                    canJump = true;
                    multiplySize(saplingSize);
                    break;
                case Player_State.Young:
                    canJump = true;
                    canGrapple = true;
                    multiplySize(youngSize);
                    activeGrapple();
                    break;
                case Player_State.Mature:
                    // final stage
                    break;
                default:
                    break;
            }
        }
        private void multiplySize(float multiflier)
        {
            CM.SetCapsuleDimensions(radius * multiflier, heights * multiflier, offset * multiflier);
            characterModel.transform.localScale = initialSizeModel * multiflier;
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
            if (canJump && Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
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