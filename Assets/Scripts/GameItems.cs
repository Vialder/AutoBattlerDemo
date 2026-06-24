using CardArchetypes;
using UnityEngine;

public interface IGameItem
{
    public string ItemName {get;}
    public string ItemDescription {get;}
    public int Cost { get; }
    public int Cooldown { get; }
    public Sprite ItemSprite {get;}
    
}

[DamageDealer(Target.ChosenEnemy, damageAmount: 2)]
public readonly struct TestItem1 : IGameItem
{
    public string ItemName => "Wand o' Hurting";
    public string ItemDescription => "Deal (2) damage";
    public int Cost => 1;
    public int Cooldown => 2;
    public Sprite ItemSprite => Resources.Load<Sprite>("TestItem1");
}


[ShieldGiver(Target.Player, shieldAmount: 3)]
[ChargeHaver(charges: 3)]
public readonly struct TestItem2 : IGameItem
{
    public string ItemName => "Warding Bracelet";
    public string ItemDescription => "Gain (3) Shield";
    public int Cost => 2;
    public int Cooldown => 3;
    public Sprite ItemSprite => Resources.Load<Sprite>("TestItem2");
}

[ChargeHaver(charges: 3)]
[AppliesStatus(status: Status.Burn, amount: 1, Target.ChosenEnemy)]
public readonly struct Firecracker : IGameItem
{
    public string ItemName => "Firecrackers";
    public string ItemDescription => "Apply (1) Burn";
    public int Cost => 2;
    public int Cooldown => 3;
    public Sprite ItemSprite => Resources.Load<Sprite>("TestItem2");
}

