using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Reflection;
using CardArchetypes;

#pragma warning disable CS4014
public class ActionProcessor : MonoBehaviour
{
    [SerializeField] private int tick;
    
    private Queue<IGameItem> itemQueue;
    //private Queue<EnemyEffect> enemyQueue;
    private Queue<string> debugTickQueue;
    public ItemManager iManager;
    public EnemySystem eManager;
    private PlayerState playerState;

    private bool isCombat = false;

    public static event Action OnCombatEnd;
    
    private void Awake()
    {   
        StateLogicControl.OnEnterCombatState += StartCombatLoop;
        itemQueue = new Queue<IGameItem>();
        //enemyQueue = new Queue<EnemyEffect>();
        debugTickQueue =  new Queue<string>();
    }

    private void Start()
    {
        playerState = PlayerState.Instance;
    }

    private void OnDisable()
    {
        StateLogicControl.OnEnterCombatState -= StartCombatLoop;
    }
    
    private void StartCombatLoop()
    {
        isCombat = true;
        TickLoop();
    }

    private void EndCombat()
    {
        PlayerState.Instance.ResetPlayerShield();
        OnCombatEnd?.Invoke();
    }
    
    
    private async Awaitable TickLoop()
    {
        while (isCombat)
        {
            try
            {
                tick++;
                iManager.TickUpdateItems();
                //enemyQueue = eManager.TickUpdateEnemies();
                iManager.ProcessItemQueue(tick);
                if (!eManager.CheckAlive())
                {
                    isCombat = false;
                    await Awaitable.WaitForSecondsAsync(0.05f, Application.exitCancellationToken);
                    EndCombat();
                    return;
                }
                await Awaitable.WaitForSecondsAsync(0.05f, Application.exitCancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                break;
            }
        }
    }
}
