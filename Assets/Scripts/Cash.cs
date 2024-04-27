using UnityEngine;

public class Cash : MonoBehaviour
{
    public int cashValue = 10; // Value written on the cash object

    public float spinSpeed = 100f; // Spin speed along the z-axis
    public float moveSpeed = 2f; // Speed at which the cash object moves towards the player

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Spin the cash object along the z-axis
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Move towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // If the player collects the cash object, add its value to the player's cash
            CurrencyManager.instance.AddCash(cashValue);
            // Destroy the cash object after collecting
            Destroy(gameObject);
        }
    }
}
