using CardArchetypes;
using UnityEngine;
using System.Reflection;

public class ItemEntity
{
    public int position;
    public int cooldown;
    public int charges;
    public DamageDealer damageDealer;
    public ShieldGiver shieldGiver;
    public ChargeHaver chargeHaver;
    public AppliesStatus appliesStatus;

    public ItemEntity(IGameItem itemData)
    {
        this.cooldown = 0;
        
        MemberInfo info = itemData.GetType();
        ChargeHaver ch = info.GetCustomAttribute<ChargeHaver>(true);
        if (ch != null)
        {
            this.chargeHaver = ch;
            charges = ch.Charges;
        }
        DamageDealer dmgD = info.GetCustomAttribute<DamageDealer>(true);
        if (dmgD != null)
        {
            this.damageDealer = dmgD;
        }
        ShieldGiver sg = info.GetCustomAttribute<ShieldGiver>(true);
        if (sg != null)
        {
            this.shieldGiver = sg;
        }
        AppliesStatus applies = info.GetCustomAttribute<AppliesStatus>(true);
        if (applies != null)
        {
            this.appliesStatus = applies;
        }
    }
}
