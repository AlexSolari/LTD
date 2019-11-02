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
    public class TowerRanged : UnitBase
    {
        public GameObject projectile;

        public TowerRanged()
        {
            CurrentHP = 100;
            MaxHP = CurrentHP;
            AttackSpeed = 85;
            Damage = 150;
            Range = 25;
            AlarmRange = 30;
            AttackType = AttackType.Ranged;
        }

        public override void FireProjectile(int damage, Unit targetUnit)
        {
            base.FireProjectile(damage, targetUnit);
            var spawnPoint = transform.position;
            spawnPoint.y += 3;
            var instance = Instantiate(projectile, spawnPoint, Quaternion.identity);
            var controller = instance.GetComponent<ProjectileController>();

            controller.Damage = Damage;
            controller.Emiter = this;
            controller.Target = targetUnit;
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
