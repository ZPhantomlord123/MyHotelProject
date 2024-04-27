using UnityEngine;

public class AreaController : MonoBehaviour
{
    public GameObject objectToEnableDisable; // Reference to the GameObject to enable/disable
    public AICustomerManager customerManager;

    public float countdownDuration = 10f; // Duration of the countdown timer in seconds
    public float countdownTimer = 0f; // Timer to count down when the player is inside the area
    private bool isPlayerInside = false; // Flag to track whether the player is inside the area

    private void Update()
    {
        if (isPlayerInside)
        {
            // If the player is inside the area, start or continue the countdown
            if (countdownTimer > 0f)
            {
                countdownTimer -= Time.deltaTime;
                if (countdownTimer <= 0f)
                {
                    customerManager.DequeueFirstCustomer();
                    countdownTimer = countdownDuration; // Reset the countdown timer
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true; // Player is inside the area
            countdownTimer = countdownDuration; // Reset the countdown timer
            // Enable the specified GameObject when the player enters the area
            objectToEnableDisable.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false; // Player has left the area
            objectToEnableDisable.SetActive(false);
        }
    }
}
