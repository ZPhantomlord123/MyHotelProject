using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    public int cash;

    public TextMeshProUGUI cashText; // Reference to the TextMeshProUGUI component

    void Awake()
    {
        // Ensure only one instance of CurrencyManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the CurrencyManager object alive between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }

        // Load saved cash when the game starts
        LoadCash();
    }

    void Start()
    {
        // Update the cash text when the game starts
        UpdateCashText();
    }

    public void AddCash(int amount)
    {
        if (amount < 0)
        {
            Debug.Log("Cannot add negative amount.");
            return;
        }
        cash += amount;
        Debug.Log($"Added {amount} to cash. Total cash: {cash}");

        // Save cash after adding
        SaveCash();
        // Update the cash text
        UpdateCashText();
    }

    public void RemoveCash(int amount)
    {
        if (amount < 0)
        {
            Debug.Log("Cannot remove negative amount.");
            return;
        }
        if (amount > cash)
        {
            Debug.Log("Insufficient funds.");
            return;
        }
        cash -= amount;
        Debug.Log($"Removed {amount} from cash. Total cash: {cash}");

        // Save cash after removing
        SaveCash();
        // Update the cash text
        UpdateCashText();
    }

    public void SaveCash()
    {
        PlayerPrefs.SetInt("Cash", cash);
        PlayerPrefs.Save();
        Debug.Log($"Saved cash ({cash}) to PlayerPrefs");
    }

    public void LoadCash()
    {
        cash = PlayerPrefs.GetInt("Cash", 0);
        Debug.Log($"Loaded cash: {cash}");
    }

    void UpdateCashText()
    {
        // Update the TextMeshProUGUI component with the current cash amount
        if (cashText != null)
        {
            // Format the cash amount as "cash $"
            string formattedCash = $"{cash} $";
            // Set the text
            cashText.text = formattedCash;
            // Set the color to green
            cashText.color = Color.green;
        }
    }

}
