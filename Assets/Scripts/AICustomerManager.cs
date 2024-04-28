using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AICustomerManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPosition;
    public Transform[] roomWaypoints; // Waypoints representing the path to rooms
    public List<Transform> queueWaypoints = new List<Transform>(); // Waypoints representing the path to queue
    public Dictionary<Transform, bool> roomAvailability = new Dictionary<Transform, bool>(); // Store room positions and availability
    public int maxCustomers = 10; // Maximum number of customers
    public float spawnInterval = 5f;

    private Queue<GameObject> customerQueue = new Queue<GameObject>(); // Queue to manage waiting customers
    private List<GameObject> customerPool = new List<GameObject>();
    private int currentWaypointIndex = 0;

    void Start()
    {
        // Initialize room availability dictionary
        foreach (Transform room in roomWaypoints)
        {
            roomAvailability.Add(room, true); // Initially all rooms are available
        }

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
                newCustomer.transform.position = spawnPosition.transform.position;
                newCustomer.SetActive(true); // Activate the customer
                customerQueue.Enqueue(newCustomer); // Enqueue the customer
                Transform queueSpot = queueWaypoints[customerQueue.Count - 1]; // Occupy the next available queue spot
                AICustomer aICustomer = newCustomer.GetComponent<AICustomer>();
                aICustomer.Initialize(this, queueSpot);
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
        currentWaypointIndex = (currentWaypointIndex + 1) % roomWaypoints.Length;
        return roomWaypoints[currentWaypointIndex];
    }

    public void RemoveCustomer(GameObject customer)
    {
        customer.SetActive(false); // Set the customer to inactive instead of destroying
    }

    // Method to check if a room is available
    public bool IsRoomAvailable(Transform room)
    {
        if (roomAvailability.ContainsKey(room))
        {
            return roomAvailability[room];
        }
        return false; // Room not found or dictionary error
    }

    // Method to set room availability
    public void SetRoomAvailability(Transform room, bool availability)
    {
        if (roomAvailability.ContainsKey(room))
        {
            roomAvailability[room] = availability;
        }
    }

    // Method to get the next available room
    public Transform GetNextAvailableRoom()
    {
        foreach (Transform room in roomWaypoints)
        {
            if (roomAvailability.ContainsKey(room) && roomAvailability[room])
            {
                return room;
            }
        }
        return null; // No available rooms
    }

    // Method to dequeue a customer when they reach their room
    public void DequeueCustomer(GameObject customer)
    {
        customerQueue.Dequeue(); // Remove the customer from the queue
        // Logic to handle customer dequeue
    }

    // Method to set availability of the next queue spot
    Transform GetQueueFirstSpotValue()
    {
        return queueWaypoints[0];
    }

    public void DequeueFirstCustomer()
    {
        if (customerQueue.Count > 0)
        {
            GameObject firstCustomer = customerQueue.Peek(); // Get the first customer without removing it
            AICustomer aiCustomer = firstCustomer.GetComponent<AICustomer>();
            if (aiCustomer != null)
            {
                Transform nextRoom = GetNextAvailableRoom();
                aiCustomer?.GoToRoom(nextRoom);
                SetRoomAvailability(nextRoom, false);

                DequeueCustomer(aiCustomer.gameObject);
                MoveRemainingCustomersToNextSpot();
            }
        }
    }

    public void MoveRemainingCustomersToNextSpot()
    {
        if (queueWaypoints.Count > 1) // Check if there are more than one queue spots
        {
            int queueIndex = 0; // Initialize the index for iterating over queue spots
            foreach (GameObject customer in customerQueue)
            {
                AICustomer aiCustomer = customer.GetComponent<AICustomer>();
                if (aiCustomer != null)
                {
                    Transform nextQueueSpot = queueWaypoints[queueIndex];
                    // Move the customer to the next spot in the queue
                    aiCustomer.GoToNextSpot(nextQueueSpot);
                    queueIndex++; // Move to the next queue spot
                }
            }
        }
    }


}
