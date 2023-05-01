using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour {
    
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sneakSpeed;
    [SerializeField] private float sprintSpeed;
    

    private Vector2 inputVelocity = new Vector2(0f, 0f);
    private CharacterController controller;

    [Header("Gravity")]
    [SerializeField] private float defaultFallVelocity;
    [SerializeField] private float gravity;
    private float yVelocity = 0 ;

    [Header("Mouse Controls")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity;
    [SerializeField][Range(-90f, 90f)] private float minCameraAngle;
    [SerializeField][Range(-90f, 90f)] private float maxCameraAngle;
    private float camXRotation = 0f;


    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sneakAction;
    private InputAction sprintAction;
    private InputAction lookAction;


    void Start() {
        controller = GetComponent<CharacterController>();
        moveAction = playerInput.actions.FindAction("move");
        jumpAction = playerInput.actions.FindAction("jump");
        sneakAction = playerInput.actions.FindAction("sneak");
        sprintAction = playerInput.actions.FindAction("sprint");
        lookAction = playerInput.actions.FindAction("look");

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        // Get controller input
        inputVelocity = moveAction.ReadValue<Vector2>();
        if (sneakAction.ReadValue<float>() > 0.1) { // Sneak
            inputVelocity *= sneakSpeed * Time.deltaTime;
        } else if (sprintAction.ReadValue<float>() > 0.1) { // Sprint
            inputVelocity *= sprintSpeed * Time.deltaTime;
        } else { // Walk
            inputVelocity *= walkSpeed * Time.deltaTime;
        }

        // Ground check
        bool isGroundedNow = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGroundedNow && !isGrounded) {
            // Fall damage goes here
        } 
        if (isGroundedNow) {
            yVelocity = defaultFallVelocity * Time.deltaTime;
        } else {
            yVelocity += gravity * Time.deltaTime;
        }
        isGrounded = isGroundedNow;

        // Camera
        Vector2 mouseMove = lookAction.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
        camXRotation -= mouseMove.y;
        camXRotation = Mathf.Clamp(camXRotation, minCameraAngle, maxCameraAngle);
        cameraTransform.localRotation = Quaternion.Euler(camXRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseMove.x);


        Move();

        
    }

    // The only controller.move call
    private void Move() {
        controller.Move(Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * (new Vector3(inputVelocity.x, yVelocity, inputVelocity.y)));
    }

    public void OnSneak() {
        Debug.Log("sneaking");
    }

}