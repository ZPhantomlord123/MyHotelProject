using System;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public static event Action<float> OnPlayerEnterCollider;
    public static event Action OnPlayerExitCollider;
    public float offsetX = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify subscribers that the player entered the collider
            OnPlayerEnterCollider?.Invoke(offsetX); // You can adjust the offset value as needed
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify subscribers that the player entered the collider
            OnPlayerExitCollider?.Invoke(); // You can adjust the offset value as needed
        }
    }
}
