using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public CharacterController controller;
    public float Speed = 12f;
    public float Gravity = -9.81f;
    public float JumpHeight = 3f;

    [Header("Ground Check")]
    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;

    [Header("Interaction Settings")]
    public float pushPower = 2.0f;

    public bool canMove = true;

    private Vector3 velocity;
    private bool IsGrounded;

    void Update()
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!canMove)
        {
            velocity.y += Gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            return;
        }

        if (Keyboard.current.leftShiftKey.isPressed && IsGrounded && Keyboard.current.wKey.isPressed)
        {
            Speed = 16f;
        }
        else
        {
            Speed = 12f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * Speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }

        velocity.y += Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.AddForceAtPosition(pushDir * pushPower, hit.point, ForceMode.Impulse);
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero;
    }
}