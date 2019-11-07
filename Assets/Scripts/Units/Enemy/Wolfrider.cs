using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common;

namespace Assets.Scripts.Units.Enemy
{
    public class Wolfrider : EnemyMobBase
    {
        public Wolfrider()
        {
            CurrentHP = 250;
            MaxHP = CurrentHP;
            AttackSpeed = 15;
            Damage = 12;
            Range = 7;
            AlarmRange = 20;
            AttackType = AttackType.Melee;

            Level = 2;
            Value = 2;
        }
    }
}
