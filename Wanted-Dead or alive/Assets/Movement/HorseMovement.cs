using UnityEngine;

public class HorseMovement : MonoBehaviour
{
    public CharacterController controller;

    public float Speed = 18f;
    public float RotateSpeed = 100f;
    public float Gravity = -9.81f;
    public float JumpHeight = 4f;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;

    private Vector3 velocity;
    private bool IsGrounded;
    public bool isMounted = false;

    void Update()
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = Vector3.zero;

        if (isMounted)
        {
            float z = Input.GetAxis("Vertical");
            float x = Input.GetAxis("Horizontal");

            transform.Rotate(0, x * RotateSpeed * Time.deltaTime, 0);
            move = transform.forward * z;

            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }
        }

        controller.Move(move * Speed * Time.deltaTime);

        velocity.y += Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}