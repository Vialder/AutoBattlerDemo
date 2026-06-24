using System;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using UnityEngine.UI;

public class EnemySystem : MonoBehaviour
{
    public SO_Enemy enemy;
    public RectTransform timerPanel;
    public Slider timerSlider;
    private Tween slideTween;
    private Queue<int> enemyQueue;
    private int enemyAttacksRemaining;
    private int attackCooldown;
    private int ticksPassed;
    [SerializeField] private int tick;
    [SerializeField] private int hp;
    public static event Action<int> OnEnemyHealthUpdated;
    
    private void Start()
    {   
        ticksPassed = 0;
        hp = enemy.enemyHP;
        tick = 0;
        OnEnemyHealthUpdated?.Invoke(hp);
    }

    private void Awake()
    {
        attackCooldown = 3;
        enemyQueue = new Queue<int>();
        StateLogicControl.OnEnterCombatState += StartSlider;
        StateLogicControl.OnEnterShopState += ResetEnemy;
    }
    
    private void OnDisable()
    {
        StateLogicControl.OnEnterCombatState -= StartSlider;
        StateLogicControl.OnEnterShopState -= ResetEnemy;
    }

    private void ResetEnemy()
    {
        hp = enemy.enemyHP;
        OnEnemyHealthUpdated?.Invoke(hp);
    }

    public bool CheckAlive()
    {
        if (hp <= 0)
        {
            if (slideTween.isAlive)
            {
                slideTween.Complete();
                timerSlider.value = 0;
            }
            return false;
        }
        return true;
    }
    
    public void ReduceEnemyHP(int amount)
    {
        hp -= amount;
        OnEnemyHealthUpdated?.Invoke(hp);
    }
    
    public Queue<int> TickUpdateEnemies(int t)
    {
        ticksPassed++;
        tick = t;
        if (ticksPassed >= attackCooldown * 20)
        {
            enemyQueue.Enqueue(3);
            ticksPassed = 0;
            AnimateSlider();
        }
        return enemyQueue;
    }
    

    private void StartSlider()
    {
        tick = 0;
        AnimateSlider();
    }
    
    private void AnimateSlider()
    {
        timerSlider.value = 0;
        timerSlider.maxValue = attackCooldown * 20;
        
        slideTween = Tween.Custom(0,timerSlider.maxValue, duration: attackCooldown, ease: Ease.Linear, onValueChange:
            newVal =>
            {
                if (Mathf.Abs(newVal - timerSlider.maxValue) < 0.01f)
                {
                    timerSlider.value = timerSlider.maxValue;
                    slideTween.Complete();
                }
                else
                {
                    timerSlider.value = newVal;
                }
            });
    }
}
