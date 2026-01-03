using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;

    public float Speed = 12f;
    public float Gravity = -9.81f;
    public float JumpHeight = 3f;

    public Transform GroundCheck;
    //public Transform WaterCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;
    //public LayerMask WaterLayer;

    private Vector3 velocity;
    private bool IsGrounded;
    //public bool IsSwimming;
    //[Header("Sound Effects")]
    //public AudioClip swimmingSound;
    //private AudioSource audioSource;
    //private bool isPlayingSwimSound = false;

    /*void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    */
    void Update()
    {
        if (Keyboard.current.leftShiftKey.isPressed && IsGrounded && Keyboard.current.wKey.isPressed)
        {
            Speed = 16f;
        }
        else
        {
            Speed = 12f;
        }

        IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
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

}