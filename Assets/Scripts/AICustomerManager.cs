using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICustomerManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform[] waypoints; // Waypoints representing the path to rooms
    public int maxCustomers = 10; // Maximum number of customers
    public float spawnInterval = 5f;

    private List<GameObject> customerPool = new List<GameObject>();
    private int currentWaypointIndex = 0;

    void Start()
    {
        // Pre-instantiate customers and set them to idle state
        for (int i = 0; i < maxCustomers; i++)
        {
            GameObject newCustomer = Instantiate(customerPrefab, transform.position, Quaternion.identity);
            newCustomer.SetActive(false); // Initially set to inactive
            customerPool.Add(newCustomer);
        }

        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            // Get an inactive customer from the pool
            GameObject newCustomer = GetInactiveCustomer();
            if (newCustomer != null)
            {
                newCustomer.SetActive(true); // Activate the customer
                AICustomer aiCustomer = newCustomer.GetComponent<AICustomer>();
                aiCustomer.Initialize(this, waypoints[currentWaypointIndex]);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    GameObject GetInactiveCustomer()
    {
        // Find and return the first inactive customer in the pool
        foreach (GameObject customer in customerPool)
        {
            if (!customer.activeInHierarchy)
            {
                return customer;
            }
        }
        return null; // If all customers are active, return null
    }

    public Transform GetNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        return waypoints[currentWaypointIndex];
    }

    public void RemoveCustomer(GameObject customer)
    {
        customer.SetActive(false); // Set the customer to inactive instead of destroying
    }
}
