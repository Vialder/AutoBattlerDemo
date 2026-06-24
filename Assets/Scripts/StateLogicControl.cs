using System;
using UnityEngine;

#pragma warning disable CS0414
public class StateLogicControl : MonoBehaviour
{
    public static event Action OnEnterShopState;
    public static event Action OnGameLoaded;
    public static event Action OnEnterCombatState;
    public static event Action OnReRoll;
    [SerializeField] private GameState gameState;
    
    
    //main flow:
    //1. buy/sell cards
    //2. press end turn, resolve combat
    //3. spawn enemy
    //4. go to step 1
    
    
    private void Awake()
    {
        ActionProcessor.OnCombatEnd += EnterShopStage;
    }
    
    private void Start()
    {
        GameLoaded();
        EnterShopStage();
    }
    
    private void GameLoaded()
    {   
        Debug.Log("Game Loaded");
        OnGameLoaded?.Invoke();
    }
    
    private void EnterShopStage()
    {   
        Debug.Log("Enter Shop Stage");
        gameState = GameState.Shop;
        OnEnterShopState?.Invoke();
    }

    public void ButtonEndTurn()
    {
        EndTurn();
    }

    public void ButtonReRoll()
    {
        ReRollItems();
    }

    private void EndTurn()
    {
        OnEnterCombatState?.Invoke();
        gameState = GameState.Combat;
    }

    private void ReRollItems()
    {
        OnReRoll?.Invoke();
    }
}

public enum GameState
{
    Shop,
    Combat
}