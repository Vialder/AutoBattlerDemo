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

[DebugLogger("logger test")]
[DamageDealer(Target.ChosenEnemy, damageAmount: 2)]
public readonly struct TestItem1 : IGameItem
{
    public string ItemName => "Test Item 1";
    public string ItemDescription => "Test Item 1";
    public int Cost => 1;
    public int Cooldown => 2;
    public Sprite ItemSprite => Resources.Load<Sprite>("TestItem1");
}


[ShieldGiver(Target.Player, shieldAmount: 2)]
public readonly struct TestItem2 : IGameItem
{
    public string ItemName => "Test Item 2";
    public string ItemDescription => "Test Item 2";
    public int Cost => 2;
    public int Cooldown => 3;
    public Sprite ItemSprite => Resources.Load<Sprite>("TestItem2");
}
