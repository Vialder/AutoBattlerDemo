using System;
using UnityEngine;
namespace CardArchetypes
{   
    public class DamageDealer : Attribute
    {   
        public Target to;
        public float damageAmount;

        public DamageDealer(Target to, float damageAmount)
        {
            this.to = to;
            this.damageAmount = damageAmount;
        }
    }

    public class ShieldGiver : Attribute
    {
        public Target to;
        public int shieldAmount;

        public ShieldGiver(Target to, int shieldAmount)
        {
            this.to = to;
            this.shieldAmount = shieldAmount;
        }
    }

    public class DebugLogger : Attribute
    {
        public string message;
        public DebugLogger(string message)
        {
            this.message = message;
        }
    }
}
