using System;
using TMPro;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public static PlayerState Instance;
    public SO_InitPlayer so_InitPlayer;

    [SerializeField] private int playerShield;
    [SerializeField] private int playerHealth;
    [SerializeField] private int playerMoney;

    public static event Action<int> OnPlayerHealthUpdated;
    public static event Action<int> OnPlayerMoneyUpdated;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        playerShield = so_InitPlayer.playerShield;
        playerHealth = so_InitPlayer.playerHealth;
        playerMoney = 4;
        OnPlayerHealthUpdated?.Invoke(playerHealth);
        OnPlayerMoneyUpdated?.Invoke(playerMoney);
        
    }

    public void AdjustPlayerHealth(int amount)
    {
        playerHealth -= amount;
        OnPlayerHealthUpdated?.Invoke(playerHealth);
    }

    public void AdjustPlayerShield(int amount)
    {
        playerShield += amount;
    }

    public void ResetPlayerShield()
    {
        playerShield = 0;
        playerMoney = 4;
    }

    public bool AdjustPlayerMoneyHasEnough(int amount)
    {
        if (playerMoney - amount < 0) return false;
        playerMoney -= amount;
        OnPlayerMoneyUpdated?.Invoke(playerMoney);
        return true;
    }
    
   
}
