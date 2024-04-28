using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AICustomer : MonoBehaviour
{
    private AICustomerManager customerManager;
    public Transform targetWaypoint;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private enum CustomerState {Idle, WaitingInLine, WalkToRoom, RunToRoom };
    private CustomerState currentState = CustomerState.WaitingInLine;

    private bool isMoving = false;

    public void Initialize(AICustomerManager manager, Transform waypoint)
    {
        customerManager = manager;
        targetWaypoint = waypoint;
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (!navMeshAgent || !navMeshAgent.enabled)
        {
            Debug.LogWarning("NavMeshAgent is not set or disabled.");
            return;
        }

        animator = GetComponent<Animator>(); // Get the Animator component

        // Set the destination to the target waypoint
        navMeshAgent.SetDestination(targetWaypoint.position);

        // Set the customer state to walk in line
        ChangeState(CustomerState.WaitingInLine);

        // Subscribe to NavMeshAgent's arrival event
        navMeshAgent.stoppingDistance = 0.1f;
        navMeshAgent.autoBraking = true;
        navMeshAgent.isStopped = false;
    }

    public void GoToNextSpot(Transform location)
    {
        targetWaypoint = location;

        // Set the destination to the target waypoint
        navMeshAgent.SetDestination(targetWaypoint.position);
    }

    public void GoToRoom(Transform location)
    {
        CashSpawner.instance.SpawnCash(transform.position+ new Vector3(-1.5f,1,1f));

        // Set the customer state to MovingToRoom
        ChangeState(Random.Range(0,2) == 0?CustomerState.WalkToRoom:CustomerState.RunToRoom);

        targetWaypoint = location;
        // Check if the room is available
        if (!customerManager.IsRoomAvailable(targetWaypoint))
        {
            Debug.LogWarning("Room is not available.");
            return;
        }

        // Set the destination to the target waypoint
        navMeshAgent.SetDestination(targetWaypoint.position);
    }

    // Function to change state and handle animation
    void ChangeState(CustomerState nextState)
    {
        currentState = nextState;
        switch (currentState)
        {
            case CustomerState.Idle:
                // Set animator speed parameter for idle animation
                if (animator != null)
                {
                    animator.SetFloat("Speed", 0.0f); // Set speed to 0 for idle animation
                    animator.SetBool("IsRunning", false); // Make sure IsRunning is false
                }
                break;
            case CustomerState.WaitingInLine:
                // Set animator speed parameter for movement animation
                if (animator != null)
                {
                    animator.SetFloat("Speed", 5.0f);
                    animator.SetBool("IsRunning", false); // Make sure IsRunning is false
                    isMoving = true;
                }
                break;
            case CustomerState.WalkToRoom:
                // Set animator speed parameter for movement animation
                if (animator != null)
                {
                    animator.SetFloat("Speed", 5.0f); // Set speed to 1 for walking animation
                    animator.SetBool("IsRunning", false); // Make sure IsRunning is false
                    isMoving = true;
                }
                break;
            case CustomerState.RunToRoom:
                // Set animator speed parameter for movement animation
                if (animator != null)
                {
                    animator.SetFloat("Speed", 5.0f); // Set speed to 1 for walking animation
                    animator.SetBool("IsRunning", true);
                    isMoving = true;
                }
                break;
        }
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f && isMoving)
        {
            ChangeState(CustomerState.Idle);
            isMoving = false;
        }
    }
}
