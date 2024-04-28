using UnityEngine;

public class DropArea : MonoBehaviour
{
    public float dropDelay = 1f; // Delay before dropping each item (in seconds)
    private bool playerInRange = false;
    private float dropTimer = 0f;
    private PlayerInventory playerInventory;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the drop area
        if (other.CompareTag("Player"))
        {
            // Start the drop timer and set the player in range
            playerInRange = true;
            dropTimer = dropDelay;
            // Get the player's inventory
            playerInventory = other.gameObject.GetComponent<PlayerInventory>();
        }
    }

    private void Update()
    {
        // Check if the player is in range and the drop timer is running
        if (playerInRange && dropTimer > 0f)
        {
            // Update the drop timer
            dropTimer -= Time.deltaTime;
            if (dropTimer <= 0f)
            {
                // Drop the top item from the player's inventory stack
                DropItem();
                // Reset the drop timer
                dropTimer = dropDelay;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the drop area
        if (other.CompareTag("Player"))
        {
            // Stop dropping items and reset the drop timer
            playerInRange = false;
            dropTimer = 0f;
        }
    }

    private void DropItem()
    {
        // Check if the player inventory is not empty
        if (playerInventory != null && playerInventory.GetItemCount() > 0)
        {
            // Get the top item from the player's inventory stack
            GameObject itemToDrop = playerInventory.GetTopItem();
            // Remove the item from the player's inventory
            playerInventory.RemoveItemFromStack(itemToDrop);
            // Drop the item at the drop area position
            itemToDrop.transform.position = transform.position;
            // Reset the item's parent
            itemToDrop.transform.SetParent(null);
            itemToDrop.GetComponent<Collider>().enabled = false;
            CashSpawner.instance.SpawnCash(this.transform.position+ new Vector3(0,0, -2f));
        }
    }
}
