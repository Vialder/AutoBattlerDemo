using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_InitPlayer", menuName = "SO/SO_InitPlayer")]
public class SO_InitPlayer : ScriptableObject
{
    public List<SO_Item> deck;
    public int playerHealth;
    public int playerMaxHealth;
    public int playerShield;
}
