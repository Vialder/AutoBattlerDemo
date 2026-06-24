using TMPro;
using UnityEngine;

public class UITextValueSetter : MonoBehaviour
{   
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI enemyHPText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI drawCountText;
    public TextMeshProUGUI discardCountText;
    
    private void Awake()
    {
        PlayerState.OnPlayerHealthUpdated += SetPlayerHPText;
        EnemySystem.OnEnemyHealthUpdated += SetEnemyHPText;
        PlayerState.OnPlayerMoneyUpdated += SetMoneyText;
        //ItemDeckSystem.OnDrawCountUpdated += SetDrawCountText;
        //ItemDeckSystem.OnDiscardCountUpdated += SetDiscardCountText;
    }

    private void OnDisable()
    {
        PlayerState.OnPlayerHealthUpdated -= SetPlayerHPText;
        EnemySystem.OnEnemyHealthUpdated -= SetEnemyHPText;
        PlayerState.OnPlayerMoneyUpdated -= SetMoneyText;
        //ItemDeckSystem.OnDrawCountUpdated -= SetDrawCountText;
        //ItemDeckSystem.OnDiscardCountUpdated -= SetDiscardCountText;
    }
    
    private void SetPlayerHPText(int playerHP)
    {
        playerHPText.text = "HP: " + playerHP;
    }

    private void SetEnemyHPText(int enemyHP)
    {
        enemyHPText.text = "Enemy: " + enemyHP;
    }

    private void SetMoneyText(int money)
    {
        moneyText.text = "Money: " + money;
    }

    private void SetDrawCountText(int drawCount)
    {
        drawCountText.text = "Draw Pile: " + drawCount;
    }

    private void SetDiscardCountText(int discardCount)
    {
        discardCountText.text = "Discard Pile: " + discardCount;
    }
}
