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
    public static event Action<int> OnPlayerShieldUpdated;
    
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
        playerShield = 0;
        playerHealth = so_InitPlayer.playerHealth;
        playerMoney = 4;
        OnPlayerHealthUpdated?.Invoke(playerHealth);
        OnPlayerMoneyUpdated?.Invoke(playerMoney);
        OnPlayerShieldUpdated?.Invoke(playerShield);
        
    }

    public void AdjustPlayerHealth(int amount)
    {
        var damageRemaining = amount;
        for (var i = 0; i < amount; i++)
        {
            if (playerShield < 1) break;
            AdjustPlayerShield(-1);
            damageRemaining--;
        }
        playerHealth -= damageRemaining;
        OnPlayerHealthUpdated?.Invoke(playerHealth);
    }

    public void AdjustPlayerShield(int amount)
    {
        playerShield += amount;
        OnPlayerShieldUpdated?.Invoke(playerShield);
    }

    public void ResetPlayerShield()
    {
        playerShield = 0;
        playerMoney = 4;
        OnPlayerMoneyUpdated?.Invoke(playerMoney);
        OnPlayerShieldUpdated?.Invoke(playerShield);
    }

    public bool AdjustPlayerMoneyHasEnough(int amount)
    {
        if (playerMoney - amount < 0) return false;
        playerMoney -= amount;
        OnPlayerMoneyUpdated?.Invoke(playerMoney);
        return true;
    }
    
   
}
