using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common;

namespace Assets.Scripts.Units.Enemy
{
    public class Lich : EnemyMobBase
    {
        public Lich()
        {
            CurrentHP = 200;
            MaxHP = CurrentHP;
            AttackSpeed = 50;
            Damage = 35;
            Range = 25;
            AlarmRange = 30;
            AttackType = AttackType.Ranged;

            Level = 3;
            Value = 3;
        }
    }
}
