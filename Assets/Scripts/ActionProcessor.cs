using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using System.Reflection;
using CardArchetypes;
using Debug = UnityEngine.Debug;

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
    private int fatigueDamage;
    private int fatigueInterval;
    private int ticksSinceFatigueTriggered;
    private Queue<int> enemyQueue;
    public static event Action OnCombatEnd;
    
    private void Awake()
    {   
        StateLogicControl.OnEnterCombatState += StartCombatLoop;
        itemQueue = new Queue<IGameItem>();
        //enemyQueue = new Queue<EnemyEffect>();
        debugTickQueue =  new Queue<string>();
        fatigueDamage = 1;
        fatigueInterval = 20;
        ticksSinceFatigueTriggered = 0;
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
        tick = 0;
        fatigueDamage = 1;
        StartCoroutine(CombatTicker());
    }

    private void EndCombat()
    {
        PlayerState.Instance.ResetPlayerShield();
        OnCombatEnd?.Invoke();
    }
    
    private IEnumerator CombatTicker()
    {
        while (isCombat)
        {
            iManager.TickUpdateItems(tick);
            iManager.ProcessItemQueue(tick);
            ProcessFatigueDamage();
            if (!eManager.CheckAlive())
            {
                isCombat = false;
                yield return new WaitForSeconds(0.05f);
                EndCombat();
                yield break;
            }
            enemyQueue = eManager.TickUpdateEnemies(tick);
            ProcessEnemyQueue();
            tick++;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void ProcessFatigueDamage()
    {   
        if (tick >= 20 * 20)
        {
            if (ticksSinceFatigueTriggered >= fatigueInterval)
            {
                Debug.Log("Dealing Fatigue Damage: " + fatigueDamage);
                eManager.ReduceEnemyHP(fatigueDamage);
                PlayerState.Instance.AdjustPlayerHealth(fatigueDamage);
                fatigueDamage++;
                ticksSinceFatigueTriggered = 0;
            }
            else
            {
                ticksSinceFatigueTriggered++;
            }
        }
       
    }

    private void ProcessEnemyQueue()
    {
        if (enemyQueue == null) return;
        while (enemyQueue.Count > 0)
        {
            var enemyEffect = enemyQueue.Dequeue();
            PlayerState.Instance.AdjustPlayerHealth(enemyEffect);
            //Debug.Log("Enemy deal damage at: " + tick);
        }
    }
}
