using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common;

namespace Assets.Scripts.Units.Enemy
{
    public class LesserSkeleton : EnemyMobBase
    {
        public LesserSkeleton()
        {
            CurrentHP = 100;
            MaxHP = CurrentHP;
            AttackSpeed = 40;
            Damage = 9;
            Range = 7;
            AlarmRange = 20;
            AttackType = AttackType.Melee;

            Level = 1;
            Value = 1;
        }
    }
}
