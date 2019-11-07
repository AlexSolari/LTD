using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    public class Tower : Unit
    {
        public Tower()
        {
            CurrentHP = 500;
            MaxHP = CurrentHP;
            AttackSpeed = 70;
            Damage = 75;
            Range = 7;
            AlarmRange = 40;

            Level = 1;
            Value = 3;
        }
    }
}
