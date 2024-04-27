using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f; // Walk speed of the player
    public float runSpeed = 10f; // Run speed of the player

    private Animator animator;
    private CharacterController controller;
    private bool isRunning;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get player input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Determine movement speed based on player input
        float moveSpeed = isRunning ? runSpeed : walkSpeed;

        // Move the player
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Update animator parameters based on movement speed
        float moveMagnitude = new Vector2(horizontalInput, verticalInput).magnitude;
        animator.SetFloat("Speed", moveMagnitude);

        // Set running parameter in the animator
        animator.SetBool("IsRunning", isRunning);

        // Rotate player to face movement direction
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        // Toggle running based on player input
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }
}
