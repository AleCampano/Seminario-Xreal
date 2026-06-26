using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;
    public float detectionRange = 5f;

    [Header("Movimiento")]
    public float moveSpeed = 2f;
    public float moveRadius = 4f;
    public float stoppingDistance = 0.2f;

    [Header("Tiempos")]
    public float idleTime = 2f;

    [Header("Referencias")]
    public CharacterController controller;
    public Animator animator;

    private bool activated = false;

    private Vector3 targetPosition;
    private float idleTimer;
    private bool moving = false;

    void Start()
    {
        animator.SetFloat("Speed", 0f);
    }

    void Update()
    {
        // Espera hasta que el jugador entre en rango
        if (!activated)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= detectionRange)
            {
                activated = true;
                ChooseNewTarget();
            }

            return;
        }

        if (moving)
        {
            MoveToTarget();
        }
        else
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTime)
            {
                ChooseNewTarget();
            }
        }
    }

    void MoveToTarget()
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction.magnitude <= stoppingDistance)
        {
            moving = false;
            idleTimer = 0f;

            animator.SetFloat("Speed", 0f);
            return;
        }

        direction.Normalize();

        controller.Move(direction * moveSpeed * Time.deltaTime);

        transform.forward = direction;

        // Activa la animación de caminar
        animator.SetFloat("Speed", 1f);
    }

    void ChooseNewTarget()
    {
        Vector2 random = Random.insideUnitCircle * moveRadius;

        targetPosition = transform.position + new Vector3(random.x, 0, random.y);

        moving = true;
    }

    // Dibuja el rango de detección en la escena
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}