using UnityEngine;

public class HorseMovement : MonoBehaviour
{
    public CharacterController controller;

    // Rychlejší než hráè
    public float Speed = 18f;
    public float RotateSpeed = 100f;
    public float Gravity = -9.81f;
    public float JumpHeight = 4f;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;

    private Vector3 velocity;
    private bool IsGrounded;

    // Abychom vìdìli, jestli na nìm sedíme
    public bool isMounted = false;

    void Update()
    {
        // Pokud na koni nikdo nesedí, nehýbe se
        if (!isMounted) return;

        // Gravitace a Ground Check
        IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Vstupy (W/S pro pohyb, A/D pro otáèení)
        float z = Input.GetAxis("Vertical"); // Dopøedu/Dozadu
        float x = Input.GetAxis("Horizontal"); // Otáèení

        // 1. Otáèení konì (rotace podle osy Y)
        transform.Rotate(0, x * RotateSpeed * Time.deltaTime, 0);

        // 2. Pohyb dopøedu (kùò chodí hlavnì dopøedu, ne do boku)
        Vector3 move = transform.forward * z;
        controller.Move(move * Speed * Time.deltaTime);

        // Skok
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }

        // Aplikace gravitace
        velocity.y += Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}