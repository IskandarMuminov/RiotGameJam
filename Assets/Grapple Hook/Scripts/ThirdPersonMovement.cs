using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    //Script of movement
    public CharacterController controller;
    CameraHandler cameraHandler;
    public float mouseX;
    public float mouseY;
    public Transform cam;
    public Transform camPivot;
    public Camera mainCam;

    public bool isHookedToGrapplePoints = true;
    HookToGrapplePoints hookToPoints;
    HookToMousePoint hookToMouse;

    public float speed = 6f;
    public float jumpSpeed = 2.0f;
    public float gravity = 9.8f;
    public bool mainIsHooked;

    private Vector3 movingDirection = Vector3.zero;

    private bool isReadyWallJump = false;
    public bool isGrounded = false;


    //Animator
    public Animator animator;
    public float horizontal;
    public float vertical;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    private void Start()
    {
        if (isHookedToGrapplePoints) hookToPoints = GetComponent<HookToGrapplePoints>();
        else hookToMouse = GetComponent<HookToMousePoint>();
        cameraHandler = CameraHandler.singleton;
    }

    void Update()
    {
        //movement for player and camera
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        //Evade
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
                animator.SetTrigger("Evade");
            }

        }



        if (!isGrounded) animator.SetBool("IsGrounded", false);
        else animator.SetBool("IsGrounded", true);
        if (!mainIsHooked)
        {
            if (direction.magnitude >= 0.1f)
            {
                //Running
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                //JUMP
                movingDirection.y = jumpSpeed;
                isGrounded = false;
            }
            //WALL Jump
            if (isGrounded) isReadyWallJump = true;

            if (movingDirection.y >= -9.8f)
            {
                movingDirection.y -= gravity * Time.deltaTime;
            }
            controller.Move(movingDirection * Time.deltaTime);
        }
    }




    private void LateUpdate()
    {
        float delta = Time.deltaTime;
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }

    }

    //Find wall for wall jump
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!isGrounded && hit.normal.y < 0.1f)
        {
            if (Input.GetButtonDown("Jump") && isReadyWallJump)
            {
                movingDirection.y = jumpSpeed;
                isReadyWallJump = false;
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
            }
        }
        if (hit.normal.y == 1f)
        {
            isGrounded = true;
        }
    }
} 

