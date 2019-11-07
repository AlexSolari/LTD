using Assets.Scripts.Common;
using Assets.Scripts.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    public class TowerRanged : Unit
    {
        public TowerRanged()
        {
            CurrentHP = 100;
            MaxHP = CurrentHP;
            AttackSpeed = 85;
            Damage = 150;
            Range = 25;
            AlarmRange = 40;
            AttackType = AttackType.Ranged;

            Level = 2;
            Value = 4;
        }
    }
}
