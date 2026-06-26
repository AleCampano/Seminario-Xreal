using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;

    [Header("Referencias")]
    public CharacterController controller;
    public Animator animator;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        // Verificar suelo
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movimiento con flechas
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;

        // Animación Idle/Walk
        animator.SetFloat("Speed", move.magnitude);

        if (move.magnitude > 0.1f)
        {
            controller.Move(move * speed * Time.deltaTime);

            // Girar hacia la dirección de movimiento
            transform.forward = move;
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Activar animación de salto una sola vez
            animator.SetTrigger("Jump");
        }

        // Gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Ataque
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }
}