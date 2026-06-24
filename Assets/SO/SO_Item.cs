using UnityEngine;

[CreateAssetMenu(fileName = "SO_Item", menuName = "SO/SO_Item")]
public class SO_Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int itemUseCharges;
    public int itemCooldown;
    public int damageToEnemy;
    public int shieldToPlayer;
    public float critChance;
    public float critDamage;
}
