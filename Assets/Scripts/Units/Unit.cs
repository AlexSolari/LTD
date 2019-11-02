using Assets.Scripts.Common;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    public abstract class Unit
    {
        public abstract int AlarmRange { get; set; }
        public abstract int AttackSpeed { get; set; }
        public abstract AttackType AttackType { get; set; }
        public abstract Command CurrentCommand { get; set; }
        public abstract int CurrentHP { get; set; }
        public abstract int Damage { get; set; }
        public abstract int MaxHP { get; set; }
        public abstract float MoveSpeed { get; set; }
        public abstract int Range { get; set; }
        public abstract float RotationSpeed { get; set; }
        public abstract GameObject Waypoint { get; set; }

        public GameObject Target;
        public int attackCooldown = 0;

        public Rigidbody rigidbody;
        public Animator animator;
        public GameObject gameObject;
        public Transform transform;
        public NavMeshAgent navMeshAgent;

        public abstract void Attack(GameObject target);
        public abstract void DealDamage(int damage, Unit to);
        public abstract void IssueOrder(Command order, GameObject target);
        public abstract void LookAt(GameObject target);
        public abstract void MoveAttacking(GameObject target);
        public abstract void MoveTo(GameObject target);
        public abstract void ProcessAI();
        public abstract void FireProjectile(int damage, Unit targetUnit);
        public abstract void MoveToRange(GameObject target);
        public abstract void OnDeath();

        public abstract void OnAwake();
        public abstract void OnFixedUpdate();

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }

        public Func<GameObject, Vector3, Quaternion, GameObject> Instantiate { get; set; }
    }
}