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
    public class UnitBase : Unit
    {
        public override int CurrentHP { get; set; } = 100;
        public override int MaxHP { get; set; } = 100;
        public override float MoveSpeed { get; set; } = 10;

        public override int AlarmRange { get; set; } = 20;
        public override int Range { get; set; } = 7;
        public override int AttackSpeed { get; set; } = 150;
        public override int Damage { get; set; } = 11;
        public override float RotationSpeed { get; set; } = 10f;

        public override AttackType AttackType { get; set; } = AttackType.Melee;
        public override Command CurrentCommand { get; set; } = Command.Idle;
        public override GameObject Waypoint { get; set; }

        protected bool IsEnemyInRange = false;

        public override void OnAwake()
        {
        }

        public override void OnFixedUpdate()
        {
            attackCooldown -= 1;

            if (CurrentHP <= 0)
            {
                OnDeath();
                Destroy();
            }
        }

        public override void IssueOrder(Command order, GameObject target)
        {
            Target = target;
            CurrentCommand = order;

            UpdateAnimations();
            rigidbody.isKinematic = false;
        }

        private void UpdateAnimations()
        {
            if (animator != null)
            {
                switch (CurrentCommand)
                {
                    case Command.Move:
                    case Command.MoveAttackingEverythingOnTheWay:
                        animator.SetBool("IsAttacking", false);
                        animator.SetBool("IsMoving", true);
                        break;
                    case Command.Attack:
                        if (IsEnemyInRange)
                        {
                            if (attackCooldown <= 0)
                            {
                                animator.SetBool("IsMoving", false);
                                animator.SetBool("IsAttacking", true);
                            }
                            else
                            {
                                animator.SetBool("IsAttacking", false);
                                animator.SetBool("IsMoving", false);
                            }
                        }
                        else
                        {
                            animator.SetBool("IsAttacking", false);
                            animator.SetBool("IsMoving", true);
                        }
                        break;
                    case Command.Idle:
                    default:
                        animator.SetBool("IsAttacking", false);
                        animator.SetBool("IsMoving", false);
                        break;
                }
            }
        }

        public override void ProcessAI()
        {
            if (CurrentCommand == Command.Idle)
            {
                IssueOrder(Command.MoveAttackingEverythingOnTheWay, Waypoint);
            }

            switch (CurrentCommand)
            {
                case Command.Move:
                    MoveTo(Target);
                    break;
                case Command.Attack:
                    Attack(Target);
                    break;
                case Command.MoveAttackingEverythingOnTheWay:
                    MoveAttacking(Target);
                    break;
                case Command.Idle:
                default:
                    break;
            }

            if (CurrentCommand != Command.Idle)
                LookAt(Target);
        }

        public override void LookAt(GameObject target)
        {
            Vector3 lookPos = Target.gameObject.transform.position - transform.position;

            lookPos.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPos);

            if (Quaternion.Angle(transform.rotation, rotation) > 15)
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.5f);
        }

        public override void MoveAttacking(GameObject target)
        {
            if (target == null)
            {
                IssueOrder(Command.Idle, null);
                return;
            }
            //Temporary solution
            MoveToRange(target);
        }

        public override void Attack(GameObject target)
        {
            if (target == null)
            {
                IssueOrder(Command.Idle, null);
                return;
            }

            var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            IsEnemyInRange = distanceToTarget <= Range;

            if (IsEnemyInRange)
            {

                rigidbody.velocity = new Vector3();
                rigidbody.isKinematic = true;
                UpdateAnimations();

                if (attackCooldown <= 0)
                {
                    attackCooldown = AttackSpeed;
                    var targetUnit = target.gameObject.GetComponent<UnitImplementationContainer>().Unit;

                    if (AttackType == AttackType.Melee)
                    {
                       DealDamage(Damage, targetUnit);
                    }
                    else
                    {
                        FireProjectile(Damage, targetUnit);
                    }

                }
            }
            else
            {
                rigidbody.isKinematic = false;
                UpdateAnimations();
                MoveToRange(target);
            }
        }

        public override void MoveToRange(GameObject target)
        {
            if (Vector3.Distance(Target.transform.position, transform.position) <= Range)
                return;

            var destinationVector = Target.transform.position - transform.position;
            destinationVector.Normalize();

            var destination = transform.position + destinationVector * 2;

            if (Vector3.Distance(target.transform.position, transform.position) < 3)
            {
                navMeshAgent.velocity = Vector3.zero;
            }
            navMeshAgent.SetDestination(destination);
        }

        public override void FireProjectile(int damage, Unit targetUnit)
        {
        }

        public override void DealDamage(int damage, Unit to)
        {
            to.CurrentHP -= damage;
        }

        public override void MoveTo(GameObject target)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < 3)
            {
                navMeshAgent.velocity = Vector3.zero;
            }

            navMeshAgent.SetDestination(target.transform.position);
        }

        public override void OnDeath()
        {
            Player.Current.Gold += 1;
        }
    }
}
