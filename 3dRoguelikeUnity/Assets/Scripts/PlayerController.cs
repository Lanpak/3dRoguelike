using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    private Vector3 moveDirection = Vector3.zero;

    private CharacterController controller;

    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private bool isPlayerGrounded = false;
    private Vector3 playerVelocity = Vector3.zero;

    private void Start()
    {
        GetReferences();

        moveSpeed = walkSpeed;
    }

    private void Update()
    {

        HandleGravity();
        HandleRunning();
        HandleMovement();
    }

    private void HandleGravity()
    {
        HandleJump();

        if(isPlayerGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0.2f;
        }

        isPlayerGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPlayerGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
        }
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = moveDirection.normalized;
        moveDirection = transform.TransformDirection(moveDirection);

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void GetReferences()
    {
        controller = GetComponent<CharacterController>();
    }

    private void HandleRunning()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
        }
    }

}
