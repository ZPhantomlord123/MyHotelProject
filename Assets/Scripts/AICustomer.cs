using UnityEngine;

public class AICustomer : MonoBehaviour
{
    private AICustomerManager customerManager;
    private Transform targetWaypoint;

    public void Initialize(AICustomerManager manager, Transform waypoint)
    {
        customerManager = manager;
        targetWaypoint = waypoint;
        MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        // Implement your movement logic here (e.g., using NavMeshAgent)
        // For simplicity, let's just move towards the target waypoint for now
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, Time.deltaTime * 2f);

        if (transform.position == targetWaypoint.position)
        {
            if (targetWaypoint == customerManager.GetNextWaypoint())
            {
                // Arrived at room, perform actions like checking in, etc.
                customerManager.RemoveCustomer(gameObject);
                return;
            }
            else
            {
                targetWaypoint = customerManager.GetNextWaypoint();
            }
        }
        Invoke("MoveToNextWaypoint", 0.5f); // Recursive call for movement
    }
}
