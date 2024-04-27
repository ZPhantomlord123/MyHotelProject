using UnityEngine;
using System.Collections.Generic;

public class CashSpawner : MonoBehaviour
{
    public static CashSpawner instance; // Singleton instance

    public GameObject cashPrefab;
    public int poolSize = 20;
    public GameObject spawnBound; // Reference to the GameObject defining spawn bounds

    private List<GameObject> cashPool = new List<GameObject>();

    void Awake()
    {
        // Singleton instance initialization
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize the cash object pool
        InitializePool();
    }

    public void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject cash = Instantiate(cashPrefab, Vector3.zero, Quaternion.identity);
            cash.SetActive(false);
            cashPool.Add(cash);
        }
    }

    public GameObject SpawnCash(Vector3 position)
    {
        // Find an inactive cash object from the pool
        foreach (GameObject cash in cashPool)
        {
            if (!cash.activeSelf)
            {
                // Set position for spawning
                cash.transform.position = position;
                cash.SetActive(true);
                return cash;
            }
        }
        // If no inactive cash object found, expand the pool and spawn
        GameObject newCash = Instantiate(cashPrefab, position, Quaternion.identity);
        cashPool.Add(newCash);
        return newCash;
    }

    public void DeactivateCash(GameObject cash)
    {
        // Deactivate cash object
        cash.SetActive(false);
    }
}
