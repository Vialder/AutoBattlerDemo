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
    //private Queue<EnemyEffect> enemyQueue;
    private int enemyAttacksRemaining;
    private int attackCooldown;
    private int ticksPassed;
    [SerializeField] private int hp;
    public static event Action<int> OnEnemyHealthUpdated;
    
    private void Start()
    {
        hp = enemy.enemyHP;
        OnEnemyHealthUpdated?.Invoke(hp);
    }

    private void Awake()
    {
        attackCooldown = 4;
        //enemyQueue = new Queue<EnemyEffect>();
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
    /*
    public Queue<EnemyEffect> TickUpdateEnemies()
    {
        ticksPassed++;
        if (ticksPassed >= attackCooldown * 20)
        {
            enemyQueue.Enqueue(new EnemyEffect(1));
            ticksPassed = 0;
            AnimateSlider();
        }
        return enemyQueue;
    }
    */

    private void StartSlider()
    {
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
