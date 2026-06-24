using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using CardArchetypes;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Queue<IGameItem> itemQueue;
    private List<IGameItem> gameItems;
    private int tickDelta;
    [SerializeField] private int tick;
    private IGameItem[] handItems;
    private Dictionary<int, int> combatCooldownDict;
    public static event Action<int, IGameItem> OnItemActivated;
    public static event Action<IGameItem> OnNewItemAdded;
    public EnemySystem eSystem;
    
    private void Awake()
    {   
        gameItems = new List<IGameItem>
        {
            null,
            null,
            null,
            null,
            null
        };
        itemQueue = new Queue<IGameItem>();
        ViewManager.OnItemAdded += AddItemHandler;
        DeckSystem.OnHandUpdated += UpdateHandItems;
        StateLogicControl.OnEnterCombatState += OnEnterCombatStageHandler;
    }

    private void OnDisable()
    {
        ViewManager.OnItemAdded -= AddItemHandler;
        DeckSystem.OnHandUpdated -= UpdateHandItems;
        StateLogicControl.OnEnterCombatState -= OnEnterCombatStageHandler;
    }

    private void UpdateHandItems(IGameItem[] newHand)
    {
        handItems = newHand;
    }

    private void AddItemHandler(int fromIndex, int toIndex)
    {   
        Debug.Log("AddItemHandler");
        try
        {
            Debug.Log("Added: " + handItems[fromIndex].ItemName + " to position: " + toIndex);
            gameItems[toIndex] = handItems[fromIndex];
            OnNewItemAdded?.Invoke(gameItems[toIndex]);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnEnterCombatStageHandler()
    {
        GenerateCombatDict();
    }
    
    private void GenerateCombatDict()
    {   
        combatCooldownDict = new Dictionary<int, int>();
        for (var index = 0; index < gameItems.Count; index++)
        {
            if (gameItems[index] == null) continue;
            combatCooldownDict[index] = 0;
            OnItemActivated?.Invoke(index, gameItems[index]);
        }
    }
    
    public void TickUpdateItems()
    {
        tick++;
        for (var index = 0; index < gameItems.Count; index++)
        {   
            var c = gameItems[index];
            if (c == null) continue;
            if (combatCooldownDict.TryGetValue(index, out var cd))
            {   
                if (cd >= gameItems[index].Cooldown*20)
                {
                    combatCooldownDict[index] = 0;
                    itemQueue.Enqueue(c);
                    OnItemActivated?.Invoke(index, gameItems[index]);
                }
                else
                {
                    combatCooldownDict[index] += 1;
                }
            }
        }
    }
    
    public void ProcessItemQueue(int t)
    {
        if (itemQueue.Count == 0 || itemQueue != null)
        {
            while (itemQueue.Count > 0)
            {   
                var item =  itemQueue.Dequeue();
                System.Reflection.MemberInfo info = item.GetType();
                object[] attributes = info.GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    switch (attribute)
                    {
                        case DamageDealer dealer:
                            ProcessDamageDealer(dealer);
                            break;
                        case ShieldGiver:
                            break;
                        case DebugLogger debugLogger:
                            Debug.Log(debugLogger.message);
                            break;
                    }
                }
            }
        }
    }
    
    private void ProcessDamageDealer(DamageDealer dmgDealer)
    {
        switch (dmgDealer.to)
        {
            case Target.ChosenEnemy:
                eSystem.ReduceEnemyHP((int)dmgDealer.damageAmount);
                break;
            case Target.Player:
                break;
            case Target.ChosenItem:
                break;
        }
    }
}
