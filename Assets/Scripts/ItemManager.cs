using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using CardArchetypes;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Queue<ItemEntity> itemQueue;
    private List<IGameItem> gameItems;
    private List<ItemEntity> itemEntities;
    private int tickDelta;
    private IGameItem[] handItems;
    private Dictionary<int, int> combatCooldownDict;
    [SerializeField] private int tick;
    public static event Action<int, IGameItem> OnItemActivated;
    public static event Action<IGameItem> OnNewItemAdded;
    public EnemySystem eSystem;
    
    private void Awake()
    {   
        itemEntities = new List<ItemEntity>
        {
           null,
           null,
           null,
           null,
           null
        };
        gameItems = new List<IGameItem>
        {
            null,
            null,
            null,
            null,
            null
        };
        itemQueue = new Queue<ItemEntity>();
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
            itemEntities[toIndex] = new ItemEntity(handItems[fromIndex]);;
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
        ResetItems();
    }
    
    private void ResetItems()
    {   
        for (var index = 0; index < gameItems.Count; index++)
        {
            if (gameItems[index] == null) continue;
            OnItemActivated?.Invoke(index, gameItems[index]);
        }

        for (var index = 0; index < itemEntities.Count; index++)
        {
            if (itemEntities[index] == null) continue;
            if (itemEntities[index].chargeHaver != null) itemEntities[index].charges = itemEntities[index].chargeHaver.Charges;
        }
    }
    
    public void TickUpdateItems(int t)
    {   
        tick = t;
        for (var index = 0; index < itemEntities.Count; index++)
        {
            var e = itemEntities[index];
            if (e == null) continue;
            e.cooldown++;
            if (e.cooldown >= gameItems[index].Cooldown*20)
            {   
                if (e.chargeHaver != null && e.charges <= 0) continue;
                if (e.chargeHaver != null && e.charges > 0)
                {
                    e.charges--;
                }
                e.cooldown = 0;
                itemQueue.Enqueue(e);
                OnItemActivated?.Invoke(index, gameItems[index]);
            }
        }
    }
    
    public void ProcessItemQueue(int t)
    {
        while (itemQueue.Count > 0)
        {   
           var item = itemQueue.Dequeue();
           if (item.damageDealer != null)
           {
               ProcessDamageDealer(item.damageDealer);
           }

           if (item.appliesStatus != null)
           {
                ApplyStatus(item.appliesStatus);   
           }
           
           if (item.shieldGiver != null)
           {
               ProcessShieldGiver(item.shieldGiver);
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

    private void ProcessShieldGiver(ShieldGiver shieldGiver)
    {   
        Debug.Log("gave shields at: " + tick);
        PlayerState.Instance.AdjustPlayerShield(shieldGiver.shieldAmount);
    }

    private void ApplyStatus(AppliesStatus status)
    {
        if (status.target == Target.ChosenEnemy)
        {
            Debug.Log("Enemy is on fire!");
        }
    }
}
