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
        // 1. GRAVITACE A GROUND CHECK (Musí bìžet VŽDYCKY, i když na koni nikdo nesedí)
        IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = Vector3.zero;

        // 2. OVLÁDÁNÍ (Bìží JEN když sedíme)
        if (isMounted)
        {
            // Vstupy (W/S pro pohyb, A/D pro otáèení)
            float z = Input.GetAxis("Vertical");
            float x = Input.GetAxis("Horizontal");

            // Otáèení konì
            transform.Rotate(0, x * RotateSpeed * Time.deltaTime, 0);

            // Výpoèet pohybu dopøedu
            move = transform.forward * z;

            // Skok
            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }
        }

        // 3. APLIKACE POHYBU A GRAVITACE
        // Pokud nesedíme, 'move' je nula, takže kùò nejde dopøedu, ale gravitace stále funguje
        controller.Move(move * Speed * Time.deltaTime);

        velocity.y += Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}