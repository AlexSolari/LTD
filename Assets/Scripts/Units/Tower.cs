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
    public class Tower : UnitBase
    {
        public Tower()
        {
            CurrentHP = 500;
            MaxHP = CurrentHP;
            AttackSpeed = 25;
            Damage = 75;
            Range = 7;
            AlarmRange = 20;
        }

        private void FixedUpdate()
        {
            attackCooldown -= 1;

            if (CurrentHP <= 0)
            {
                Destroy();
            }
        }

        public override void OnDeath()
        {
        }
    }
}
