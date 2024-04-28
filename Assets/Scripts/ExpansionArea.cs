using UnityEngine;
using UnityEngine.UI;

public class ExpansionArea : MonoBehaviour
{
    public string areaName;
    public GameObject objectToEnable; // Reference to the object to enable
    public float range = 5f; // Range of the expansion area
    public int amount = 100; // Amount required to enable the object
    public float roomToEnable = 10f; // Room to enable the object (time in seconds)
    public Image countdownImage; // Reference to the Image component for countdown visualization

    private bool playerInRange;
    private bool countdownStarted;
    private float countdownTimer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Start the countdown if the player has enough money and the object is not already enabled
            if (CurrencyManager.instance.HasEnoughCash(amount) && !objectToEnable.activeSelf)
            {
                StartCountdown();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Stop the countdown if it's started
            StopCountdown();
        }
    }

    void Update()
    {
        if (playerInRange)
        {
            // If the player is in range and countdown is started
            if (countdownStarted)
            {
                // If player does not have enough cash, stop the countdown
                if (!CurrencyManager.instance.HasEnoughCash(amount))
                {
                    StopCountdown();
                    return;
                }

                // Update the countdown timer
                countdownTimer -= Time.deltaTime;
                // Update the fill amount of the countdown image
                if (countdownImage != null)
                {
                    countdownImage.fillAmount = countdownTimer / roomToEnable;
                }
                // If the countdown timer reaches zero, enable the object
                if (countdownTimer <= 0f)
                {
                    EnableObject();
                }
            }
        }
        else
        {
            // If the player is out of range, stop the countdown
            StopCountdown();
        }
    }

    void StartCountdown()
    {
        countdownStarted = true;
        countdownTimer = roomToEnable;
    }

    void StopCountdown()
    {
        countdownStarted = false;
    }

    void EnableObject()
    {
        objectToEnable.SetActive(true);

        //save room daata
        if(areaName != null)
        {
            PlayerPrefs.SetInt(areaName, 1);
        }

        // Optionally, deduct the amount from the player's cash
        CurrencyManager.instance.RemoveCash(amount);
        // Disable the ExpansionArea object
        gameObject.SetActive(false);
    }
}
