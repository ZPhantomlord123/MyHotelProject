using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public bool stackable;
    public float pickupDelay = 1f; // Delay before pickup (in seconds)
    public Image countdownImage; // Reference to the Image component for countdown visualization

    private bool canPickup = false;
    private bool playerInRange = false;
    private bool pickedUp = false; // Flag to track whether the item has been picked up
    private PlayerInventory playerInventory;

    private bool countdownStarted; // Flag to track whether the countdown has started
    private float countdownTimer; // Timer for the countdown

    private void Update()
    {
        // If the item can be picked up and the player is in range, initiate pickup after the delay
        if (canPickup && playerInRange && !pickedUp)
        {
            pickupDelay -= Time.deltaTime;
            if (pickupDelay <= 0f)
            {
                // Add the item to the player's inventory stack
                playerInventory.AddItemToStack(this.gameObject);
                // Set the pickedUp flag to true to prevent further pickups
                pickedUp = true;
                // Destroy the item from the scene
                //Destroy(gameObject);
            }

            // Update the fill amount of the countdown image
            if (countdownImage != null)
            {
                countdownImage.fillAmount = pickupDelay / countdownTimer;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is nearby
        if (other.CompareTag("Player"))
        {
            // Start the pickup delay timer
            playerInRange = true;
            playerInventory = other.gameObject.GetComponent<PlayerInventory>();
            canPickup = true;
            // Start the countdown if it's not started already
            if (!countdownStarted)
            {
                StartCountdown();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player moves out of range
        if (other.CompareTag("Player"))
        {
            // Stop the pickup delay timer
            playerInRange = false;
            canPickup = false;
            pickupDelay = 1f; // Reset the pickup delay

            // Reset the fill amount of the countdown image
            if (countdownImage != null)
            {
                countdownImage.fillAmount = 1f;
            }

            // Stop the countdown
            StopCountdown();
        }
    }

    public void DropItem()
    {
        // Implement logic to drop the item from the stack
    }

    private void StartCountdown()
    {
        countdownStarted = true;
        countdownTimer = pickupDelay;
    }

    private void StopCountdown()
    {
        countdownStarted = false;
    }
}
