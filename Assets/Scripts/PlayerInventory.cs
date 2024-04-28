using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int maxStackCount = 3; // Maximum stack count for items
    public Transform stackReference; // Reference transform for stacking
    public float stackSpacing = 0.15f; // Spacing between stacked items
    private List<GameObject> itemStack = new List<GameObject>(); // List to store stacked items


    // Method to get the count of items in the inventory stack
    public int GetItemCount()
    {
        return itemStack.Count;
    }

    // Add an item to the stack
    public void AddItemToStack(GameObject item)
    {
        Debug.Log("Item added");
        // Check if the stack count is less than the maximum limit
        if (itemStack.Count < maxStackCount)
        {
            // Calculate the vertical offset for the new item
            float yOffset = stackSpacing * itemStack.Count;
            // Calculate the position of the new item relative to the stack reference
            Vector3 newItemPosition = stackReference.TransformPoint(new Vector3(0f, yOffset, 0f));
            // Set the new item's position
            item.transform.position = newItemPosition;

            // Set the new item as a child of the stack reference
            item.transform.SetParent(stackReference);

            // Add the item to the stack
            itemStack.Add(item);
        }
        else
        {
            Debug.LogWarning("Stack is full. Cannot add more items.");
        }
    }

    // Get the top item from the stack
    public GameObject GetTopItem()
    {
        if (itemStack.Count > 0)
        {
            return itemStack[itemStack.Count - 1];
        }
        else
        {
            Debug.LogWarning("Stack is empty.");
            return null;
        }
    }

    // Remove an item from the stack
    public void RemoveItemFromStack(GameObject item)
    {
        if (itemStack.Contains(item))
        {
            itemStack.Remove(item);
        }
        else
        {
            Debug.LogWarning("Item not found in the stack.");
        }
    }
}
